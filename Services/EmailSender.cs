using AuthentcationServiceForTradingMarket.Interfaces;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace AuthentcationServiceForTradingMarket.Services
{
    public class EmailSender : IEmailSender
    {
      
        public async Task<int> SendEmailAsycn(string email)
        {
            var ma = "tekoo146@gmail.com";
            var pa = "MY_App_Password";

            var sender = new MimeMessage();
            sender.From.Add(MailboxAddress.Parse(ma));
            sender.To.Add(MailboxAddress.Parse(email));
            sender.Subject = "Verify your email";

            var otp = Convert.ToInt32(Random.Shared.Next(1000, 9999));
            sender.Body = new TextPart(TextFormat.Text)
            {
                Text = $"This is your OTP {otp}. Please DO NOT share it with anyone."
            };

            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(ma, pa);
                await smtp.SendAsync(sender);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send email.", ex);
            }

            return otp;
        }
    }
}
