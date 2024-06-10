using System.Net;
using System.Net.Mail;
using HotelBooking.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.Infrastructure.Services 
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string toEmail, string subject, string body)
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPass = _configuration["EmailSettings:SmtpPass"];

            var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            client.Send(mailMessage);
        }
    }
}