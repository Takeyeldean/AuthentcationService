using AuthentcationServiceForTradingMarket.Model;

namespace AuthentcationServiceForTradingMarket.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);

        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
    }
}
