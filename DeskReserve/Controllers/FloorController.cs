using Microsoft.AspNetCore.Mvc;
using DeskReserve.Interfaces;
using DeskReserve.Domain;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace DeskReserve.Controllers
{
    [Route("api/floors")]
    [ApiController]
    public class FloorController : Controller
    {
        private readonly IFloorService _service;

        public FloorController(IFloorService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [Authorize(Roles ="User")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Floor>>> Get()
        {
            var floors = await _service.GetAllAsync();

            return !ReferenceEquals(floors, null) ? Ok(floors) : NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FloorDto>> Get(Guid id)
        {
            var floor = await _service.GetOneAsync(id);

            return !ReferenceEquals(floor, null) ? Ok(floor) : NotFound();
        }

        [Authorize]
        [HttpPut("{id}/edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, FloorDto floor)
        {
            var success = await _service.UpdateOneAsync(id, floor);

            return success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FloorDto>> Post(FloorDto floor)
        {
            var success = await _service.CreateOneAsync(floor);

            return success ? StatusCode(StatusCodes.Status201Created) : BadRequest();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteOneAsync(id);
                return Ok();
            } 
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
