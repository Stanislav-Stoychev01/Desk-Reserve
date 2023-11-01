using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Domain
{
	public class DeskService : IDeskService
	{
		private readonly ApplicationDbContext _dbContext;

		public DeskService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<IEnumerable<Desk>> GetAllAsync()
		{
			return await _dbContext.Desks.ToListAsync();
		}

		public async Task<Desk> GetOneAsync(Guid id)
		{
			return await _dbContext.Desks.FindAsync(id);
		}

		public async Task<bool> UpdateOneAsync(Guid id, Desk desk)
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

		public async Task<Desk> CreateOneAsync(Desk desk)
		{
			_dbContext.Desks.Add(desk);
			await _dbContext.SaveChangesAsync();
			return desk;
		}

		public async Task<bool> DeleteOneAsync(Guid id)
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