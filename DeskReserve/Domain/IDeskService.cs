using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
	public interface IDeskService
	{
		Task<IEnumerable<Desk>> GetAllAsync();

		Task<DeskDto> GetOneAsync(Guid id);

		Task<bool> UpdateOneAsync(Guid id, DeskDto deskDto);

		Task<bool> CreateOneAsync(DeskDto desk);

		Task<bool> DeleteOneAsync(Guid id);
		
	}
}
