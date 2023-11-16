using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Security.Claims;

namespace DeskReserve.Interfaces
{
    public interface IAuthService
    {
        Task<bool> CreateUser(RegisterModel registerModel, string passwordHash, string salt);
        Task<bool> ValidateUser(LoginModel loginModel);
        Task<string> CreateToken();
        Task<string> CreateRefreshToken();
        Task<bool> ChangeUserPassword(string userEmail, ChangePasswordModel changePasswordModel);
        Task<User> GetUser(Guid userId);
        Task<Role> GetRole(Guid userId);
        Task<Guid> GetRoleId(string roleName);
        Task<UserRole> GetUserRole(Guid userId);
        Task<bool> UpdateUserRole(UserRole newRole);
    }
}
