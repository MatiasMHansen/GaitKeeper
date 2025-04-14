using GaitPointData.API.DTO_s.Requests;
using GaitPointData.Application.Query;
using Microsoft.AspNetCore.Mvc;

namespace GaitPointData.API.Controllers
{
    [ApiController]
    [Route("partialpointdata")]
    public class ReadPartialPointDataController : ControllerBase
    {
        private readonly IPartialPointDataQuery _query;

        public ReadPartialPointDataController(IPartialPointDataQuery query)
        {
            _query = query;
        }

        [HttpPost("by-labels")]
        public async Task<IActionResult> GetByLabels([FromBody] GetPartialPointDataRequest request)
        {
            if (request == null || !request.PointDataIds.Any() || !request.Labels.Any())
                return BadRequest("Request must include: PointDataId(s) and Labels.");

            var result = await _query.GetAsync(request.PointDataIds, request.Labels);
            return Ok(result);
        }
    }
}
