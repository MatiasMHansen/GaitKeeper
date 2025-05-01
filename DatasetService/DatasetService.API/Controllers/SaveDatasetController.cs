using Dapr.Client;
using DatasetService.API.DTOs.Requests;
using DatasetService.Application.Command;
using DatasetService.Application.Command.CommandDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DatasetService.API.Controllers
{
    [ApiController]
    [Route("dataset")]
    public class SaveDatasetController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly IDatasetCommand _command;

        public SaveDatasetController(DaprClient daprClient, IDatasetCommand command)
        {
            _daprClient = daprClient;
            _command = command;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveDataset([FromBody] CreateDatasetRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || request.PointDataIds == null || !request.PointDataIds.Any())
                return BadRequest("Name and PointDataIds are required.");

            // Fetch data:
            var json = JsonSerializer.Serialize(request.PointDataIds);
            List<PartialGaitSessionDTO> gaitSession;

            try
            {
                gaitSession = await _daprClient.InvokeMethodAsync<List<Guid>, List<PartialGaitSessionDTO>>(
                    HttpMethod.Post,
                    "gaitsessionservice",
                    "gaitsession/by-pointdata",
                    request.PointDataIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching data from internal services: {ex.Message}");
            }

            // Saml CreateDatasetDTO
            var createDatasetDto = new CreateDatasetDTO
            {
                Name = request.Name,
                gaitSessions = gaitSession,
            };

            // Kald Command:
            await _command.CreateAsync(createDatasetDto);

            return Ok("Dataset have successfully been created.");
        }
    }
}
