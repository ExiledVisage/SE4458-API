using HotelBooking.Core.Models;
namespace HotelBooking.Core.DTOs
{
    public class RoomDto
    {
        public string? Type { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class SearchCriteriaDto
    {
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfPeople { get; set; }
    }


    public class RoomResponseDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class RoomRegisterDto
    {
        public int HotelId { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}