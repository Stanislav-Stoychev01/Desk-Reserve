using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace DeskReserve.Domain
{
    public class BuildingService
    {
        private readonly ApplicationDbContext _dbContext;

        public BuildingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<Building> GetAllBuildings()
        {
            return _dbContext.Buildings.ToList();
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

        public async Task<IActionResult> DeleteBuilding(Guid id)
        {
            var building = await _dbContext.Buildings.FindAsync(id);
            if (building == null)
            {
                return new NotFoundResult();
            }

            _dbContext.Buildings.Remove(building);
            await _dbContext.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
