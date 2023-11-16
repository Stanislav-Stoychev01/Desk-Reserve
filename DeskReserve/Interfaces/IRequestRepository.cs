using DeskReserve.Data.DBContext.Entity;

namespace RequestReserve.Interfaces
{
	public interface IRequestRepository
	{
		Task<IEnumerable<Request>> GetAll();

		Task<Request> GetById(Guid id);

		Task<bool> Approve(Request desk);

		Task<bool> Create(Request floor);
	}
}