using GaitSessionService.Application.Query;
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
                return NotFound();

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _query.GetAllAsync();
            return Ok(result);
        }
    }
}
