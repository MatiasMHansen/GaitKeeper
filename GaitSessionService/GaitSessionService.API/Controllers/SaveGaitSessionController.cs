using Dapr;
using Dapr.Client;
using GaitSessionService.Application.Command;
using GaitSessionService.Application.Command.CommandDTOs;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.PubSubDTOs;

namespace GaitSessionService.API.Controllers
{
    [ApiController]
    [Route("gaitsession")]
    public class SaveGaitSessionController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly IGaitSessionCommand _gaitSessionCommand;

        public SaveGaitSessionController(DaprClient daprClient, IGaitSessionCommand gaitSessionCommand)
        {
            _daprClient = daprClient;
            _gaitSessionCommand = gaitSessionCommand;
        }

        [Topic("pubsub", "save-gait-session")]
        public async Task<IActionResult> HandleSaveGaitSessionEvent([FromBody] CreateGaitSessionDTO gaitSessionDTO)
        {
            try
            {
                Console.WriteLine($"SaveGaitSessionController received save request for: {gaitSessionDTO.FileName} - Published from GaitDataOrchestrator");


                // TODO: Kald IGaitSessionCommand
                await _gaitSessionCommand.CreateAsync(gaitSessionDTO);

                // Send status tilbage til Orchestrator
                await _daprClient.PublishEventAsync("pubsub", "save-status", new SaveStatusDTO
                {
                    Service = "GaitPointDataService",
                    CorrelationId = gaitSessionDTO.PointDataId.Value,
                    Success = true,
                });

                Console.WriteLine($"Success! GaitSession from {gaitSessionDTO.FileName} have been saved - SQL key: xxxxx");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION - HandleSaveGaitSessionEvent failed trying to save '{gaitSessionDTO.FileName}': {ex.Message}");

                // Send fejlstatus tilbage til Orchestrator
                await _daprClient.PublishEventAsync("pubsub", "save-status", new SaveStatusDTO
                {
                    Service = "GaitSessionService",
                    CorrelationId = gaitSessionDTO.PointDataId.Value,
                    Success = false,
                    ErrorMessage = ex.Message
                });

                return StatusCode(500);
            }
        }
    }
}
