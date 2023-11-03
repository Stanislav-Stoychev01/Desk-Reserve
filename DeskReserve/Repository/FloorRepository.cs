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
            var floorEntity = await _context.Floor.FindAsync(floor.FloorId);

            if (ReferenceEquals(floorEntity, null))
            {
                return false;
            }
            else
            {
                _context.Entry(floorEntity).State = EntityState.Detached;
            }

            _context.Entry(floor).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var floor = await _context.Floor.FindAsync(id);

            if (ReferenceEquals(floor, null))
            {
                return false;
            }

            _context.Floor.Remove(floor);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
