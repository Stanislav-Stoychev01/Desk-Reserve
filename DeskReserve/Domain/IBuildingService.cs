using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Domain
{
    public interface IBuildingService
    {
        Task<List<Building>> GetAll();
        Task<Building> GetOne(Guid id);
        Task<Building> NewEntity(Building building);
        Task<bool> Erase(Guid id);
        Task<bool> Update(Building building);
    }
}
