using Dapr;
using Dapr.Client;
using GaitPointData.Application.Command;
using GaitPointData.Application.Command.CommandDTOs;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.PubSubDTOs;

namespace GaitPointData.API.Controllers
{
    [ApiController]
    [Route("pointdata")]
    public class SavePointDataController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly IPointDataCommand _pointDataCommand;
        private readonly ILogger<SavePointDataController> _log;

        public SavePointDataController(DaprClient daprClient, IPointDataCommand pointDataCommand, ILogger<SavePointDataController> logger)
        {
            _daprClient = daprClient;
            _pointDataCommand = pointDataCommand;
            _log = logger;
        }

        [Topic("pubsub", "save-point-data")]
        public async Task<IActionResult> HandleSavePointDataEvent([FromBody] GaitDataKeysDTO gaitDataKeys)
        {
            try
            {
                _log.LogInformation($"SavePointDataController received save request for: {gaitDataKeys.FileName} - Published from GaitDataOrchestrator");

                // GET: Kald PythonC3DReader for PointData
                var createPointDataDTO = await _daprClient.InvokeMethodAsync<CreatePointDataDTO>(
                    HttpMethod.Get,
                    "c3dreader", // AppId fra AppHost
                    $"pointdata/{gaitDataKeys.FileName}" // Endpoint i PythonC3DReader
                );

                // Inject Id
                createPointDataDTO.Id = gaitDataKeys.CorrelationId;

                // TODO: Kald IPointDataCommand
                await _pointDataCommand.CreateAsync(createPointDataDTO);

                // Send status tilbage til Orchestrator
                await _daprClient.PublishEventAsync("pubsub", "save-status", new SaveStatusDTO
                {
                    Service = "GaitPointDataService",
                    CorrelationId = gaitDataKeys.CorrelationId,
                    Success = true,
                });

                _log.LogInformation($"The successful save-status for {gaitDataKeys.CorrelationId} is published");
                
                return Ok();
            }
            catch (Exception ex)
            {
                // Send fejlstatus tilbage til Orchestrator
                await _daprClient.PublishEventAsync("pubsub", "save-status", new SaveStatusDTO
                {
                    Service = "GaitPointDataService",
                    CorrelationId = gaitDataKeys.CorrelationId,
                    Success = false,
                    ErrorMessage = ex.Message
                });

                _log.LogWarning($"The failed save-status for {gaitDataKeys.CorrelationId} is published");

                return StatusCode(500);
            }
        }
    }
}
