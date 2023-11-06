using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
    public interface IBuildingService
    {
        Task<List<Building>> GetAll();
        Task<Building> GetOne(Guid id);
        Task<bool> NewEntity(BuildingDto building);
        Task<bool> Erase(Guid id);
        Task<bool> Update(Guid id, BuildingDto building);
    }
}
