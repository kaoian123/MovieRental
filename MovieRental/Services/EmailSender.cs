using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;

namespace MovieRental.Services
{
    public class EmailSender(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IConfiguration _configuration = configuration;

        public async Task SendPasswordResetEmailAsync(string email, string callbackUrl)
        {
            var htmlMessage = $"請重設您的密碼，點擊 <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>這裡</a>.";
            await SendEmailAsync(email, "重設密碼", htmlMessage);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpSettings = _configuration.GetSection("EmailSettings:SMTP").Get<SmtpSettings>();
            var client = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
            {
                Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
                EnableSsl = smtpSettings.EnableSSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings.From),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }

        private class SmtpSettings
        {
            public required string Host { get; set; }
            public int Port { get; set; }
            public required string Username { get; set; }
            public required string Password { get; set; }
            public required string From { get; set; }
            public bool EnableSSL { get; set; }
        }
    }
}
