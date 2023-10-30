﻿using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System;

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
            var deletionResult = await _buildingService.DeleteBuilding(id);

            if (deletionResult)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(Guid id, Building building)
        {
            if (id != building.BuildingId)
            {
                return BadRequest();
            }

            var result = await _buildingService.UpdateBuilding(id, building);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
//6c167556-5e80-449f-2633-08dbd62410c6
