using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace DeskReserve.Domain
{
    public class BuildingService : IBuildingService
    {
        private readonly IRepository _buildingRepository;

        public BuildingService(IRepository buildingRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
        }

        public async Task<List<Building>> GetAll()
        {
            return await _buildingRepository.GetAllAsync();
        }

        public async Task<Building> GetOne(Guid id)
        {
            return await _buildingRepository.GetByIdAsync(id);
        }

        public async Task<Building> NewEntity(Building building)
        {
            return await _buildingRepository.CreateAsync(building);
        }

        public async Task<bool> Erase(Guid id)
        {
              var existingBuilding = await _buildingRepository.GetByIdAsync(id);
            if (existingBuilding == null)
            {
                return false;
            }

            return await _buildingRepository.DeleteAsync(existingBuilding);
        }

        public async Task<bool> Update(Building building)
        {
            return await _buildingRepository.UpdateAsync(building);
        }
    }
}




