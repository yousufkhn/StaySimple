using StaySimple.BookingService.DTOs;
using System.Security.Claims;

namespace StaySimple.BookingService.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<IEnumerable<BookingDto>> GetMineAsync(int userId);
        Task<BookingDto?> CreateAsync(CreateBookingDto dto, ClaimsPrincipal user);
        Task<bool> CancelAsync(int id, int userId, string? role);

    }
}
