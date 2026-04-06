using BCrypt.Net;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using StaySimple.AuthService.Data;
using StaySimple.AuthService.DTOs;
using StaySimple.AuthService.Models;

namespace StaySimple.AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly IJwtHelper _jwt;
        private readonly IPublishEndpoint _bus;

        public AuthService(AuthDbContext db, IJwtHelper jwt, IPublishEndpoint bus)
        {
            _db = db;
            _jwt = jwt;
            _bus = bus;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var email = (dto.Email ?? string.Empty).Trim().ToLowerInvariant();
            var password = dto.Password ?? string.Empty;

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return new AuthResponseDto
            {
                Token = _jwt.GenerateToken(user),
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var name = (dto.Name ?? string.Empty).Trim();
            var email = (dto.Email ?? string.Empty).Trim().ToLowerInvariant();
            var password = dto.Password ?? string.Empty;

            if (await _db.Users.AnyAsync(u => u.Email.ToLower() == email))
            {
                throw new Exception("Email already Exists");
            }

            var user = new User
            {
                Name = name,
                Email = email,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // ── RabbitMQ: Notification service subscribes to this
            await _bus.Publish(new UserRegisteredEvent
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            });

            return new AuthResponseDto
            {
                Token = _jwt.GenerateToken(user),
                Name = user.Name,
                Role = user.Role,
                Email = user.Email
            };
        }
    }
}
