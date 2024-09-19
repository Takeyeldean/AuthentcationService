namespace AuthentcationServiceForTradingMarket.Interfaces
{
    public interface IEmailSender
    {
        Task<int> SendEmailAsycn(string email);
    }
}
