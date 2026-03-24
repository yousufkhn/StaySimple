using System.ComponentModel.DataAnnotations;

namespace StaySimple.HotelService.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        [Required, MaxLength(200)] public string Name { get; set; } = "";
        [Required, MaxLength(100)] public string City { get; set; } = "";
        [MaxLength(300)] public string Address { get; set; } = "";
        [MaxLength(1000)] public string Description { get; set; } = "";
        public double Rating { get; set; }
        [MaxLength(500)] public string ImageUrl { get; set; } = "";
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
