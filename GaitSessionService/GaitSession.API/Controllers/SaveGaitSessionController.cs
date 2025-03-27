using Dapr;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.PubSubDTOs;

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
        public async Task<IActionResult> HandleSaveGaitSessionEvent([FromBody] GaitDataKeysDTO gaitDataKeys)
        {
            try
            {
                Console.WriteLine($"✅ Received GaitSession save request for: {gaitDataKeys.FileName}");
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
