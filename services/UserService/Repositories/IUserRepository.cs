using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<IReadOnlyList<User>> GetAllUsersAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ExternalUserExists(string email);
        Task<User> AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
