using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Services;
using HotelBooking.Core.Interfaces;
using HotelBooking.Infrastructure.Notifications;
using Hangfire;
using Hangfire.MemoryStorage;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure PostgreSQL database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register application services
            builder.Services.AddScoped<IHotelService, HotelService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<IQueueService, QueueService>();
            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();

            // Add controllers
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHangfire(config => config.UseMemoryStorage());
            builder.Services.AddHangfireServer();


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.MapControllers();
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate<NotificationService>("NotifyLowCapacity", service => service.NotifyLowCapacity(), Cron.Daily);
            RecurringJob.AddOrUpdate<NotificationService>("ProcessReservationQueue", service => service.ProcessReservationQueue(), Cron.Daily);

            app.Run();
        }
    }
}