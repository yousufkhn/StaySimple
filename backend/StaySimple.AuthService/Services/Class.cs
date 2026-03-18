using StaySimple.AuthService.Models;

namespace StaySimple.AuthService.Services
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
    }
}
