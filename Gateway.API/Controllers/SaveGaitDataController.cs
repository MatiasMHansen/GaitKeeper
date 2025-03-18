using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers
{
    public class SaveGaitDataController : ControllerBase
    {
        //private readonly DaprClient _daprClient;

        //// Proxy for GaitSessionService's PutGaitSession
        //public async Task<IActionResult> PutGaitSession()
        //{
        //    var response = await _daprClient.InvokeMethodAsync<CreateGaitSessionDTO>(
        //        HttpMethod.Post,
        //        "gaitsessionservice", // AppId fra AppHost
        //        $"" // Endpoint i GaitSessionService
        //    );

        //    return Ok(response);
        //}
    }
}
