using Dapr;
using GaitSessionService.Application.Command;
using Microsoft.AspNetCore.Mvc;

namespace GaitSessionService.API.Controllers
{
    [ApiController]
    [Route("gaitsession/rollback")]
    public class RollbackGaitSessionController : ControllerBase
    {
        private readonly IGaitSessionCommand _gaitSessionCommand;
        private readonly ILogger<RollbackGaitSessionController> _log;

        public RollbackGaitSessionController(IGaitSessionCommand gaitSessionCommand, ILogger<RollbackGaitSessionController> logger)
        {
            _gaitSessionCommand = gaitSessionCommand;
            _log = logger;
        }

        [Topic("pubsub", "rollback-gaitdata")]
        public async Task<IActionResult> HandleRollback([FromBody] Guid correlationId)
        {
            _log.LogInformation($"RollbackGaitSessionController received rollback request for: {correlationId} Published from GaitDataOrchestrator");

            try
            {
                await _gaitSessionCommand.DeleteAsync(correlationId);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
