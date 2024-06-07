namespace WebAPI.DTOs
{
    public class BookingCreateDto
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BookingResponseDto
    {
        public int Id { get; set; }
        public required BookingUserResponseDto User { get; set; }
        public required BookingRoomResponseDto Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BookingUserResponseDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
    }

    public class BookingRoomResponseDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public decimal Price { get; set; }
    }
}