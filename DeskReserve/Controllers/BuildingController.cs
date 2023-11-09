using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using DeskReserve.Service;
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
            try
            {
                var buildings = await _buildingService.GetAll();
                return Ok(buildings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetById(Guid id)
        {
            try
            {
                var building = await _buildingService.GetBuildingById(id);
                return Ok(building);
            }
            catch (DataNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(BuildingDto building)
        {
            try
            {
                var buildingIsAdded = await _buildingService.AddNew(building);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deletionResult = await _buildingService.DeleteBuilding(id);
                return Ok(deletionResult);
            }
            catch (DataNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, BuildingDto building)
        {
            try
            {
                var updateResult = await _buildingService.UpdateBuilding(id, building);
                return Ok(updateResult);
            }
            catch (DataNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
