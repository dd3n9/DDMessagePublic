using Gateway.Api.Models;
using Gateway.Api.Models.DTOs;

namespace Gateway.Api.Interfaces.Services
{
    public interface IUserService
    {
        Task<Guid?> GetUserIdByExternalId(Guid externalId);
        Task<User?> GetUserByEmail(string email);
        Task AddUserAsync(RegisterDto user);
    }
}
