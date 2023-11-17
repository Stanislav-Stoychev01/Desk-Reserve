using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;

namespace DeskReserve.Repository
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BuildingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Building>> GetAllAsync()
        {
<<<<<<< HEAD
			var buildings = _dbContext.Buildings
	            .Include(b => b.Floors)
                .ToList();
=======
            var buildings = await _dbContext.Buildings
				.Include(b => b.Floors).ToListAsync();
>>>>>>> 86971e14444a370dbdc684ca42931a3b44925ef4

			return buildings;
		}

        public async Task<Building> GetByIdAsync(Guid id)
        {
            return await _dbContext.Buildings.FindAsync(id) ?? throw new DataNotFound(ExceptionMessages.DataNotFound);
        }

        public async Task<bool> CreateAsync(Building newBuilding)
        {
            _dbContext.Buildings.Add(newBuilding);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Building toDelete)
        {
            _dbContext.Buildings.Remove(toDelete);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Building toUpdate)
        {
            var buildingItem = await GetByIdAsync(toUpdate.BuildingId);
            _dbContext.Entry(buildingItem).State = EntityState.Detached;
            _dbContext.Entry(toUpdate).State = EntityState.Modified;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}