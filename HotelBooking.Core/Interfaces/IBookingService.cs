using HotelBooking.Core.DTOs;
using HotelBooking.Core.Models;
namespace HotelBooking.Core.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<Booking> CreateBookingAsync(BookingCreateDto bookingDto);
        Task<Booking> UpdateBookingAsync(int id, BookingCreateDto bookingDto);
        Task DeleteBookingAsync(int id);
    }
}