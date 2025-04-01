using Dapr;
using GaitPointData.Application.Command;
using Microsoft.AspNetCore.Mvc;

namespace GaitPointData.API.Controllers
{
    [ApiController]
    [Route("pointdata/rollback")]
    public class RollbackPointDataController : ControllerBase
    {
        private readonly IPointDataCommand _pointDataCommand;

        public RollbackPointDataController(IPointDataCommand pointDataCommand)
        {
            _pointDataCommand = pointDataCommand;
        }

        [Topic("pubsub", "rollback-gaitdata")]
        public async Task<IActionResult> HandleRollback([FromBody] Guid correlationId)
        {
            Console.WriteLine($"RollbackPointDataController received rollback request for: {correlationId} Published from GaitDataOrchestrator");

            try
            {
                await _pointDataCommand.DeleteAsync(correlationId);
                Console.WriteLine($"Success! Rolled back PointData for: {correlationId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION - HandleRollback failed rollback for {correlationId}: {ex.Message}");
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
