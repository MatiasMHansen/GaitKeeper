using GaitSessionService.Application.Query;
using GaitSessionService.Application.Query.QueryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace GaitSessionService.API.Controllers
{
    [ApiController]
    [Route("gaitsession")]
    public class ReadGaitSessionController : ControllerBase
    {
        private readonly IGaitSessionQuery _query;

        public ReadGaitSessionController(IGaitSessionQuery query)
        {
            _query = query;
        }

        [HttpGet("by-pointdata/{pointDataId:guid}")]
        public async Task<IActionResult> GetByPointDataId(Guid pointDataId)
        {
            var result = await _query.GetAsync(pointDataId);
            if (result == null)
                return NotFound($"Couldn't find any GaitSession related to: {pointDataId}");

            return Ok(result);
        }

        [HttpPost("by-pointdata")]
        public async Task<IActionResult> GetByPointDataIds([FromBody] List<Guid> pointDataIds)
        {
            if (pointDataIds == null || !pointDataIds.Any())
                return BadRequest("At least one PointDataId must be provided.");

            var queryGaitSessionDto = await _query.GetAsync(pointDataIds);
            return Ok(queryGaitSessionDto);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var queryGaitSessionDtos = await _query.GetAllAsync();
            return Ok(queryGaitSessionDtos);
        }
    }
}
