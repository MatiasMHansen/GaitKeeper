using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.RawGaitData;

namespace GaitSessionService.API.Controllers
{
    [ApiController]
    [Route("gaitsession")]
    public class RawGaitSessionController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public RawGaitSessionController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        // Kalder PythonC3DReader via Dapr Service Invocation -> Henter GaitSession-data fra C3D-fil
        [HttpGet("raw/{fileName}")]
        public async Task<IActionResult> GetRawGaitSession(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("File name is required.");

            try
            {
                var response = await _daprClient.InvokeMethodAsync<RawGaitSessionDTO>(
                    HttpMethod.Get,
                    "c3dreader", // AppId fra AppHost
                    $"gaitsession/{fileName}" // Endpoint i PythonC3DReader
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
