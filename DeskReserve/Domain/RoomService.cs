using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
	public class RoomService
	{
		private readonly ApplicationDbContext _dbContext;

		public RoomService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public List<Room> GetAllDogs()
		{
			return _dbContext.Rooms.ToList();
		}
	}
}