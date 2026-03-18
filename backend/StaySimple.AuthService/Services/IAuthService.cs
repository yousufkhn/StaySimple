using StaySimple.AuthService.DTOs;

namespace StaySimple.AuthService.Services
{
    public interface IAuthService
    {
        // The Task here helps it make these methods async
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}

