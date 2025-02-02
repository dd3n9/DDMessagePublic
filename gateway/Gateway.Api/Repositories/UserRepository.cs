using Gateway.Api.Data;
using Gateway.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty");

            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserExternalId(string userEmail, Guid newExternalId)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("Email cannot be null or empty.", nameof(userEmail));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                throw new InvalidOperationException($"User with email '{userEmail}' not found.");

            user.ExternalUserId = newExternalId;
            await _context.SaveChangesAsync();
        }
    }
}
