using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DeskReserve.Domain
{
	public class RoomService : IRoomService
	{
		private readonly ApplicationDbContext _context;

		public RoomService(ApplicationDbContext context)
		{
			_context = context;
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		//public async Task<List<Room>> GetRooms()
		public async Task<IEnumerable<Room>> GetAllAsync()
		{
			return await _context.Rooms.ToListAsync();
		}

		//public async Task<Room> GetRoom(Guid id)
		public async Task<Room> GetOneAsync(Guid id)
		{
			return await _context.Rooms.FindAsync(id);
		}

		public async Task<bool> UpdateOneAsync(Guid id, Room Room)
		{
			if (id != Room.RoomId)
			{
				return false;
			}

			_context.Entry(Room).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				return true;
			}
			catch (DbUpdateConcurrencyException)
			{
				return false;
			}
		}

		public async Task<Room> CreateOneAsync(Room Room)
		{
			_context.Rooms.Add(Room);
			await _context.SaveChangesAsync();
			return Room;
		}

		public async Task<bool> DeleteOneAsync(Guid id)
		{
			var Room = await _context.Rooms.FindAsync(id);

			if (Room == null)
			{
				return false;
			}

			_context.Rooms.Remove(Room);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
