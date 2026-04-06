namespace StaySimple.Notifications.Services
{
    /// <summary>
    /// Mock email service for development/testing.
    /// Logs emails to console/file instead of sending via SMTP.
    /// </summary>
    public class MockEmailService : IEmailService
    {
        private readonly ILogger<MockEmailService> _logger;

        public MockEmailService(ILogger<MockEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendWelcomeEmailAsync(string email, string name)
        {
            _logger.LogInformation(
                "[MOCK EMAIL] Welcome email queued for sending\n" +
                "  To: {Email}\n" +
                "  Name: {Name}\n" +
                "  Subject: Welcome to StaySimple - Your Hotel Booking Platform",
                email, name);
            return Task.CompletedTask;
        }

        public Task SendBookingConfirmationEmailAsync(
            string email,
            string userName,
            string hotelName,
            string roomType,
            DateTime checkInDate,
            DateTime checkOutDate,
            decimal totalPrice,
            string bookingRef)
        {
            var nights = (checkOutDate - checkInDate).Days;
            _logger.LogInformation(
                "[MOCK EMAIL] Booking confirmation queued for sending\n" +
                "  To: {Email}\n" +
                "  User: {UserName}\n" +
                "  Hotel: {HotelName}\n" +
                "  Room Type: {RoomType}\n" +
                "  Check-in: {CheckIn:yyyy-MM-dd}\n" +
                "  Check-out: {CheckOut:yyyy-MM-dd}\n" +
                "  Nights: {Nights}\n" +
                "  Total: ₹{TotalPrice}\n" +
                "  Booking Ref: {BookingRef}\n" +
                "  Subject: Booking Confirmation - {BookingRef}",
                email, userName, hotelName, roomType, checkInDate, checkOutDate, nights, totalPrice, bookingRef);
            return Task.CompletedTask;
        }

        public Task SendBookingCancellationEmailAsync(
            string email,
            string userName,
            string hotelName,
            string bookingRef)
        {
            _logger.LogInformation(
                "[MOCK EMAIL] Booking cancellation queued for sending\n" +
                "  To: {Email}\n" +
                "  User: {UserName}\n" +
                "  Hotel: {HotelName}\n" +
                "  Booking Ref: {BookingRef}\n" +
                "  Subject: Booking Cancelled - {BookingRef}",
                email, userName, hotelName, bookingRef);
            return Task.CompletedTask;
        }
    }
}
