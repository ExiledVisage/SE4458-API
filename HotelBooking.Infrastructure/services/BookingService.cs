using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Core.Models;
using HotelBooking.Core.Interfaces;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Core.DTOs;

namespace HotelBooking.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IQueueService _queueService;

        public BookingService(ApplicationDbContext context, IQueueService queueService)
        {
            _queueService = queueService;
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking> CreateBookingAsync(BookingCreateDto bookingDto)
        {
    
            var overlappingBookings = await _context.Bookings
                .Where(b => b.RoomId == bookingDto.RoomId &&
                            ((bookingDto.StartDate >= b.StartDate && bookingDto.StartDate <= b.EndDate) ||
                            (bookingDto.EndDate >= b.StartDate && bookingDto.EndDate <= b.EndDate) ||
                            (bookingDto.StartDate <= b.StartDate && bookingDto.EndDate >= b.EndDate)))
                .ToListAsync();

            if (overlappingBookings.Any())
            {
                throw new InvalidOperationException("The room is not available for the selected dates.");
            }

            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                RoomId = bookingDto.RoomId,
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> UpdateBookingAsync(int id, BookingCreateDto bookingDto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return null;
            }

            booking.UserId = bookingDto.UserId;
            booking.RoomId = bookingDto.RoomId;
            booking.StartDate = bookingDto.StartDate;
            booking.EndDate = bookingDto.EndDate;

            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var message = new ReservationMessage
            {
            UserId = bookingDto.UserId,
            RoomId = bookingDto.RoomId,
            StartDate = bookingDto.StartDate,
            EndDate = bookingDto.EndDate
            };

            _queueService.SendReservationMessage(message);
            return booking;
        }

        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}    