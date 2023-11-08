using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Repository
{
	public interface IDeskRepository
	{
		Task<IEnumerable<Desk>> GetAll();

		Task<Desk> GetById(Guid id);

		Task<bool> Update(Desk desk);

		Task<bool> Create(Desk floor);

		Task<bool> Delete(Guid id);
	}
}
