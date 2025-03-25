using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace GaitDataOrchestrator.API.Controllers
{
    [ApiController]
    [Route("gaitdata")]
    public class SaveGaitSessionController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public SaveGaitSessionController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [HttpPost("post")]
        public async Task<IActionResult> OrchestrarteSaveGaitData([FromBody] GaitDataDTO gaitData)
        {
            try
            {
                if (gaitData == null)
                    return BadRequest(new { error = "Invalid Gait Data request" });

                // Orkestere processe med PubSub og RabbitMQ
                var correlationId = Guid.NewGuid().ToString();
                var message = new
                {
                    FileName = gaitData.FileName,
                    SubjectId = gaitData.SubjectId,
                    CorrelationId = correlationId
                };

                // Publish til RabbitMQ via Dapr PubSub
                await _daprClient.PublishEventAsync("pubsub", "save-gait-session", message);
                await _daprClient.PublishEventAsync("pubsub", "save-point-data", message);
                // await _daprClient.PublishEventAsync("pubsub", "save-analog-data", message); Ikke implementeret endnu

                return Ok(new { Message = $"Events er publiceret for: {message.FileName}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
