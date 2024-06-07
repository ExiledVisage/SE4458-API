using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Services
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

    
     public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Checks if a user exists based on their email
        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        // Registers a new user with a hashed password
        public async Task<User> RegisterAsync(User user, string password)
        {
            if (await UserExistsAsync(user.Email))
                throw new Exception("User already exists");

            using var hmac = new HMACSHA512();
            user.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // Authenticates a user by comparing hashed passwords
        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
                throw new Exception("User not found");

            using var hmac = new HMACSHA512();
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (user.PasswordHash != Convert.ToBase64String(computedHash))
                throw new Exception("Password is incorrect");

            return user;
        }

        // Retrieves a user by their ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Updates a user's details
        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Deletes a user by their ID
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}