using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Repository
{
    public class FloorRepository : IFloorRepository
    { 
        private readonly ApplicationDbContext _context;

        public FloorRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Floor>> GetAll()
        {
            return await _context.Floor.ToListAsync();
        }

        public async Task<Floor> GetById(Guid id)
        {
            return await _context.Floor.FindAsync(id);
        }

        public async Task<bool> Add(Floor floor)
        {
            await _context.Floor.AddAsync(floor);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Floor floor)
        {
            _context.Entry(floor).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Floor floor)
        {
            _context.Floor.Remove(floor);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
