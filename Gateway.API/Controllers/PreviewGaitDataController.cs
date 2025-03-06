using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.RawGaitData;

namespace Gateway.API.Controllers
{
    [ApiController]
    [Route("gateway/preview")]
    public class PreviewGaitDataController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public PreviewGaitDataController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        // Proxy for GaitSessionService's GetRawGaitSession
        [HttpGet("gaitsession/{fileName}")]
        public async Task<IActionResult> GetRawGaitSession(string fileName)
        {
            var response = await _daprClient.InvokeMethodAsync<RawGaitSessionDTO>(
                HttpMethod.Get,
                "gaitsessionservice", // AppId fra AppHost
                $"gaitsession/raw/{fileName}" // Endpoint i GaitSessionService
            );

            return Ok(response);
        }
    }
}
