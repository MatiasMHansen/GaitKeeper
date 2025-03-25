using Dapr;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace GaitSession.API.Controllers
{
    [ApiController]
    [Route("gaitsession")]
    public class SaveGaitSessionController : ControllerBase
    {
        public SaveGaitSessionController()
        {
            
        }

        [Topic("pubsub", "save-gait-session")]
        public async Task<IActionResult> HandleSaveGaitSessionEvent([FromBody] GaitDataEventDTO gaitDataEvent)
        {
            try
            {
                Console.WriteLine($"✅ Received GaitSession save request for: {gaitDataEvent.FileName}, SubjectId: {gaitDataEvent.SubjectId}");
                // TODO: Kald PythonC3DReader for GaitSession data
                // TODO: Valider & gem i database
                
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in subscriber: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
