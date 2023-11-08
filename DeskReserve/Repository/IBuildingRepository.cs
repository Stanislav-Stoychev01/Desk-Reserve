using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Repository
{
    public interface IBuildingRepository
    {
        Task<List<Building>> GetAllAsync();
        Task<Building> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Building newBuilding);
        Task<bool> DeleteAsync(Building toDelete);
        Task<bool> UpdateAsync(Building toUpdate);
    }
}