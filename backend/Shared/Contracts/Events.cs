using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class BookingConfirmedEvent
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string HotelName { get; set; } = "";
        public string RoomType { get; set; } = "";
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingRef { get; set; } = "";
    }

    public class BookingCancelledEvent
    {
        public int BookingId { get; set; }
        public string UserEmail { get; set; } = "";
        public string UserName { get; set; } = "";
        public string HotelName { get; set; } = "";
        public string BookingRef { get; set; } = "";
    }

    public class UserRegisteredEvent
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }

}
