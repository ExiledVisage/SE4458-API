using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Core.Interfaces;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Core.Models;

namespace HotelBooking.Infrastructure.Notifications
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IQueueService _queueService;
        private readonly IEmailService _emailService;

        public NotificationService(ApplicationDbContext context, IQueueService queueService, IEmailService emailService)
        {
            _context = context;
            _queueService = queueService;
            _emailService = emailService;
        }

        public async Task NotifyLowCapacity()
        {
            var oneMonthFromNow = DateTime.Now.AddMonths(1);
            var hotels = await _context.Hotels
                .Include(h => h.Rooms)
                .ToListAsync();

            foreach (var hotel in hotels)
            {
                var totalCapacity = hotel.Rooms.Sum(r => r.Capacity);
                var reservedCapacity = await _context.Bookings
                    .Where(b => b.Room.HotelId == hotel.Id && b.StartDate <= oneMonthFromNow)
                    .SumAsync(b => b.Room.Capacity);

                if (reservedCapacity / (double)totalCapacity >= 0.8)
                {
                    // Notify all admins
                    var admins = await _context.Users
                        .Where(u => u.Role == UserRole.Admin)
                        .ToListAsync();
                    
                    foreach (var admin in admins)
                    {
                        // Implement email notification logic here
                        SendEmailNotification(admin.Email, hotel.Name);
                    }
                }
            }
        }

        private void SendEmailNotification(string email, string hotelName)
        {
            _emailService.Send(email, "Low Capacity Alert", $"The capacity for hotel {hotelName} is below 20%");
        }

        public void ProcessReservationQueue()
        {
            _queueService.ReceiveReservationMessages(async message =>
            {
                var user = await _context.Users.FindAsync(message.UserId);
                var room = await _context.Rooms.FindAsync(message.RoomId);
                
                if (user != null && room != null)
                {
                    var body = $"Dear {user.Username},<br/><br/>" +
                               $"Your reservation for room {room.Type} from {message.StartDate:dd MMM yyyy} to {message.EndDate:dd MMM yyyy} has been confirmed.<br/><br/>" +
                               "Thank you for choosing our service!";
                    _emailService.Send(user.Email, "Reservation Confirmation", body);
                }
            });
        }
    }
}