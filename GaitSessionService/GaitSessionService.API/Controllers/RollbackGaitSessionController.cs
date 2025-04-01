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

        public RollbackGaitSessionController(IGaitSessionCommand gaitSessionCommand)
        {
            _gaitSessionCommand = gaitSessionCommand;
        }

        [Topic("pubsub", "rollback-gaitdata")]
        public async Task<IActionResult> HandleRollback([FromBody] Guid correlationId)
        {
            Console.WriteLine($"RollbackGaitSessionController received rollback request for: {correlationId} Published from GaitDataOrchestrator");

            try
            {
                await _gaitSessionCommand.DeleteAsync(correlationId);
                Console.WriteLine($"Success! Rolled back GaitSession for: {correlationId}");
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
