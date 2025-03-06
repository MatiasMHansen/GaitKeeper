using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.RawGaitData;

namespace Gateway.API.Controllers
{
    [ApiController]
    [Route("gateway/preview")]
    public class SubjectServiceController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public SubjectServiceController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        // Proxy for SubjectService's GetRawC3DMetadata
        [HttpGet("gaitsession/{fileName}")]
        public async Task<IActionResult> GetRawC3DMetadata(string fileName)
        {
            var response = await _daprClient.InvokeMethodAsync<RawGaitSessionDTO>(
                HttpMethod.Get,
                "subjectservice", // AppId fra AppHost
                $"gaitsession/raw/{fileName}" // Endpoint i SubjectService
            );

            return Ok(response);
        }
    }
}
