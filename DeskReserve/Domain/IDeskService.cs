using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
	public interface IDeskService
	{
		Task<IEnumerable<Desk>> GetAllAsync();

		Task<Desk> GetOneAsync(Guid id);

		Task<bool> UpdateOneAsync(Guid id, Desk desk);

		Task<Desk> CreateOneAsync(Desk floor);

		Task<bool> DeleteOneAsync(Guid id);
		
	}
}
