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
            return await _buildingService.GetAll(); ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetById(Guid id)
        {
            ActionResult result;

            if (Validate.IsNull(id))
            {
                result = BadRequest();
            }
            else
            {
                var building = await _buildingService.GetOne(id);
                result = building != null ? Ok(building) : NotFound();
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Building>> Post(Building building)
        {
            ActionResult result;

            if (Validate.IsNull(building))
            {
                result = BadRequest();
            }
            else
            {
                var buildingAdd = await _buildingService.NewEntity(building);
                result = StatusCode(201, buildingAdd);
            }

            return result;
        }

        [HttpDelete("{id}")]
         public async Task<IActionResult> Delete(Guid id)
         {
            IActionResult result;

            if (Validate.IsNull(id))
            {
                result = BadRequest();
            }
            else
            {
                var deletionResult = await _buildingService.Erase(id);
                result = deletionResult ? Ok() : NotFound();
            }

            return result;
         }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Building building)
        {
            IActionResult result;

            if (Validate.IsNull(id) || Validate.IsNull(building) || building.BuildingId != id)
            {
                result = BadRequest();
            }
            else
            {
                var updateResult = await _buildingService.Update(building);
                result = updateResult ? Ok() : NotFound();
            }

            return result;
        }
    }
}
