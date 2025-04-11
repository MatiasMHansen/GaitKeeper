using Dapr;
using Dapr.Client;
using GaitSessionService.Application.Command;
using GaitSessionService.Application.Command.CommandDTOs;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using Shared.DTOs.PubSubDTOs;

namespace GaitSessionService.API.Controllers
{
    [ApiController]
    [Route("gaitsession")]
    public class SaveGaitSessionController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly IGaitSessionCommand _gaitSessionCommand;
        private readonly ILogger<SaveGaitSessionController> _log;

        public SaveGaitSessionController(DaprClient daprClient, IGaitSessionCommand gaitSessionCommand, ILogger<SaveGaitSessionController> logger)
        {
            _daprClient = daprClient;
            _gaitSessionCommand = gaitSessionCommand;
            _log = logger;
        }

        [Topic("pubsub", "save-gait-session")]
        public async Task<IActionResult> HandleSaveGaitSessionEvent([FromBody] CreateGaitSessionDTO gaitSessionDTO)
        {
            try
            {
                _log.LogInformation($"SaveGaitSessionController received save request for: {gaitSessionDTO.FileName} - Published from GaitDataOrchestrator");

                // TODO: Kald IGaitSessionCommand
                await _gaitSessionCommand.CreateAsync(gaitSessionDTO);

                // Send status tilbage til Orchestrator
                await _daprClient.PublishEventAsync("pubsub", "save-status", new SaveStatusDTO
                {
                    Service = "GaitPointDataService",
                    CorrelationId = gaitSessionDTO.PointDataId.Value,
                    Success = true,
                });

                _log.LogWarning($"The successful save-status for {gaitSessionDTO.PointDataId} is published");

                return Ok();
            }
            catch (Exception ex)
            {
                // Send fejlstatus tilbage til Orchestrator
                await _daprClient.PublishEventAsync("pubsub", "save-status", new SaveStatusDTO
                {
                    Service = "GaitSessionService",
                    CorrelationId = gaitSessionDTO.PointDataId.Value,
                    Success = false,
                    ErrorMessage = ex.Message
                });

                _log.LogWarning($"The failed save-status for {gaitSessionDTO.PointDataId} is published");

                return StatusCode(500);
            }
        }
    }
}
