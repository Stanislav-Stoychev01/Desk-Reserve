using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly BuildingService _buildingService;

        public BuildingController(BuildingService buildingService)
        {
            _buildingService = buildingService ?? throw new ArgumentNullException(nameof(buildingService));
        }

        [HttpGet]
        public IEnumerable<Building> Get()
        {
            return _buildingService.GetAllBuildings();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetId(Guid id)
        {
            var building = await _buildingService.GetBuildingById(id);

            if (building == null)
            {
                return NotFound();
            }

            return building;
        }

        [HttpPost]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
            if (building == null)
            {
                return BadRequest();
            }

            var buildingAdd = await _buildingService.PostBuilding(building);
            return CreatedAtAction(nameof(GetId), new { id = buildingAdd.BuildingId }, buildingAdd);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(Guid id)
        {
            return await _buildingService.DeleteBuilding(id);
        }
    }
}
//6c167556-5e80-449f-2633-08dbd62410c6
