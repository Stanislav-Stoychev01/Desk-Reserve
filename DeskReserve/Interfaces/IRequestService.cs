using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace RequestReserve.Interfaces
{
	public interface IRequestService
	{
		Task<IEnumerable<Request>> GetAllAsync();

		Task<RequestDto> GetAsync(Guid id);

		Task<bool> ApproveAsync(Guid id, RequestDto deskDto);

		Task<bool> CreateAsync(RequestDto desk);

	}
}