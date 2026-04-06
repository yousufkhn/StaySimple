using System.Net;
using System.Net.Mail;
using System.Text;

namespace StaySimple.Notifications.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendWelcomeEmailAsync(string email, string name)
        {
            try
            {
                var subject = "Welcome to StaySimple - Your Hotel Booking Platform";
                var body = GetWelcomeEmailBody(name);
                
                await SendEmailAsync(email, subject, body);
                _logger.LogInformation("[EMAIL] Welcome email sent to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EMAIL] Failed to send welcome email to {Email}", email);
                throw;
            }
        }

        public async Task SendBookingConfirmationEmailAsync(
            string email,
            string userName,
            string hotelName,
            string roomType,
            DateTime checkInDate,
            DateTime checkOutDate,
            decimal totalPrice,
            string bookingRef)
        {
            try
            {
                var subject = $"Booking Confirmation - {bookingRef}";
                var body = GetBookingConfirmationEmailBody(
                    userName, hotelName, roomType, checkInDate, checkOutDate, totalPrice, bookingRef);
                
                await SendEmailAsync(email, subject, body);
                _logger.LogInformation("[EMAIL] Booking confirmation sent to {Email} - Ref: {Ref}", email, bookingRef);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EMAIL] Failed to send booking confirmation to {Email}", email);
                throw;
            }
        }

        public async Task SendBookingCancellationEmailAsync(
            string email,
            string userName,
            string hotelName,
            string bookingRef)
        {
            try
            {
                var subject = $"Booking Cancelled - {bookingRef}";
                var body = GetBookingCancellationEmailBody(userName, hotelName, bookingRef);
                
                await SendEmailAsync(email, subject, body);
                _logger.LogInformation("[EMAIL] Cancellation email sent to {Email} - Ref: {Ref}", email, bookingRef);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EMAIL] Failed to send cancellation email to {Email}", email);
                throw;
            }
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpServer = _configuration["Email:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var fromEmail = _configuration["Email:FromEmail"];
            var fromName = _configuration["Email:FromName"] ?? "StaySimple";
            var username = _configuration["Email:Username"];
            var password = _configuration["Email:Password"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(username, password);

                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail!, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(new MailAddress(to));

                await client.SendMailAsync(message);
            }
        }

        private string GetWelcomeEmailBody(string name)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #4a70ff 0%, #7c5cde 100%); color: white; padding: 20px; border-radius: 8px 8px 0 0; text-align: center; }}
        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #e0e0e0; border-radius: 0 0 8px 8px; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #999; }}
        .cta-button {{ display: inline-block; background: linear-gradient(135deg, #4a70ff 0%, #7c5cde 100%); color: white; padding: 12px 30px; border-radius: 5px; text-decoration: none; margin-top: 10px; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>Welcome to StaySimple!</h2>
        </div>
        <div class=""content"">
            <p>Hello <strong>{name}</strong>,</p>
            <p>Thank you for joining StaySimple, your new favorite hotel booking platform.</p>
            <p>With StaySimple, you can:</p>
            <ul>
                <li>Browse thousands of hotels across India</li>
                <li>Compare rooms and find the best deals</li>
                <li>Book in just a few clicks</li>
                <li>Track your reservations in real-time</li>
                <li>Enjoy 24/7 travel support</li>
            </ul>
            <p>Ready to explore? Start browsing hotels and plan your next adventure!</p>
            <a href=""https://staysimple.com/hotels"" class=""cta-button"">Explore Hotels</a>
            <p style=""margin-top: 20px; color: #999; font-size: 14px;"">
                If you have any questions, please don't hesitate to reach out to our support team.
            </p>
        </div>
        <div class=""footer"">
            <p>&copy; 2024 StaySimple. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GetBookingConfirmationEmailBody(
            string userName,
            string hotelName,
            string roomType,
            DateTime checkInDate,
            DateTime checkOutDate,
            decimal totalPrice,
            string bookingRef)
        {
            var nights = (checkOutDate - checkInDate).Days;
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #4a70ff 0%, #7c5cde 100%); color: white; padding: 20px; border-radius: 8px 8px 0 0; text-align: center; }}
        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #e0e0e0; border-radius: 0 0 8px 8px; }}
        .booking-details {{ background: white; border-left: 4px solid #4a70ff; padding: 15px; margin: 15px 0; border-radius: 4px; }}
        .detail-row {{ display: flex; justify-content: space-between; margin: 10px 0; padding: 8px 0; border-bottom: 1px solid #eee; }}
        .detail-label {{ font-weight: 600; color: #555; }}
        .detail-value {{ color: #333; }}
        .price-total {{ background: #e8f0ff; padding: 10px; border-radius: 4px; font-weight: 700; color: #4a70ff; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #999; }}
        .ref-badge {{ display: inline-block; background: #d4af37; color: #1a1f2e; padding: 8px 12px; border-radius: 4px; font-weight: 700; margin: 10px 0; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>Booking Confirmed!</h2>
            <p>Your reservation is now confirmed</p>
        </div>
        <div class=""content"">
            <p>Hello <strong>{userName}</strong>,</p>
            <p>Your booking has been successfully confirmed. Below are your reservation details:</p>
            
            <div class=""ref-badge"">Booking Reference: {bookingRef}</div>
            
            <div class=""booking-details"">
                <div class=""detail-row"">
                    <span class=""detail-label"">Hotel:</span>
                    <span class=""detail-value"">{hotelName}</span>
                </div>
                <div class=""detail-row"">
                    <span class=""detail-label"">Room Type:</span>
                    <span class=""detail-value"">{roomType}</span>
                </div>
                <div class=""detail-row"">
                    <span class=""detail-label"">Check-in:</span>
                    <span class=""detail-value"">{checkInDate:MMMM dd, yyyy}</span>
                </div>
                <div class=""detail-row"">
                    <span class=""detail-label"">Check-out:</span>
                    <span class=""detail-value"">{checkOutDate:MMMM dd, yyyy}</span>
                </div>
                <div class=""detail-row"">
                    <span class=""detail-label"">Number of Nights:</span>
                    <span class=""detail-value"">{nights} night(s)</span>
                </div>
                <div class=""detail-row price-total"">
                    <span>Total Amount:</span>
                    <span>₹{totalPrice:N2}</span>
                </div>
            </div>
            
            <p><strong>What's Next?</strong></p>
            <ul>
                <li>Check your email and SMS for booking confirmation details</li>
                <li>Keep your booking reference handy for check-in</li>
                <li>The hotel will contact you 24 hours before your arrival (if needed)</li>
                <li>You can view, modify, or cancel your booking anytime through your account</li>
            </ul>
            
            <p style=""color: #666; margin-top: 20px;"">
                Questions? Visit our support center or contact us at support@staysimple.com
            </p>
        </div>
        <div class=""footer"">
            <p>&copy; 2024 StaySimple. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GetBookingCancellationEmailBody(string userName, string hotelName, string bookingRef)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #ff6b6b 0%, #ee5a6f 100%); color: white; padding: 20px; border-radius: 8px 8px 0 0; text-align: center; }}
        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #e0e0e0; border-radius: 0 0 8px 8px; }}
        .info-box {{ background: white; border-left: 4px solid #ff6b6b; padding: 15px; margin: 15px 0; border-radius: 4px; }}
        .detail-row {{ display: flex; justify-content: space-between; margin: 10px 0; padding: 8px 0; border-bottom: 1px solid #eee; }}
        .detail-label {{ font-weight: 600; color: #555; }}
        .detail-value {{ color: #333; }}
        .ref-badge {{ display: inline-block; background: #fff3cd; color: #856404; padding: 8px 12px; border-radius: 4px; font-weight: 700; margin: 10px 0; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #999; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>Booking Cancelled</h2>
        </div>
        <div class=""content"">
            <p>Hello <strong>{userName}</strong>,</p>
            <p>Your booking has been successfully cancelled as per your request.</p>
            
            <div class=""ref-badge"">Cancelled Booking Reference: {bookingRef}</div>
            
            <div class=""info-box"">
                <div class=""detail-row"">
                    <span class=""detail-label"">Hotel:</span>
                    <span class=""detail-value"">{hotelName}</span>
                </div>
            </div>
            
            <p><strong>What Happens Next?</strong></p>
            <ul>
                <li>Your refund will be processed within 5-7 business days</li>
                <li>You'll receive a refund confirmation email shortly</li>
                <li>The refund will be credited to your original payment method</li>
            </ul>
            
            <p>We'd love to have you back! If there's anything we could improve, please let us know.</p>
            
            <p style=""color: #666; margin-top: 20px;"">
                Need help? Contact our support team at support@staysimple.com
            </p>
        </div>
        <div class=""footer"">
            <p>&copy; 2024 StaySimple. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
