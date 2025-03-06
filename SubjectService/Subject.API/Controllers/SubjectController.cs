using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.RawGaitData;

namespace Subject.API.Controllers
{
    [ApiController]
    [Route("gaitsession")]
    public class SubjectController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public SubjectController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        // Simpel endpoint til at teste, om SubjectService er oppe
        [HttpGet]
        public IActionResult GetServiceInfo()
        {
            return Ok("Hello from SubjectService");
        }

        // Kalder PythonC3DReader via Dapr Service Invocation -> Henter metadata fra C3D-fil
        [HttpGet("raw/{fileName}")]
        public async Task<IActionResult> GetRawC3DMetadata(string fileName)
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
