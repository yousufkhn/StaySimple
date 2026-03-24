using Microsoft.EntityFrameworkCore;
using StaySimple.HotelService.Models;

namespace StaySimple.HotelService.Data
{
    public class HotelDbContext(DbContextOptions<HotelDbContext> options) : DbContext(options)
    {
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<Room> Rooms => Set<Room>();

        protected override void OnModelCreating(ModelBuilder m)
        {
            m.Entity<Room>().Property(r => r.PricePerNight).HasColumnType("decimal(10,2)");

            m.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Name = "The Grand Palace", City = "Mumbai", Address = "123 Marine Drive, Mumbai", Description = "A luxurious 5-star hotel overlooking the Arabian Sea with world-class amenities.", Rating = 4.8, ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=800" },
                new Hotel { Id = 2, Name = "Royal Heritage Inn", City = "Jaipur", Address = "45 Palace Road, Jaipur", Description = "Experience royal Rajasthani hospitality in this beautifully restored heritage property.", Rating = 4.5, ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=800" },
                new Hotel { Id = 3, Name = "Mountain View Resort", City = "Manali", Address = "78 Mall Road, Manali", Description = "A serene mountain retreat with stunning Himalayan views and cozy accommodations.", Rating = 4.3, ImageUrl = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=800" },
                new Hotel { Id = 4, Name = "Sunset Beach Hotel", City = "Goa", Address = "22 Baga Beach Road, Goa", Description = "A beachfront paradise with private beach access and vibrant nightlife nearby.", Rating = 4.6, ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=800" },
                new Hotel { Id = 5, Name = "City Central Hotel", City = "Delhi", Address = "99 Connaught Place, Delhi", Description = "Modern business hotel in the heart of New Delhi, perfect for work and leisure.", Rating = 4.2, ImageUrl = "https://images.unsplash.com/photo-1564501049412-61c2a3083791?w=800" },
                new Hotel { Id = 6, Name = "Lakeside Retreat", City = "Udaipur", Address = "12 Lake Pichola Road, Udaipur", Description = "A romantic lakeside hotel with stunning views of Lake Pichola and City Palace.", Rating = 4.7, ImageUrl = "https://images.unsplash.com/photo-1582719508461-905c673771fd?w=800" }
            );

            m.Entity<Room>().HasData(
                new Room { Id = 1, HotelId = 1, RoomType = "Single", PricePerNight = 3500, Capacity = 1, IsAvailable = true },
                new Room { Id = 2, HotelId = 1, RoomType = "Double", PricePerNight = 5500, Capacity = 2, IsAvailable = true },
                new Room { Id = 3, HotelId = 1, RoomType = "Suite", PricePerNight = 12000, Capacity = 4, IsAvailable = true },
                new Room { Id = 4, HotelId = 2, RoomType = "Single", PricePerNight = 2500, Capacity = 1, IsAvailable = true },
                new Room { Id = 5, HotelId = 2, RoomType = "Double", PricePerNight = 4000, Capacity = 2, IsAvailable = true },
                new Room { Id = 6, HotelId = 2, RoomType = "Deluxe", PricePerNight = 7500, Capacity = 3, IsAvailable = true },
                new Room { Id = 7, HotelId = 3, RoomType = "Single", PricePerNight = 2000, Capacity = 1, IsAvailable = true },
                new Room { Id = 8, HotelId = 3, RoomType = "Double", PricePerNight = 3500, Capacity = 2, IsAvailable = true },
                new Room { Id = 9, HotelId = 3, RoomType = "Suite", PricePerNight = 6000, Capacity = 4, IsAvailable = true },
                new Room { Id = 10, HotelId = 4, RoomType = "Single", PricePerNight = 3000, Capacity = 1, IsAvailable = true },
                new Room { Id = 11, HotelId = 4, RoomType = "Double", PricePerNight = 5000, Capacity = 2, IsAvailable = true },
                new Room { Id = 12, HotelId = 4, RoomType = "Deluxe", PricePerNight = 8500, Capacity = 3, IsAvailable = true },
                new Room { Id = 13, HotelId = 5, RoomType = "Single", PricePerNight = 2200, Capacity = 1, IsAvailable = true },
                new Room { Id = 14, HotelId = 5, RoomType = "Double", PricePerNight = 3800, Capacity = 2, IsAvailable = true },
                new Room { Id = 15, HotelId = 6, RoomType = "Double", PricePerNight = 4500, Capacity = 2, IsAvailable = true },
                new Room { Id = 16, HotelId = 6, RoomType = "Suite", PricePerNight = 9500, Capacity = 4, IsAvailable = true }
            );
        }
    }
}
