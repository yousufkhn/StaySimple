using Microsoft.EntityFrameworkCore;
using StaySimple.BookingService.Models;

namespace StaySimple.BookingService.Data
{
    public class BookingDbContext(DbContextOptions<BookingDbContext> options ) : DbContext(options)
    {
        public DbSet<Booking> Bookings => Set<Booking>();
    }
}
