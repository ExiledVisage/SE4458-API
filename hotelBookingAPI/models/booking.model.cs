using RoomModel;
using UserModel;

namespace BookingModel
{
    public class Booking
    {       
        public int Id { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }

        public int RoomId { get; set; }
        public required Room Room { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
