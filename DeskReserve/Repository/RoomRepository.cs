using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;

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

		public async Task<Room> GetById(Guid id)
		{
			return await _context.Rooms.FindAsync(id);
		}

		public async Task<bool> Create(Room room)
		{
			await _context.Rooms.AddAsync(room);

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> Update(Room room)
		{
			var exists = await _context.Rooms.FindAsync(room.RoomId) ?? throw new EntityNotFoundException();
			_context.Entry(room).State = EntityState.Modified;

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> Delete(Guid roomId)
		{
			var roomEntity = await _context.Rooms.FindAsync(roomId) ?? throw new EntityNotFoundException();

			_context.Rooms.Remove(roomEntity);

			return await _context.SaveChangesAsync() > 0;
		}
	}
}