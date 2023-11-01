using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Interfaces
{
    public interface IFloorService
    {
        Task<IEnumerable<Floor>> GetAllAsync();

        Task<FloorDto> GetOneAsync(Guid id);

        Task<bool> UpdateOneAsync(Guid id, FloorDto floor);

        Task<bool> CreateOneAsync(FloorDto floor);

        Task<bool> DeleteOneAsync(Guid id);
    }
}
