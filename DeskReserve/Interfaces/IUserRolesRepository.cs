using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Interfaces
{
    public interface IUserRolesRepository
    {
        Task<Guid> GetRoleIdByRoleName(string roleName);

        Task<IEnumerable<UserRoleDto>> GetAllUsersRoles();

        Task<UserRole> GetUserRole(Guid userId);

        Task<Role> GetRoleByUserId(Guid userId);

        Task<bool> AddUserRole(UserRole userRole);

        Task<bool> Update(UserRole userRole);

        Task<bool> Delete(UserRole userRole);
    }
}
