using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Interfaces
{
    public interface IBuildingService
    {
        Task<List<Building>> GetAll();
        Task<Building> GetBuildingById(Guid id);
        Task<bool> AddNew(BuildingDto building);
        Task<bool> DeleteBuilding(Guid id);
        Task<bool> UpdateBuilding(Guid id, BuildingDto building);
    }
}