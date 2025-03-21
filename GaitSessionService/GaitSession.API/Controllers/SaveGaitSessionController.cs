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

        [HttpPost("post")]
        public async Task<IActionResult> SaveGaitSession([FromBody] GaitMetadataDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { error = "Invalid Gait Data request" });

                // Simulér databasegemning (udvid senere med en service)
                await Task.Delay(100);

                return Ok(new { Message = "GaitData saved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // forsøg på at løse CORS error - JEG SKAL MULIGVIS SLETTES!!!
        [HttpOptions("post")]
        public IActionResult Preflight()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7089");
            Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "*");
            return Ok();
        }
    }
}
