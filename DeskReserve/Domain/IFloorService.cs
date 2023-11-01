using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
    public interface IFloorService
    {
        Task<IEnumerable<Floor>> GetAllAsync();

        Task<Floor> GetOneAsync(Guid id);

        Task<bool> UpdateOneAsync(Guid id, Floor floor);

        Task<Floor> CreateOneAsync(Floor floor);

        Task<bool> DeleteOneAsync(Guid id);
    }
}
