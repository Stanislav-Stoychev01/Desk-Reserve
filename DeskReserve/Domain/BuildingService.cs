using DeskReserve.Data.DBContext.Entity;


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

        public async Task<Building> NewEntity(BuildingDto building)
        {
            var newBuilding = UpgradeDto(building);
            return await _buildingRepository.CreateAsync(newBuilding);
        }

        public async Task<bool> Erase(Guid id)
        {
            bool result;
            var existingBuilding = await _buildingRepository.GetByIdAsync(id);

            if (Object.Equals(existingBuilding, null))
            {
                result = false;
            }
            else
            {
                result = await _buildingRepository.DeleteAsync(existingBuilding); ;
            }

            return result;
        }

        public async Task<bool> Update(Guid id, BuildingDto building)
        {
            bool result;
            var existingBuilding = await _buildingRepository.GetByIdAsync(id);

            if (Object.Equals(existingBuilding, null))
            {
                result = false;
            }
            else
            {
                var updateBuilding = UpgradeDto(id, building);
                result = await _buildingRepository.UpdateAsync(updateBuilding);
            }

            return result;
        }

        public Building UpgradeDto(BuildingDto buildingDto)
        {
            var building = new Building
            {
                BuildingId = Guid.NewGuid(),
                City = buildingDto.City,
                StreetAddress = buildingDto.StreetAddress,
                Neighbourhood = buildingDto.Neighbourhood,
                Floors = buildingDto.Floors
            };
            return building;
        }

        public Building UpgradeDto(Guid id,BuildingDto buildingDto)
        {
            var building = new Building
            {
                BuildingId = id,
                City = buildingDto.City,
                StreetAddress = buildingDto.StreetAddress,
                Neighbourhood = buildingDto.Neighbourhood,
                Floors = buildingDto.Floors
            };
            return building;
        }
    }
}




