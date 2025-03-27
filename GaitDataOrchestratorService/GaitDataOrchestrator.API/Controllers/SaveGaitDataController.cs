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
                // await _daprClient.PublishEventAsync("pubsub", "save-analog-data", message); Ikke implementeret endnu

                Console.WriteLine($"✅ All events for {fileName} have successfully been published");

                return Ok(new { Message = $"Events er publiceret for: {message.FileName}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in 'OrchestrarteSaveGaitData' method: {ex.Message}");

                return StatusCode(500, new { error = ex.Message });
            }
        }

        //[Topic("pubsub", "save-status")]
        //[HttpPost("status")]
        //public async Task<IActionResult> HandleSaveStatus([FromBody] SaveStatusDTO status)
        //{
        //    Console.WriteLine($"📩 Status received from: {status.Service}, Success: {status.Success}");

        //    // TODO: Gem status i hukommelse eller persistent storage
        //    // Fx: add to Dictionary<string, List<SaveStatusDto>> based on CorrelationId

        //    var correlationId = status.CorrelationId;

        //    // Simpel mock – du skal erstatte med en rigtig status-tracking service
        //    StatusStore.Track(correlationId, status);

        //    var all = StatusStore.Get(correlationId);

        //    if (all.Count >= 2) // Fx GaitSession + PointData (ændres når du har 3)
        //    {
        //        if (all.Any(x => !x.Success))
        //        {
        //            Console.WriteLine($"❌ Failure detected – initier rollback for {correlationId}");

        //            await _daprClient.PublishEventAsync("pubsub", "rollback-gaitdata", new
        //            {
        //                CorrelationId = correlationId
        //            });
        //        }
        //        else
        //        {
        //            Console.WriteLine($"✅ All saves succeeded for {correlationId} – GaitData saved!");
        //        }

        //        // Ryd op efter orchestration
        //        StatusStore.Remove(correlationId);
        //    }

        //    return Ok();
        //}

    }
}
