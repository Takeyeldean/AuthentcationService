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

            var ma = "adonis.welch22@ethereal.email";
            var pa = "WsXANVTurd1SHdCQrs";


            var sender = new MimeMessage();
            sender.From.Add(MailboxAddress.Parse(ma));
            sender.To.Add(MailboxAddress.Parse(email));
            sender.Subject = "Verify your email";
            var otp =Convert.ToInt32( Random.Shared.Next(1000, 9999));
            sender.Body = new TextPart(TextFormat.Text)
            {
                Text = $"This is your OTP {otp}. Please DO NOT share it with anyone."
            };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(ma,pa);
            smtp.Send(sender);
            smtp.Disconnect(true);

            return otp;
        }

    }
}
