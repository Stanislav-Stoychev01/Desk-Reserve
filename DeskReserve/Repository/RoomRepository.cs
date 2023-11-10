using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Exceptions;
using DeskReserve.Domain;

namespace DeskReserve.Repository
{
	public class RoomRepository : IRoomRepository
	{
		private readonly ApplicationDbContext _context;

		public RoomRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Room>> GetAllAsync()
		{
			return await _context.Rooms.ToListAsync();
		}

		public async Task<Room> GetOneAsync(Guid id)
		{
			return await _context.Rooms.FindAsync(id);
		}

		public async Task<bool> CreateOneAsync(Room room)
		{
			await _context.Rooms.AddAsync(room);

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateOneAsync(Room room)
		{
			var exict = await _context.Rooms.FindAsync(room.RoomId) ?? throw new EntityNotFoundException();
			_context.Entry(room).State = EntityState.Modified;

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> DeleteOneAsync(Guid roomId)
		{
			var roomEntity = await _context.Rooms.FindAsync(roomId) ?? throw new EntityNotFoundException();

			_context.Rooms.Remove(roomEntity);

			return await _context.SaveChangesAsync() > 0;
		}
	}
}