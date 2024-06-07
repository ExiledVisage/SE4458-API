namespace WebAPI.DTOs
{
    // DTO for user registration
    public class UserRegisterDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
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