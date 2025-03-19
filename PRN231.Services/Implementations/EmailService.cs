using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace PRN231.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string mail, string subject, string message)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("smartheadproject@gmail.com");
            email.From.Add(MailboxAddress.Parse("smartheadproject@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = message;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate("smartheadproject@gmail.com", "klkx pypc mper auyj");

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
