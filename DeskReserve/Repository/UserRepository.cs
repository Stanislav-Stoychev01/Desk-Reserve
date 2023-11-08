using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.User.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Role> GetRoleById(Guid userId)
        {
            UserRoles userRoles = await _context.UserRoles.FindAsync(userId);
            Role role = await _context.Role.FindAsync(userRoles.RoleId);
            return role;
        }

        public async Task<bool> Add(User user)
        {
            await _context.User.AddAsync(user);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(User user)
        {
            var userEntity = await _context.User.FindAsync(user.UserId);

            if (ReferenceEquals(userEntity, null))
            {
                return false;
            }
            else
            {
                _context.Entry(userEntity).State = EntityState.Detached;
            }

            _context.Entry(user).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await _context.User.FindAsync(id);

            if (ReferenceEquals(user, null))
            {
                return false;
            }

            _context.User.Remove(user);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
