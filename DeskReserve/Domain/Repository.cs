﻿using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;



namespace DeskReserve.Domain
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Building>> GetAllAsync()
        {
            return await _dbContext.Buildings.ToListAsync();
        }

        public async Task<Building> GetByIdAsync(Guid id)
        {
            return await _dbContext.Buildings.FindAsync(id);
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
            var buildingItem = await _dbContext.Buildings.FindAsync(toUpdate.BuildingId);
            _dbContext.Entry(buildingItem).State = EntityState.Detached;
            _dbContext.Entry(toUpdate).State = EntityState.Modified;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
