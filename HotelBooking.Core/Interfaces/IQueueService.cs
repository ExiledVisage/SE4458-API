using HotelBooking.Core.Models;

public interface IQueueService
{
    void SendReservationMessage(ReservationMessage message);
    void ReceiveReservationMessages(Action<ReservationMessage> processMessage);
}