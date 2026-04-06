using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaySimple.Notifications.Services
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string name);
        Task SendBookingConfirmationEmailAsync(
            string email,
            string userName,
            string hotelName,
            string roomType,
            DateTime checkInDate,
            DateTime checkOutDate,
            decimal totalPrice,
            string bookingRef);
        Task SendBookingCancellationEmailAsync(
            string email,
            string userName,
            string hotelName,
            string bookingRef);
    }
}
