using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaySimple.HotelService.DTOs;
using StaySimple.HotelService.Services.Implementations;
using StaySimple.HotelService.Services.Interfaces;

namespace StaySimple.HotelService.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetByHotel(int hotelId)
        => Ok(await _roomService.GetByHotelAsync(hotelId));

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            return room == null ? NotFound() : Ok(room);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<RoomDto>> Create(CreateRoomDto dto)
            => Ok(await _roomService.CreateAsync(dto));

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomDto>> Update(int id, CreateRoomDto dto)
        {
            var updated = await _roomService.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _roomService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("{roomId}/availability")]
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var result = await _roomService.CheckAvailabilityAsync(roomId, checkIn, checkOut);

            if (!result.RoomExists)
                return NotFound(new { message = "Room not found" });

            return Ok(new { isAvailable = result.IsAvailable });
        }

    }
}
