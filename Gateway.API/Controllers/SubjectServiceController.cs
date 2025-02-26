using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared.DTOs;

namespace Gateway.API.Controllers
{
    [ApiController]
    [Route("gateway/subject")]
    public class SubjectServiceController : ControllerBase
    {
        private readonly DaprClient _daprClient;

        public SubjectServiceController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        // Proxy for SubjectService's GetRawC3DMetadata
        [HttpGet("raw-c3d-metadata/{fileName}")]
        public async Task<IActionResult> GetRawC3DMetadata(string fileName)
        {
            var response = await _daprClient.InvokeMethodAsync<RawMetadataDTO>(
                HttpMethod.Get,
                "subjectservice", // AppId fra AppHost
                $"subject/raw-c3d-metadata/{fileName}" // Endpoint i SubjectService
            );

            return Ok(response);
        }
    }
}
