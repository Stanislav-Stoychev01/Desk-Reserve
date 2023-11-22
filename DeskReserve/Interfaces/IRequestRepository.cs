using DeskReserve.Data.DBContext.Entity;

namespace RequestReserve.Interfaces
{
	public interface IRequestRepository
	{
		Task<IEnumerable<Request>> GetAll();

		Task<Request> GetById(Guid id);

		Task<bool> Create(Request floor);

		Task<bool> Uppdate(Request request);
	}
}