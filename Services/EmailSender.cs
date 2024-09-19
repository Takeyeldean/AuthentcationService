using AuthentcationServiceForTradingMarket.Interfaces;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp; // Ensure this is the correct import

namespace AuthentcationServiceForTradingMarket.Services
{
    public class EmailSender : IEmailSender
    {

        public async Task<int> SendEmailAsycn(string email)
        {
            var sender = new MimeMessage();
            sender.From.Add(MailboxAddress.Parse("janelle92@ethereal.email"));
            sender.To.Add(MailboxAddress.Parse(email));
            sender.Subject = "Verify your email";
            var otp = Random.Shared.Next(1000, 9999);
            sender.Body = new TextPart(TextFormat.Text)
            {
                Text = $"This is your OTP {otp}. Please DO NOT share it with anyone."
            };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("janelle92@ethereal.email", "Yf2kJH15WJetQ5n4dX");
            smtp.Send(sender);
            smtp.Disconnect(true);

            return otp;
        }

    }
}
