using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(LoginModel loginModel);
        Task<string> CreateToken();
        Task<string> CreateRefreshToken();
        Task<TokenRequest> VerifyRefreshToken(TokenRequest request);
        Task<bool> CreateUser(User user);
        User HashUserPassword(RegisterModel registerModel);
    }
}
