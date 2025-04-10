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
        private readonly ILogger<RollbackPointDataController> _log;

        public RollbackPointDataController(IPointDataCommand pointDataCommand, ILogger<RollbackPointDataController> logger)
        {
            _pointDataCommand = pointDataCommand;
            _log = logger;
        }

        [Topic("pubsub", "rollback-gaitdata")]
        public async Task<IActionResult> HandleRollback([FromBody] Guid correlationId)
        {
            _log.LogInformation($"RollbackPointDataController received rollback request for: {correlationId} Published from GaitDataOrchestrator");

            try
            {
                await _pointDataCommand.DeleteAsync(correlationId);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
