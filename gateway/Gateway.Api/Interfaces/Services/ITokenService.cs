using Gateway.Api.Models;

namespace Gateway.Api.Interfaces.Services
{
    public interface ITokenService
    {
        Task<RefreshToken?> GetRefreshTokenByUserId(Guid userId);
        Task<RefreshToken?> FindRefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(Guid userId);
    }
}
