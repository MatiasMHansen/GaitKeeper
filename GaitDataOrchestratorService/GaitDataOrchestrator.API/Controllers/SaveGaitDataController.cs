using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.PubSubDTOs;

namespace GaitDataOrchestrator.API.Controllers
{
    [ApiController]
    [Route("gaitdata")]
    public class SaveGaitDataController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public SaveGaitDataController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [HttpPost("post")]
        public async Task<IActionResult> OrchestrarteSaveGaitData([FromBody] string fileName)
        {
            try
            {
                Console.WriteLine($"📩 Received save request for: {fileName} -> Now orchestrating the process...");

                if (fileName == null)
                    return BadRequest(new { error = "Invalid Gait Data request" });

                // Orkestere processe med PubSub og RabbitMQ
                var correlationId = Guid.NewGuid();
                var message = new
                {
                    FileName = fileName,
                    CorrelationId = correlationId
                };

                // Publish til RabbitMQ via Dapr PubSub
                await _daprClient.PublishEventAsync("pubsub", "save-gait-session", message);
                await _daprClient.PublishEventAsync("pubsub", "save-point-data", message);
                // await _daprClient.PublishEventAsync("pubsub", "save-analog-data", message); Ikke implementeret, endnu...

                Console.WriteLine($"✅ All events for {fileName} have successfully been published");

                return Ok(new { Message = $"Events er publiceret for: {message.FileName}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in 'OrchestrarteSaveGaitData' method: {ex.Message}");

                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Topic("pubsub", "save-status")]
        [HttpPost("status")]
        public async Task<IActionResult> HandleSaveStatus([FromBody] SaveStatusDTO status)
        {
            Console.WriteLine($"📩 Status received from: {status.Service}, Success: {status.Success}");

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
                    Console.WriteLine($"❌ Failure detected – initier rollback for {correlationId}");

                    await _daprClient.PublishEventAsync("pubsub", "rollback-gaitdata", correlationId);
                }
                else
                {
                    Console.WriteLine($"✅ All saves succeeded for {correlationId} – GaitData saved!");
                }

                // 5. Ryd op – slet nøgle fra Redis
                await _daprClient.DeleteStateAsync("statestore", stateKey);
            }

            return Ok();
        }
    }
}
