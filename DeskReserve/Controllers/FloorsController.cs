using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorsController : ControllerBase
    {
        private readonly FloorService _floorService;

        public FloorsController(FloorService floorService)
        {
            _floorService = floorService;
        }

        // GET: api/Floors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Floor>>> GetFloors()
        {
            var floors = await _floorService.GetFloors();
            return Ok(floors);
        }

        // GET: api/Floors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Floor>> GetFloor(Guid id)
        {
            var floor = await _floorService.GetFloor(id);

            if (floor == null)
            {
                return NotFound();
            }

            return floor;
        }

        // PUT: api/Floors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFloor(Guid id, Floor floor)
        {
            if (id != floor.FloorId)
            {
                return BadRequest();
            }

            var success = await _floorService.UpdateFloor(id, floor);

            return success ? NoContent() : NotFound();
        }

        // POST: api/Floors
        [HttpPost]
        public async Task<ActionResult<Floor>> PostFloor(Floor floor)
        {
            var newFloor = await _floorService.CreateFloor(floor);
            return CreatedAtAction("GetFloor", new { id = newFloor.FloorId }, newFloor);
        }

        // DELETE: api/Floors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFloor(Guid id)
        {
            var success = await _floorService.DeleteFloor(id);

            return success ? NoContent() : NotFound();
        }
    }
}
