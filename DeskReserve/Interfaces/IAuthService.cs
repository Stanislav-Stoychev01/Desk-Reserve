using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Security.Claims;

namespace DeskReserve.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(LoginModel loginModel);
        Task<string> CreateToken();
        Task<string> CreateRefreshToken();
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(RegisterModel registerModel);
        User HashUserPassword(RegisterModel registerModel);
        IEnumerable<Claim> GetAllClaimsFromToken(string token);
    }
}
