using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Core.Models;
using HotelBooking.Infrastructure.Services;
using HotelBooking.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.Core.Interfaces;
using HotelBooking.Core.Models;
using HotelBooking.Core.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet("get-all-hotels")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetAllHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("get-hotel/{id}")]
        public async Task<ActionResult<Hotel>> GetHotelById(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpPost("create-hotel")]
        
        public async Task<ActionResult<Hotel>> CreateHotel([FromBody] HotelDto hotelDto)
        {
            if (hotelDto == null)
            {
                return BadRequest("Invalid hotel data");
            }

            var newHotel = new Hotel
            {
                Name = hotelDto.Name,
                Location = hotelDto.Location
            };

            var createdHotel = await _hotelService.CreateHotelAsync(newHotel);
            return CreatedAtAction(nameof(GetHotelById), new { id = createdHotel.Id }, createdHotel);
        }

        [HttpPut("update-hotel/{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelDto hotelDto)
        {
            var hotelToUpdate = await _hotelService.GetHotelByIdAsync(id);
            if (hotelToUpdate == null)
            {
                return NotFound($"Hotel with Id = {id} not found.");
            }

            hotelToUpdate.Name = hotelDto.Name;
            hotelToUpdate.Location = hotelDto.Location;
            await _hotelService.UpdateHotelAsync(id, hotelToUpdate);

            return NoContent();
        }

        [HttpDelete("delete-hotel/{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound($"Hotel with Id = {id} not found.");
            }

            await _hotelService.DeleteHotelAsync(id);
            return NoContent();
        }
    }
}