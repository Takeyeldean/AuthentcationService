using System.ComponentModel.DataAnnotations;

namespace AuthentcationServiceForTradingMarket.Model
{
    public class TokenRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}