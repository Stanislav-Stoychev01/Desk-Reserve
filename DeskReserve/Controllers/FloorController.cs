using Microsoft.AspNetCore.Mvc;
using DeskReserve.Interfaces;
using DeskReserve.Domain;

namespace DeskReserve.Controllers
{
    [Route("api/floors")]
    [ApiController]
    public class FloorController : Controller
    {
        private readonly IFloorService _service;

        public FloorController(IFloorService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FloorDto>>> Get()
        {
            var entities = await _service.GetAllAsync();
            return entities != null ? Ok(entities) : NotFound();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FloorDto>> Get(Guid id)
        {
            var entity = await _service.GetOneAsync(id);

            return entity != null ? Ok(entity) : NotFound();
        }

        [HttpPut("{id}/edit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(Guid id, FloorDto floor)
        {
            await _service.UpdateOneAsync(id, floor);

            return NoContent();
        }

        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<FloorDto>> Post(FloorDto floor)
        {
            var success = await _service.CreateOneAsync(floor);

            return success ? StatusCode(StatusCodes.Status201Created) : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteOneAsync(id);

            return success ? NoContent() : NotFound();
        }
    }
}
