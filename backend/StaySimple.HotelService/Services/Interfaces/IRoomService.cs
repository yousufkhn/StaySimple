using StaySimple.HotelService.DTOs;

namespace StaySimple.HotelService.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetByHotelAsync(int hotelId);
        Task<RoomDto?> GetByIdAsync(int id);
        Task<RoomDto> CreateAsync(CreateRoomDto dto);
        Task<RoomDto?> UpdateAsync(int id, CreateRoomDto dto);
        Task<bool> DeleteAsync(int id);
        Task<RoomAvailabilityResult> CheckAvailabilityAsync(int roomId, DateTime checkIn, DateTime checkOut);
    }
}
