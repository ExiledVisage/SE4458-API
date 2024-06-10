using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core.DTOs;
using HotelBooking.Core.Interfaces;
using HotelBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("get-all-bookings")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            var bookingDtos = bookings.Select(booking => new BookingResponseDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                RoomId = booking.RoomId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                UserName = booking.User?.Username,  
                RoomType = booking.Room?.Type  
            });
            return Ok(bookingDtos);
        }

        [HttpGet("get-booking/{id}")]
        public async Task<ActionResult<BookingResponseDto>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingDto = new BookingResponseDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                RoomId = booking.RoomId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                UserName = booking.User?.Username,  
                RoomType = booking.Room?.Type  
            };
            return Ok(bookingDto);
        }

        [HttpPost("create-booking")]
        public async Task<ActionResult<BookingResponseDto>> CreateBooking([FromBody] BookingCreateDto bookingDto)
        {
            if (bookingDto == null)
            {
                return BadRequest("Booking details are required.");
            }

            var booking = await _bookingService.CreateBookingAsync(bookingDto);
            var bookingResponseDto = new BookingResponseDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                RoomId = booking.RoomId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, bookingResponseDto);
        }

        [HttpPut("update-booking/{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingCreateDto bookingDto)
        {
            if (bookingDto == null)
            {
                return BadRequest("Booking details are required.");
            }

            var updatedBooking = await _bookingService.UpdateBookingAsync(id, bookingDto);
            if (updatedBooking == null)
            {
                return NotFound($"Booking with Id = {id} not found.");
            }

            return NoContent();
        }

        [HttpDelete("delete-booking/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}