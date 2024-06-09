using HotelBooking.Core.Models;
namespace HotelBooking.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string email);
        Task<User> RegisterAsync(User user, string password);
        Task<User> LoginAsync(string email, string password);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }    
}