using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DeskReserve.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase, IBuildingController
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService ?? throw new ArgumentNullException(nameof(buildingService));
        }

        [HttpGet]
        public async Task<ActionResult<List<Building>>> Get()
        {
            var buildings = await _buildingService.GetAll();
            return buildings;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetById(Guid id)
        {
            var building = await _buildingService.GetOne(id);

            return building != null ? Ok(building) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Building>> Post(Building building)
        {
            if (building == null)
            {
                return BadRequest();
            }

            var buildingAdd = await _buildingService.NewEntity(building);
            //return StatusCode(201, buildingAdd);
            return buildingAdd;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletionResult = await _buildingService.Erase(id);

            return deletionResult ? NotFound() : NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Building building)
        {
            if (id != building.BuildingId)
            {
                return BadRequest();
            }

            var result = await _buildingService.Update(id, building);

            return result ? NotFound() : NoContent();
        }
    }
}
