namespace DeskReserve.Interfaces
{
    public interface ITokenRepository
    {
        Task SaveRefreshToken(Guid userId, string refreshToken);
        Task RemoveRefreshToken(Guid userId);
        Task<bool> VerifyUserToken(Guid userId, string tokenToVerify);
    }
}
