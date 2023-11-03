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
            return !Object.Equals(buildings, null) ? Ok(buildings) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetById(Guid id)
        {
            var building = await _buildingService.GetOne(id);
            return !Object.Equals(building, null) ? Ok(building) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Building>> Post(BuildingDto building)
        {
            var buildingAdd = await _buildingService.NewEntity(building);

            return !Object.Equals(buildingAdd, null) ? StatusCode(201, buildingAdd) : StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletionResult = await _buildingService.Erase(id);

            return deletionResult ? Ok() : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, BuildingDto building)
        {
            var updateResult = await _buildingService.Update(id, building);

            return updateResult ? Ok() : NotFound();
        }
    }
}
