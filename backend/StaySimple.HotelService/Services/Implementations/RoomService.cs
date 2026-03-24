using Microsoft.EntityFrameworkCore;
using StaySimple.HotelService.Data;
using StaySimple.HotelService.DTOs;
using StaySimple.HotelService.Models;
using StaySimple.HotelService.Services.Interfaces;

namespace StaySimple.HotelService.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly HotelDbContext _db;

        public RoomService(HotelDbContext db)
        {
            _db = db;
        }

        private static RoomDto Map(Room r)
        {
            return new(r.Id, r.HotelId, r.Hotel?.Name ?? "", r.RoomType,
            r.PricePerNight, r.Capacity, r.IsAvailable);
        }
        public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
        {
            var room = new Room
            {
                HotelId = dto.HotelId,
                RoomType = dto.RoomType,
                PricePerNight = dto.PricePerNight,
                Capacity = dto.Capacity,
                IsAvailable = dto.IsAvailable
            };
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
            return Map(room);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var r = await _db.Rooms.FindAsync(id);
            if (r == null) return false;

            _db.Rooms.Remove(r);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<RoomDto>> GetByHotelAsync(int hotelId)
        {
            var result = await _db.Rooms.Include(r => r.Hotel)
                                        .Where(r => r.HotelId == hotelId)
                                        .ToListAsync();

            return result.Select(Map);
        }

        public async Task<RoomDto?> GetByIdAsync(int id)
        {
            var room = await _db.Rooms
                           .Include(r => r.Hotel)
                           .FirstOrDefaultAsync(r => r.Id == id);
            return room == null ? null : Map(room);
        }

        public async Task<RoomDto?> UpdateAsync(int id, CreateRoomDto dto)
        {
            var r = await _db.Rooms.FindAsync(id);
            if (r == null) return null;

            r.RoomType = dto.RoomType;
            r.PricePerNight = dto.PricePerNight;
            r.Capacity = dto.Capacity;
            r.IsAvailable = dto.IsAvailable;

            await _db.SaveChangesAsync();

            return Map(r);
        }

        public async Task<RoomAvailabilityResult> CheckAvailabilityAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            // Date-window conflicts are validated in BookingService using bookings data.
            // HotelService only exposes whether the room exists and is currently sellable.
            var room = await _db.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
            {
                return new RoomAvailabilityResult(false, false);
            }

            return new RoomAvailabilityResult(true, room.IsAvailable);
        }
    }
}
