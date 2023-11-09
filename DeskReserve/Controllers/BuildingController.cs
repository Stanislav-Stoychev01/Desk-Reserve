using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using DeskReserve.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskReserve.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService ?? throw new ArgumentNullException(nameof(buildingService));
        }

        [HttpGet]
        public async Task<ActionResult<List<Building>>> Get()
        {
            ActionResult result;

            try
            {
                var buildings = await _buildingService.GetAll();
                result = Ok(buildings);
            }
            catch (Exception ex)
            {
                result = StatusCode(500, ex.Message);
            }

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetById(Guid id)
        {
            ActionResult result;

            try
            {
                var building = await _buildingService.GetBuildingById(id);
                result = Ok(building);
            }
            catch (DataNotFound ex)
            {
                result = NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                result = StatusCode(500, ex.Message);
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BuildingDto building)
        {
            ActionResult result;

            try
            {
                var buildingIsAdded = await _buildingService.AddNew(building);
                result = StatusCode(201);
            }
            catch (Exception ex)
            {
                result = StatusCode(500, ex.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            ActionResult result;

            try
            {
                var deletionResult = await _buildingService.DeleteBuilding(id);
                result = Ok(deletionResult);
            }
            catch (DataNotFound ex)
            {
                result = NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                result = StatusCode(500,ex.Message);
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, BuildingDto building)
        {
            ActionResult result;

            try
            {
                var updateResult = await _buildingService.UpdateBuilding(id, building);
                result = Ok(updateResult);
            }
            catch (DataNotFound ex)
            {
                result = NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                result = StatusCode(500, ex.Message);
            }

            return result;
        }
    }
}
