using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using DeskReserve.Repository;

namespace DeskReserve.Service
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;

        public BuildingService(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
        }

        public async Task<List<Building>> GetAll()
        {
            return await _buildingRepository.GetAllAsync();
        }

        public async Task<Building> GetBuildingById(Guid id)
        {
            return await _buildingRepository.GetByIdAsync(id) ;
        }

        public async Task<bool> AddNew(BuildingDto building)
        {
            var newBuilding = building.ToBuilding();
            return await _buildingRepository.CreateAsync(newBuilding);
        }

        public async Task<bool> DeleteBuilding(Guid id)
        {
            bool result;
            var existingBuilding = await _buildingRepository.GetByIdAsync(id);

            result = await _buildingRepository.DeleteAsync(existingBuilding);
            return result;
        }

        public async Task<bool> UpdateBuilding(Guid id, BuildingDto building)
        {
            bool result;
            var existingBuilding = await _buildingRepository.GetByIdAsync(id) ?? throw new DataNotFound();

            var updateBuilding = building.ToBuilding(id);
            result = await _buildingRepository.UpdateAsync(updateBuilding);
            
            return result;
        }
    }
}