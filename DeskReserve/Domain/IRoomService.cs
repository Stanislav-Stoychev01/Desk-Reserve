using DeskReserve.Data.DBContext.Entity;
using System.Drawing;

namespace DeskReserve.Domain
{
	public interface IRoomService
	{
		Task<IEnumerable<Room>> GetAllAsync();
		Task<Room> GetOneAsync(Guid id);
		Task<bool> UpdateOneAsync(Guid id, Room room);
		Task<Room> CreateOneAsync(Room room);
		Task<bool> DeleteOneAsync(Guid id);
	}
}
