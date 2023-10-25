using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
    public class DogService
    {
        private readonly ApplicationDbContext _dbContext;
        
        public DogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<Dog> GetAllDogs()
        {
            return _dbContext.Dogs.ToList();
        }
    }
}