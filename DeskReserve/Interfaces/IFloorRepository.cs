using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Interfaces
{
    public interface IFloorRepository
    {
        Task<IEnumerable<Floor>> GetAll();

        Task<Floor> GetById(Guid id);

        Task<bool> Add(Floor floor);

        Task<bool> Update(Floor floor);

        Task<bool> Delete(Guid id);
    }
}
