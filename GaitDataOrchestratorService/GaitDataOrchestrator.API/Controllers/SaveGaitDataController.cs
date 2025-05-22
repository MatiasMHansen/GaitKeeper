using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.PubSubDTOs;

namespace GaitDataOrchestrator.API.Controllers
{
    [ApiController]
    [Route("gaitdata")]
    public class SaveGaitDataController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<SaveGaitDataController> _log;

        public SaveGaitDataController(DaprClient daprClient, ILogger<SaveGaitDataController> logger)
        {
            _daprClient = daprClient;
            _log = logger;
        }

        [HttpPost("post")]
        public async Task<IActionResult> OrchestrarteSaveGaitData([FromBody] CreateGaitSessionDTO gaitSessionDTO)
        {
            try
            {
                _log.LogInformation($"SaveGaitDataController received save request for: {gaitSessionDTO.FileName} -> Now orchestrating the process...");

                if (gaitSessionDTO.FileName == null)
                    return BadRequest(new { error = "Invalid Gait Data request" });

                // Orkestere processe med PubSub
                var correlationId = Guid.NewGuid();
                var message = new
                {
                    FileName = gaitSessionDTO.FileName,
                    CorrelationId = correlationId,
                };

                gaitSessionDTO.PointDataId = correlationId;

                // Publish til Message-broker via Dapr PubSub
                await _daprClient.PublishEventAsync("pubsub", "save-gait-session", gaitSessionDTO);
                await _daprClient.PublishEventAsync("pubsub", "save-point-data", message);

                // await _daprClient.PublishEventAsync("pubsub", "save-analog-data", message); Ikke implementeret, endnu...

                _log.LogInformation($"Success! All events for {gaitSessionDTO.FileName} have successfully been published -> Id:{correlationId}");

                return Ok(new { Message = $"Saving...: {message.FileName}" });
            }
            catch (Exception ex)
            {
            _log.LogError($"EXCEPTION - OrchestrarteSaveGaitData: {ex.Message}");

                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Topic("pubsub", "save-status")]
        [HttpPost("status")]
        public async Task<IActionResult> HandleSaveStatus([FromBody] SaveStatusDTO status)
        {
            _log.LogInformation($"SaveGaitDataController received a Status from: {status.Service} regarding {status.CorrelationId}, Success: {status.Success}");

            var correlationId = status.CorrelationId;
            var stateKey = $"status-{correlationId}";

            // 1. Hent eksisterende statusliste fra Redis state store
            var statuses = await _daprClient.GetStateAsync<List<SaveStatusDTO>>("statestore", stateKey)
                           ?? new List<SaveStatusDTO>();

            // 2. Tilføj ny status
            statuses.Add(status);

            // 3. Gem den opdaterede liste tilbage i state store
            await _daprClient.SaveStateAsync("statestore", stateKey, statuses);

            // 4. Evaluer hvis vi har modtaget fra alle forventede services
            if (statuses.Count >= 2) // TODO: juster til 3 når AnalogData kommer
            {
                if (statuses.Any(x => !x.Success))
                {
                    _log.LogWarning($"WARNING - Failure detected, initiate rollback for {correlationId}");

                    await _daprClient.PublishEventAsync("pubsub", "rollback-gaitdata", correlationId);
                }
                else
                {
                    _log.LogInformation($"Success! All saves succeeded for {correlationId} – GaitData saved!");
                }

                // 5. Ryd op – slet nøgle fra Redis
                await _daprClient.DeleteStateAsync("statestore", stateKey);
            }

            return Ok();
        }
    }
}
