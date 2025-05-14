using Dapr.Client;
using DatasetService.API.DTOs.Requests;
using DatasetService.Application.Command;
using DatasetService.Application.Command.CommandDTOs;
using DatasetService.Application.Query;
using DatasetService.Application.Utility;
using DatasetService.Application.Utility.UtilDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DatasetService.API.Controllers
{
    [ApiController]
    [Route("dataset/print/")]
    public class PrintDatasetController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly IExportDataset _export;
        private readonly IDatasetQuery _query;

        public PrintDatasetController(DaprClient daprClient, IExportDataset export, IDatasetQuery query)
        {
            _daprClient = daprClient;
            _export = export;
            _query = query;
        }

        [HttpGet("characteristics/{id:guid}")]
        public async Task<IActionResult> PrintCharacteristicsToCSV(Guid id)
        {
            var csvContent = await _export.PrintCharacteristicToCSV(id);
            var bytes = Encoding.UTF8.GetBytes(csvContent);
            var dataset = await _query.GetAsync(id);
            var fileName = $"Characteristics_{dataset.Name}.csv";

            return File(bytes, "text/csv", fileName);
        }


        [HttpPost("marker-axis")]
        public async Task<IActionResult> PrintMarkerAxisToCSV([FromBody] PrintMarkerAxisRequest request)
        {
           
            // Fetch Dataset:
            var dataset = await _query.GetAsync(request.Id);

            // Setup & prepare request
            List<PartialPointDataDTO> pointData;

            var pointDataRequest = new GetPartialPointDataRequest()
            {
                PointDataIds = dataset.Subjects
                         .Select(s => s.PointDataId)
                         .ToList(),
                Labels = new List<string> { request.MarkerLabel }
            };

            try
            {
                pointData = await _daprClient.InvokeMethodAsync<GetPartialPointDataRequest, List<PartialPointDataDTO>>(
                    HttpMethod.Post,
                    "gaitpointdataservice",
                    "partialpointdata/by-labels",
                    pointDataRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching data from internal services: {ex.Message}");
            }

            // Print til CSV:
            var csvContent = await _export.PrintMarkerToCSV(dataset, pointData, request.MarkerLabel, request.Axis);

            // Returnér som file download:
            var bytes = Encoding.UTF8.GetBytes(csvContent);
            var fileName = $"MarkerData_{dataset.Name}_{request.MarkerLabel}_{request.Axis}.csv";

            return File(bytes, "text/csv", fileName);
        }
    }
}
