using Microsoft.EntityFrameworkCore;
using StaySimple.BookingService.Data;
using StaySimple.BookingService.DTOs;
using StaySimple.BookingService.Services.Interfaces;
using System.Security.Claims;

namespace StaySimple.BookingService.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly BookingDbContext _db;
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        

        public BookingService(BookingDbContext db,IHttpClientFactory http,IConfiguration config)
        {
            _db = db;
            _http = http;
            _config = config;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync()
        {
            var bookings = await _db.Bookings
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(Map);
        }

        public async Task<IEnumerable<BookingDto>> GetMineAsync(int userId)
        {
            var bookings = await _db.Bookings
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(Map);
        }

        public async Task<BookingDto?> CreateAsync(CreateBookingDto dto, ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userName = user.FindFirstValue(ClaimTypes.Name) ?? "";
            var userEmail = user.FindFirstValue(ClaimTypes.Email) ?? "";

            if (dto.CheckOutDate <= dto.CheckInDate)
                return null;

            // this checks in the local db for room availability
            var isAvailableLocal = await IsRoomAvailableLocally(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            if (!isAvailableLocal) return null;

            var isAvailableExternal = await IsRoomAvailableFromHotel(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            if (!isAvailableExternal) return null;

            var nights = (dto.CheckOutDate - dto.CheckInDate).Days;
            var total = nights * dto.PricePerNight;
            var bookingRef = $"STY-{DateTime.UtcNow.Year}-{Random.Shared.Next(100000, 999999)}";

            var booking = new Models.Booking
            {
                UserId = userId,
                UserName = userName,
                UserEmail = userEmail,
                RoomId = dto.RoomId,
                RoomType = dto.RoomType,
                HotelName = dto.HotelName,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                TotalPrice = total,
                Status = "Confirmed",
                BookingRef = bookingRef,
                CreatedAt = DateTime.UtcNow
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return Map(booking);
        }

        public async Task<bool> CancelAsync(int id, int userId, string? role)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null) return false;
            if (booking.Status == "Cancelled") return false;
            if (role != "Admin" && booking.UserId != userId) return false;

            booking.Status = "Cancelled";
            await _db.SaveChangesAsync();

            return true;
        }

        private static BookingDto Map(Models.Booking b) => new(
            b.Id, b.UserId, b.UserName, b.RoomId, b.RoomType, b.HotelName,
            b.CheckInDate, b.CheckOutDate, b.TotalPrice, b.Status, b.BookingRef, b.CreatedAt);

        private async Task<bool> IsRoomAvailableLocally(int roomId, DateTime checkIn, DateTime checkOut)
        {
            // this returns true is room is available
            return !await _db.Bookings.AnyAsync(b =>
                                        b.RoomId == roomId &&
                                        b.Status != "Cancelled" &&
                                        b.CheckInDate < checkOut &&
                                        b.CheckOutDate > checkIn);
        }

        private async Task<bool> IsRoomAvailableFromHotel(int roomId,DateTime checkIn,DateTime checkOut)
        {
            var client = _http.CreateClient();

            var baseUrl = _config["Services:HotelService"];

            // why checkId:O -> prevents timezone bugs
            var url = $"{baseUrl}/api/rooms/{roomId}/availability" + $"?checkIn={checkIn:O}&checkOut={checkOut:O}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<AvailabilityResponse>();

            return result?.IsAvailable ?? false;


        }
    }

    public class AvailabilityResponse
    {
        public bool IsAvailable { get; set; }
    }
}
