using Gateway.Api.AsyncDataServices;
using Gateway.Api.Data;
using Gateway.Api.Models;
using Gateway.Api.Models.DTOs;
using Gateway.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Interfaces.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(AppDbContext context,
            IUserRepository userRepository,
            IMessageBusClient messageBusClient,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _userRepository = userRepository;
            _messageBusClient = messageBusClient;
            _passwordHasher = passwordHasher;
        }

        public async Task AddUserAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.UserExistsAsync(registerDto.Email);
            if (existingUser)
                throw new InvalidOperationException("User with this email already exists.");


            var hashedPassword = _passwordHasher.Hash(registerDto.Password);

            await _userRepository.AddUserAsync(new User
            {
                Email = registerDto.Email,
                HashedPassword = hashedPassword
            });

            var userPublished = new UserPublishedDto(registerDto.UserName,
                registerDto.Email,
                hashedPassword,
                "User_Published");

            _messageBusClient.PublishNewUser(userPublished);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync();
        }

        public async Task<Guid?> GetUserIdByExternalId(Guid externalId)
        {
            return await _context.Users
                .Where(u => u.ExternalUserId == externalId)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
        }
    }
}
