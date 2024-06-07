using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.DTOs;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            var bookingDtos = bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                User = new BookingUserResponseDto { Id = b.User.Id, Username = b.User.Username },
                Room = new BookingRoomResponseDto { Id = b.Room.Id, Type = b.Room.Type, Price = b.Room.Price },
                StartDate = b.StartDate,
                EndDate = b.EndDate
            }).ToList();
            return Ok(bookingDtos);
        }

        [HttpGet("{id}")]
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
                User = new BookingUserResponseDto { Id = booking.User.Id, Username = booking.User.Username },
                Room = new BookingRoomResponseDto { Id = booking.Room.Id, Type = booking.Room.Type, Price = booking.Room.Price },
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
            return Ok(bookingDto);
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponseDto>> CreateBooking([FromBody] BookingCreateDto bookingCreateDto)
        {
            var newBooking = new Booking
            {
                UserId = bookingCreateDto.UserId,
                RoomId = bookingCreateDto.RoomId,
                StartDate = bookingCreateDto.StartDate,
                EndDate = bookingCreateDto.EndDate
            };

            var createdBooking = await _bookingService.CreateBookingAsync(newBooking);
            var createdBookingDto = new BookingResponseDto
            {
                Id = createdBooking.Id,
                User = new BookingUserResponseDto { Id = createdBooking.User.Id, Username = createdBooking.User.Username },
                Room = new BookingRoomResponseDto { Id = createdBooking.Room.Id, Type = createdBooking.Room.Type, Price = createdBooking.Room.Price },
                StartDate = createdBooking.StartDate,
                EndDate = createdBooking.EndDate
            };
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBookingDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDto bookingDto)
        {
            var bookingToUpdate = await _bookingService.GetBookingByIdAsync(id);
            if (bookingToUpdate == null)
            {
                return NotFound($"Booking with Id = {id} not found.");
            }

            bookingToUpdate.UserId = bookingDto.UserId;
            bookingToUpdate.RoomId = bookingDto.RoomId;
            bookingToUpdate.StartDate = bookingDto.StartDate;
            bookingToUpdate.EndDate = bookingDto.EndDate;

            await _bookingService.UpdateBookingAsync(bookingToUpdate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with Id = {id} not found.");
            }

            await _bookingService.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}