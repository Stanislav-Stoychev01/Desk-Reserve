using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Repository
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRolesRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Guid> GetRoleIdByRoleName(string roleName)
        {
            Role role = await _context.Roles
                .Where(r => r.RoleName == roleName)
                .FirstOrDefaultAsync() 
                ?? throw new EntityNotFoundException();

            return role.RoleId;
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllUsersRoles()
        {
            var usersWithRoles = _context.UserRoles
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .Select(ur => new UserRoleDto
                {
                    UserId = ur.UserId,
                    UserName = ur.User.UserName,
                    RoleName = ur.Role.RoleName
                })
                .ToList();

            return usersWithRoles;
        }

        public async Task<UserRole> GetUserRole(Guid userId)
        {
            var userRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .FirstOrDefaultAsync() 
                ?? throw new EntityNotFoundException();

            return userRole;
        }

        public async Task<Role> GetRoleByUserId(Guid userId)
        {
            Role role = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<bool> AddUserRole(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(UserRole userRole)
        {
            _context.Entry(userRole).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(UserRole userRole)
        {
            _context.UserRoles.Remove(userRole);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
