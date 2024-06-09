using HotelBooking.Core.Models;
namespace HotelBooking.Core.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<Hotel> CreateHotelAsync(Hotel hotel);
        Task<Hotel> UpdateHotelAsync(int id, Hotel hotel);
        Task DeleteHotelAsync(int id);
    }
}