using HotelBooking.Core.DTOs;
using HotelBooking.Core.Models;
namespace HotelBooking.Core.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> SearchRoomsAsync(SearchCriteriaDto criteria);
        Task<Room> RegisterRoomAsync(RoomRegisterDto roomDto);
        Task<Room> UpdateRoomAsync(int id, Room updatedRoom);
        Task DeleteRoomAsync(int id);
    }
}