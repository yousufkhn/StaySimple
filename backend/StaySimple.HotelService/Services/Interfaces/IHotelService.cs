using StaySimple.HotelService.DTOs;

namespace StaySimple.HotelService.Services.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelDto>> GetAllAsync();
        Task<HotelDto?> GetByIdAsync(int id);
        Task<IEnumerable<HotelDto>> SearchAsync(string city);
        Task<HotelDto> CreateAsync(CreateHotelDto dto);
        Task<HotelDto?> UpdateAsync(int id, CreateHotelDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
