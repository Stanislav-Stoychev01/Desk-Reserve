using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Interfaces
{
    public interface IUserRolesRepository
    {
        Task<Guid> GetRoleIdByRoleName(string roleName);

        Task<IEnumerable<UserRoleDto>> GetAllUsersRoles();

        Task<Role> GetRoleByUserId(Guid userId);

        Task<bool> AddUserRole(UserRole userRole);

        Task<bool> Update(UserRole userRole);

        Task<bool> Update(Role role);

        Task<bool> Delete(UserRole userRole);
    }
}
