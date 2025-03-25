using Dapr;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace GaitPointData.API.Controllers
{
    [ApiController]
    [Route("pointdata")]
    public class SavePointDataController : ControllerBase
    {
        public SavePointDataController()
        {

        }

        [Topic("pubsub", "save-point-data")]
        public async Task<IActionResult> HandleSavePointDataEvent([FromBody] GaitDataEventDTO gaitDataEvent)
        {
            try
            {
                Console.WriteLine($"✅ Received PointData save request for: {gaitDataEvent.FileName}, SubjectId: {gaitDataEvent.SubjectId}");
                // TODO: Kald PythonC3DReader for PointData
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
