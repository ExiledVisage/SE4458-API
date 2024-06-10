namespace HotelBooking.Core.Interfaces
{
    public interface IEmailService
    {
        void Send(string toEmail, string subject, string body);
    }
}