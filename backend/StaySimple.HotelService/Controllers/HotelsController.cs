using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaySimple.HotelService.DTOs;
using StaySimple.HotelService.Services.Interfaces;

namespace StaySimple.HotelService.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelsController:ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetAll()
        {
            var result = await _hotelService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")] // this will get the id from url like this -> eg (/api/hotels/5)
        public async Task<ActionResult<HotelDto>> GetById(int id)
        {
            var result = await _hotelService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search")] // this will get the data from query parameter like this -> eg (/api/hotels/search?city=Delhi)
        public async Task<ActionResult<IEnumerable<HotelDto>>> Search([FromQuery] string city) // this would work the same without [FromQuery] as its a simple type
        {
            var hotels = await _hotelService.SearchAsync(city);
            return Ok(hotels);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<HotelDto>> Create(CreateHotelDto dto) // this dto will be passed from the body(POST/PUT) eg(/api/hotels) {"name" : "adfs",...}
        {
            var hotel = await _hotelService.CreateAsync(dto);
            if (hotel == null) return NotFound();
            return Ok(hotel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<HotelDto>> Update(int id, CreateHotelDto dto) // here for PUT its mixed,request - api/hotels/5 and in body, the dto
        {
            var hotel = await _hotelService.UpdateAsync(id, dto);
            return hotel == null ? NotFound() : Ok(hotel);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _hotelService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        

    }
}
