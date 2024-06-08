using WebAPI.Models;

namespace WebAPI.DTOs
{
    // DTO for user registration
    public class UserRegisterDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; } = UserRole.NormalUser;
    }

    // DTO for user login
    public class UserLoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    // DTO for updating user information
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}