using AuthentcationServiceForTradingMarket.Interfaces;
using AuthentcationServiceForTradingMarket.Model;
using Microsoft.AspNetCore.Mvc;

namespace AuthentcationServiceForTradingMarket.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;

        public AuthController(IAuthService authService, IEmailSender emailSender)
        {
            _authService = authService;
            _emailSender = emailSender;
        }
        [HttpPost("email")]
        public async Task<IActionResult> RegisterEmail(string email)
        {
            Console.WriteLine("Please check your email");
            var input = Convert.ToInt32(Console.ReadLine());
         var otp =  await _emailSender.SendEmailAsycn(email);
            if (otp == input) return BadRequest("valid");
            else return BadRequest("not valid");
          
          
        }
            
       [HttpPost("Register")]
      public  async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated) return BadRequest(result.Message);

            return Ok(result);

        }


        [HttpPost("Login")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated) return BadRequest(result.Message);
            return Ok(result);

        }



    }
}
