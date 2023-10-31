using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Domain
{
    public class BuildingService
    {
        private readonly ApplicationDbContext _dbContext;

        public BuildingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Building>> GetAllBuildings()
        {
            return await _dbContext.Buildings.ToListAsync();
        }

        public async Task<Building> GetBuildingById(Guid id)
        {
            var building = await _dbContext.Buildings.FindAsync(id);

            return building;
        }

        public async Task<Building> PostBuilding(Building building)
        {
            _dbContext.Buildings.Add(building);
            await _dbContext.SaveChangesAsync();

            return building;
        }

        public async Task<bool> DeleteBuilding(Guid id)
        {
            var building = await _dbContext.Buildings.FindAsync(id);

            if (building == null)
            {
                return false;
            }

            _dbContext.Buildings.Remove(building);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBuilding(Guid id, Building building)
        {
            var buildingExist = await _dbContext.Buildings.FindAsync(id);

            if (buildingExist == null)
            {
                return false;
            }
        
            _dbContext.Entry(buildingExist).State = EntityState.Detached;
            _dbContext.Buildings.Update(building);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
