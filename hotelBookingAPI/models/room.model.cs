using HotelModel;

namespace RoomModel
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public required Hotel Hotel { get; set; }

        public string? Type { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
