using System.Net;
using System.Net.Mail;

namespace Site.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // Ayarları appsettings.json'dan oku
            var mailServer = _configuration["EmailSettings:MailServer"];
            var mailPort = int.Parse(_configuration["EmailSettings:MailPort"]);
            var senderName = _configuration["EmailSettings:SenderName"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:Password"];

            var client = new SmtpClient(mailServer, mailPort)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true // HTML formatında mail atabilmek için
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}