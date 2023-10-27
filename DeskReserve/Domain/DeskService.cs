using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Domain
{
	public class DeskService
	{
		private readonly ApplicationDbContext _dbContext;

		public DeskService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public List<Desk> GetAllDesks()
		{
			return _dbContext.Desks.ToList();
		}

		public async Task<Desk> GetDesk(Guid id)
		{
			return await _dbContext.Desks.FindAsync(id);
		}

		public async Task<bool> UpdateDesk(Guid id, Desk desk)
		{
			if (id != desk.DeskId)
			{
				return false;
			}

			_dbContext.Entry(desk).State = EntityState.Modified;

			try
			{
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (DbUpdateConcurrencyException)
			{
				return false;
			}
		}

		public async Task<Desk> CreateDesk(Desk desk)
		{
			_dbContext.Desks.Add(desk);
			await _dbContext.SaveChangesAsync();
			return desk;
		}

		public async Task<bool> DeleteDesk(Guid id)
		{
			var desk = await _dbContext.Desks.FindAsync(id);
			if (desk == null)
			{
				return false;
			}

			_dbContext.Desks.Remove(desk);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}