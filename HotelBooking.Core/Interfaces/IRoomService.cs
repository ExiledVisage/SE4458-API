using HotelBooking.Core.Models;
namespace HotelBooking.Core.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<Room> CreateRoomAsync(Room room);
        Task<Room> UpdateRoomAsync(int id, Room room);
        Task DeleteRoomAsync(int id);
    }
}