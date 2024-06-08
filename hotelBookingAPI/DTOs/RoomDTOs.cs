namespace WebAPI.DTOs
{
    public class RoomDto
    {
        public string? Type { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class RoomResponseDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}