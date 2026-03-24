using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaySimple.BookingService.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public int RoomId { get; set; }
        public string RoomType { get; set; } = "";
        public string HotelName { get; set; } = "";
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        [Column(TypeName = "decimal(10,2)")] public decimal TotalPrice { get; set; }
        [MaxLength(30)] public string Status { get; set; } = "Confirmed";
        public string BookingRef { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
