using Microsoft.EntityFrameworkCore;
using StaySimple.HotelService.Data;
using StaySimple.HotelService.DTOs;
using StaySimple.HotelService.Models;
using StaySimple.HotelService.Services.Interfaces;

namespace StaySimple.HotelService.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly HotelDbContext _db;
        public HotelService(HotelDbContext db)
        {
            _db = db;
        }

        // this is a internal function to map from hotel -> to -> hotelDto
        private static HotelDto Map(Hotel h)
        {
            return new(h.Id, h.Name, h.City, h.Address, h.Description, h.Rating, h.ImageUrl);
        }
        public async Task<HotelDto> CreateAsync(CreateHotelDto dto)
        {
            var h = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Rating = dto.Rating
            };

            _db.Hotels.Add(h);
            await _db.SaveChangesAsync();
            return Map(h);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _db.Hotels.FindAsync(id);
            if (result == null) return false;

            _db.Hotels.Remove(result);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<HotelDto>> GetAllAsync()
        {
            var result = (await _db.Hotels.ToListAsync()).Select(Map);
            return result;
        }

        public async Task<HotelDto?> GetByIdAsync(int id)
        {
            var result = await _db.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (result == null) return null;
            else return Map(result);
        }

        public async Task<IEnumerable<HotelDto>> SearchAsync(string city)
        {
            var result = await _db.Hotels
                            .Where(h => h.City.ToLower().Contains(city.ToLower()))
                            .ToListAsync();
            return result.Select(Map);
        }

        public async Task<HotelDto?> UpdateAsync(int id, CreateHotelDto dto)
        {
            var hotel =  await _db.Hotels.FindAsync(id);
            if (hotel == null) return null;

            hotel.Name = dto.Name;
            hotel.City = dto.City;
            hotel.Address = dto.Address;
            hotel.Description = dto.Description;
            hotel.Rating = dto.Rating;
            hotel.ImageUrl = dto.ImageUrl;

            await _db.SaveChangesAsync();

            return Map(hotel);


        }
    }
}
