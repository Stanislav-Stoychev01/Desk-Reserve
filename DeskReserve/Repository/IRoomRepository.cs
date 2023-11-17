using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Repository
{
	public interface IRoomRepository
	{
		Task<IEnumerable<Room>> GetAllAsync();

		Task<Room> GetById(Guid id);

		Task<bool> Update(Room room);

		Task<bool> Create(Room room);

		Task<bool> Delete(Guid id);
	}
}