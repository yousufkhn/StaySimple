using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaySimple.BookingService.DTOs;
using StaySimple.BookingService.Services;
using StaySimple.BookingService.Services.Interfaces;
using System.Security.Claims;

namespace StaySimple.BookingService.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        => Ok(await _bookingService.GetAllAsync());

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMine()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _bookingService.GetMineAsync(userId));
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create(CreateBookingDto dto)
        {
            try
            {
                var result = await _bookingService.CreateAsync(dto, User);

                if (result == null)
                    return BadRequest(new { message = "Invalid booking request." });

                return Ok(result);
            }
            catch (BookingValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var success = await _bookingService.CancelAsync(id, userId, role);

            if (!success)
                return BadRequest(new { message = "Cannot cancel booking." });

            return Ok(new { message = "Booking cancelled." });
        }

    }
}
