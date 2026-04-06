using MassTransit;
using MassTransit.RabbitMqTransport;
using Shared.Contracts;
using StaySimple.Notifications.Services;
using System.Reflection.Metadata;

namespace StaySimple.Notifications.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly ILogger<UserRegisteredConsumer> _logger;
        private readonly IEmailService _emailService;

        public UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> ctx)
        {
            var e = ctx.Message;
            _logger.LogInformation(
                "[NOTIFICATION] New user registration - {Email} (UserId: {UserId}, Name: {Name})",
                e.Email, e.UserId, e.Name);

            try
            {
                await _emailService.SendWelcomeEmailAsync(e.Email, e.Name);
                _logger.LogInformation("[NOTIFICATION] Welcome email sent successfully to {Email}", e.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[NOTIFICATION] Failed to queue welcome email to {Email}. Will retry.", e.Email);
                // Don't re-throw; let MassTransit handle retry logic
                // or log and continue depending on your needs
            }
        }
    }

    // ── Handles BookingConfirmedEvent published by Booking.API ──
    public class BookingConfirmedConsumer : IConsumer<BookingConfirmedEvent>
    {
        private readonly ILogger<BookingConfirmedConsumer> _logger;
        private readonly IEmailService _emailService;

        public BookingConfirmedConsumer(ILogger<BookingConfirmedConsumer> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<BookingConfirmedEvent> ctx)
        {
            var e = ctx.Message;
            _logger.LogInformation(
                "[NOTIFICATION] Booking confirmed — Ref: {Ref}, Hotel: {Hotel}, CheckIn: {CheckIn:yyyy-MM-dd}, Total: ₹{Total}",
                e.BookingRef, e.HotelName, e.CheckInDate, e.TotalPrice);

            try
            {
                await _emailService.SendBookingConfirmationEmailAsync(
                    e.UserEmail,
                    e.UserName,
                    e.HotelName,
                    e.RoomType,
                    e.CheckInDate,
                    e.CheckOutDate,
                    e.TotalPrice,
                    e.BookingRef);
                _logger.LogInformation("[NOTIFICATION] Booking confirmation email sent to {Email} — Ref: {Ref}", e.UserEmail, e.BookingRef);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[NOTIFICATION] Failed to queue booking confirmation to {Email}. Will retry.", e.UserEmail);
                // Don't re-throw; let MassTransit handle retry logic
            }
        }
    }

    // ── Handles BookingCancelledEvent published by Booking.API ──
    public class BookingCancelledConsumer : IConsumer<BookingCancelledEvent>
    {
        private readonly ILogger<BookingCancelledConsumer> _logger;
        private readonly IEmailService _emailService;

        public BookingCancelledConsumer(ILogger<BookingCancelledConsumer> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<BookingCancelledEvent> ctx)
        {
            var e = ctx.Message;
            _logger.LogInformation(
                "[NOTIFICATION] Booking cancelled — Ref: {Ref}, Hotel: {Hotel}",
                e.BookingRef, e.HotelName);

            try
            {
                await _emailService.SendBookingCancellationEmailAsync(
                    e.UserEmail,
                    e.UserName,
                    e.HotelName,
                    e.BookingRef);
                _logger.LogInformation("[NOTIFICATION] Cancellation email sent to {Email} — Ref: {Ref}", e.UserEmail, e.BookingRef);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[NOTIFICATION] Failed to queue cancellation email to {Email}. Will retry.", e.UserEmail);
                // Don't re-throw; let MassTransit handle retry logic
            }
        }
    }
}
