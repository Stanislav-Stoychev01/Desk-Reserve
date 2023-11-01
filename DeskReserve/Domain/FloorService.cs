using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Domain
{
    public class FloorService : IFloorService
    {
        private readonly ApplicationDbContext _context;

        public FloorService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Floor>> GetAllAsync()
        {
            return await _context.Floors.ToListAsync();
        }

        public async Task<Floor> GetOneAsync(Guid id)
        {
            return await _context.Floors.FindAsync(id);
        }

        public async Task<bool> UpdateOneAsync(Guid id, Floor floor)
        {
            if (id != floor.FloorId)
            {
                return false;
            }

            _context.Entry(floor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<Floor> CreateOneAsync(Floor floor)
        {
            _context.Floors.Add(floor);
            await _context.SaveChangesAsync();
            return floor;
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var floor = await _context.Floors.FindAsync(id);

            if (floor == null)
            {
                return false;
            }

            _context.Floors.Remove(floor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
