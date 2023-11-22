using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using RequestReserve.Interfaces;

namespace DeskReserve.Repository
{
	public class RequestRepository : IRequestRepository
	{
		private readonly ApplicationDbContext _context;

		public RequestRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Request>> GetAll()
		{
			return await _context.Requests.ToListAsync() ?? throw new EntityNotFoundException("Request not found");
		}

		public async Task<Request> GetById(Guid id)
		{
			return await _context.Requests.FindAsync(id) ?? throw new EntityNotFoundException("Request not found");
		}

		public async Task<bool> Uppdate(Request request)
		{
			var existingRequest = await _context.Desks.FindAsync(request.RequestId) ?? throw new EntityNotFoundException("Desk not found");

			_context.Entry(request).State = EntityState.Modified;

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> Create(Request request)
		{

			var deskId = await _context.Desks.FindAsync(request.DeskId) ?? throw new EntityNotFoundException("Desk not found");

			_context.Requests.AddAsync(request);

			return await _context.SaveChangesAsync() > 0;
		}
	}
}