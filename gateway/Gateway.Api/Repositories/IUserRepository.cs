using Gateway.Api.Models;

namespace Gateway.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<IReadOnlyList<User>> GetAllUsersAsync();
        Task<bool> UserExistsAsync(string email);
        Task<User> AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task UpdateUserExternalId(string userEmail, Guid newExternalId);
        Task DeleteUserAsync(User user);
    }
}
