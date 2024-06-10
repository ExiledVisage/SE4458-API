namespace HotelBooking.Core.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }  // Foreign key for Hotel
        public string? Type { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public Hotel? Hotel { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}