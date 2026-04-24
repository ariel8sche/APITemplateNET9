using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Requests;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APITemplate.Services.Services
{
    public class UserService : IUserService
    {
        private readonly AuthContext _db;

        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, AuthContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<UserDto>> GetById(int userId)
        {
            _logger.LogInformation("GetById: fetching user UserId={UserId}", userId);

            var users = await _db.Users
                .Where(e => e.UserId == userId)
                .Select(e => new UserDto
                {
                    UserId = e.UserId,
                    Username = e.Username,
                    Email = e.Email,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            if (users is null || users.Count == 0)
            {
                _logger.LogWarning("GetById: no user found for UserId={UserId}", userId);
            }
            else
            {
                _logger.LogDebug("GetById: retrieved {Count} user(s) for UserId={UserId}", users.Count, userId);
            }

            return users;
        }

        public async Task<List<UserDto>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all users");

            var items = await _db.Users
                .OrderBy(u => u.UserId)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            _logger.LogDebug("GetAll: retrieved {Count} users", items?.Count ?? 0);

            return items;
        }

        public async Task<int> Create(CreateUserRequest request)
        {
            _logger.LogInformation("Create: creating user Username={Username}", request.Username);

            var passwordHash = ComputeSha256Hash(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Create: created user UserId={UserId} Username={Username}", user.UserId, user.Username);

            return user.UserId;
        }

        public async Task<bool> Update(int userId, UpdateUserRequest request)
        {
            _logger.LogInformation("Update: updating user UserId={UserId}", userId);

            var user = await _db.Users.FirstOrDefaultAsync(e => e.UserId == userId);

            if (user is null)
            {
                _logger.LogWarning("Update: user not found UserId={UserId}", userId);
                return false;
            }

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            user.IsActive = request.IsActive;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Update: updated user UserId={UserId} (IsActive={IsActive})", userId, request.IsActive);

            return true;
        }

        public async Task<bool> Delete(int userId)
        {
            _logger.LogInformation("Delete: deactivating user UserId={UserId}", userId);

            var user = await _db.Users.FirstOrDefaultAsync(e => e.UserId == userId);

            if (user is null)
            {
                _logger.LogWarning("Delete: user not found UserId={UserId}", userId);
                return false;
            }

            user.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Delete: deactivated user UserId={UserId}", userId);

            return true;
        }

        // Helpers

        private static string ComputeSha256Hash(string password)
        {
            if (password is null) throw new ArgumentNullException(nameof(password));

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}