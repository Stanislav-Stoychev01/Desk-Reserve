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
        Task<Role> GetRole(Guid userId);
        Task<User> GetUser(Guid userId);
        Task<bool> UpdateRole(Role role);
    }
}
