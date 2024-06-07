using System.Collections.Generic;

namespace WebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Booking> Bookings { get; set; }  // Navigation property
    }
}