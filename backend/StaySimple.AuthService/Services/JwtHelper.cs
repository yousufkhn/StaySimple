using Microsoft.IdentityModel.Tokens;
using StaySimple.AuthService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StaySimple.AuthService.Services
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _config;
        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var name = string.IsNullOrWhiteSpace(user.Name) ? "User" : user.Name;
            var email = string.IsNullOrWhiteSpace(user.Email) ? string.Empty : user.Email;
            var role = string.IsNullOrWhiteSpace(user.Role) ? "User" : user.Role;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,name),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Role,role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
            
            
        }
    }
}
