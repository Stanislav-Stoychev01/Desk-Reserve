using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Repository
{
	public interface IRoomRepository
	{
		Task<IEnumerable<Room>> GetAllAsync();

		Task<Room> GetOneAsync(Guid id);

		Task<bool> UpdateOneAsync(Room room);

		Task<bool> CreateOneAsync(Room room);

		Task<bool> DeleteOneAsync(Guid id);
	}
}