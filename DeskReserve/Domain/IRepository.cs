﻿using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
    public interface IRepository
    {
        Task<List<Building>> GetAllAsync();
        Task<Building> GetByIdAsync(Guid id);
        Task<Building> CreateAsync(Building newBuilding);
        Task<bool> DeleteAsync(Building toDelete);
        Task<bool> UpdateAsync(Building toUpdate);
    }
}