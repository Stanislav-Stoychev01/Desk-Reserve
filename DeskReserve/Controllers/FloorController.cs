using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Net.Mime;

namespace DeskReserve.Controllers
{
    [Route("api/floors")]
    [ApiController]
    public class FloorController : ControllerBase, IFloorController
    {
        private readonly IFloorService _floorService;

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService ?? throw new ArgumentNullException(nameof(floorService));
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Floor>>> GetAll()
        {
            var floors = await _floorService.GetAllAsync();
            return Ok(floors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Floor>> GetById(Guid id)
        {
            var floor = await _floorService.GetOneAsync(id);

            if (floor == null)
            {
                return NotFound();
            }

            return floor;
        }

        [HttpPut("edit/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOne(Guid id, Floor floor)
        {
            var success = await _floorService.UpdateOneAsync(id, floor);

            return success ? NoContent() : NotFound();
        }

        [HttpPost("new")]
        public async Task<ActionResult<Floor>> Create(Floor floor)
        {
            var newFloor = await _floorService.CreateOneAsync(floor);
            return StatusCode(StatusCodes.Status201Created, newFloor);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _floorService.DeleteOneAsync(id);

            return success ? NoContent() : NotFound();
        }
    }
}
