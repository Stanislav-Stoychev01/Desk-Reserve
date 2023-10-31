using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Domain
{
    public class FloorService
    {
        private readonly ApplicationDbContext _context;

        public FloorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Floor>> GetFloors()
        {
            return await _context.Floors.ToListAsync();
        }

        public async Task<Floor> GetFloor(Guid id)
        {
            return await _context.Floors.FindAsync(id);
        }

        public async Task<bool> UpdateFloor(Guid id, Floor floor)
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

        public async Task<Floor> CreateFloor(Floor floor)
        {
            _context.Floors.Add(floor);
            await _context.SaveChangesAsync();
            return floor;
        }

        public async Task<bool> DeleteFloor(Guid id)
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
