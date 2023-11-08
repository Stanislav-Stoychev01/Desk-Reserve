using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeskReserve.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SaveRefreshToken(Guid userId, string refreshToken)
        {
            var existingToken = await _context.RefreshToken
                .Where(rt => rt.UserId == userId)
                .FirstOrDefaultAsync();

            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
            }
            else
            {
                var newToken = new RefreshToken
                {
                    UserId = userId,
                    Token = refreshToken
                };
                _context.RefreshToken.Add(newToken);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveRefreshToken(Guid userId)
        {
            var tokenToRemove = await _context.RefreshToken
                .Where(rt => rt.UserId == userId)
                .FirstOrDefaultAsync();

            if (tokenToRemove != null)
            {
                _context.RefreshToken.Remove(tokenToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> VerifyUserToken(Guid userId, string tokenToVerify)
        {
            var storedToken = await _context.RefreshToken
                .Where(rt => rt.UserId == userId)
                .Select(rt => rt.Token)
                .FirstOrDefaultAsync();

            return storedToken == tokenToVerify;
        }
    }

}
