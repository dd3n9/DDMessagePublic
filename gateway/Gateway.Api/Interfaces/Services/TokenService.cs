using Gateway.Api.Data;
using Gateway.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Interfaces.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _appDbContext;

        public TokenService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<RefreshToken?> FindRefreshTokenAsync(string token)
        {
            return await _appDbContext.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task<RefreshToken?> GetRefreshTokenByUserId(Guid userId)
        {
            return await _appDbContext.RefreshTokens
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> RevokeTokenAsync(Guid userId)
        {
            await _appDbContext.RefreshTokens
                .Where(x => x.UserId == userId)
                .ExecuteDeleteAsync();

            return true;
        }
    }
}
