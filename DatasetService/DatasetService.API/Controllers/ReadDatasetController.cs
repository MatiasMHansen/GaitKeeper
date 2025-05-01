using DatasetService.Application.Query;
using Microsoft.AspNetCore.Mvc;

namespace DatasetService.API.Controllers
{
    [ApiController]
    [Route("dataset/read")]
    public class ReadDatasetController : ControllerBase
    {
        private readonly IDatasetQuery _query;

        public ReadDatasetController(IDatasetQuery query)
        {
            _query = query;
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _query.GetAsync(id);
            if (result == null)
                return NotFound($"Couldn't find any GaitSession related to: {id}");

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllIds()
        {
            var result = await _query.GetAllAsync();
            return Ok(result);
        }
    }
}
