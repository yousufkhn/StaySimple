using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaySimple.HotelService.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        [ForeignKey("HotelId")] public Hotel Hotel { get; set; } = null!;
        [Required, MaxLength(50)] public string RoomType { get; set; } = "";
        [Column(TypeName = "decimal(10,2)")] public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
