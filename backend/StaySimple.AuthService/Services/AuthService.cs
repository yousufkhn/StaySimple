using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using StaySimple.AuthService.Data;
using StaySimple.AuthService.DTOs;
using StaySimple.AuthService.Models;

namespace StaySimple.AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly IJwtHelper _jwt;

        public AuthService(AuthDbContext db,IJwtHelper jwt)
        {
            _db = db;
            _jwt = jwt;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
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
            if(await _db.Users.AnyAsync(u=>u.Email == dto.Email))
            {
                throw new Exception("Email already Exists");
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

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
