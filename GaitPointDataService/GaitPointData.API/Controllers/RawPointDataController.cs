using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.RawGaitData;

namespace GaitPointData.API.Controllers
{
    [ApiController]
    [Route("pointdata")]
    public class RawPointDataController : ControllerBase // FORÆLDET????? 
    {
        private readonly DaprClient _daprClient;

        public RawPointDataController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        // Kalder PythonC3DReader via Dapr Service Invocation -> Henter PointData fra C3D-fil
        [HttpGet("raw/{fileName}")]
        public async Task<IActionResult> GetRawPointData(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("File name is required.");

            try
            {
                var response = await _daprClient.InvokeMethodAsync<RawPointDataDTO>(
                    HttpMethod.Get,
                    "c3dreader", // AppId fra AppHost
                    $"pointdata/{fileName}" // Endpoint i PythonC3DReader
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
