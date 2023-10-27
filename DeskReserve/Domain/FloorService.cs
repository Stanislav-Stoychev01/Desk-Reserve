using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;

namespace DeskReserve.Domain
{
    public class FloorService
    {
        private readonly ApplicationDbContext _dbContext;

        public FloorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<Floor> GetAllFloors()
        {
            return _dbContext.Floors.ToList();
        }
    }
}
