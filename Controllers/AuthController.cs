using AuthentcationServiceForTradingMarket.Interfaces;
using AuthentcationServiceForTradingMarket.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AuthentcationServiceForTradingMarket.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, IEmailSender emailSender, ApplicationDbContext dbcontext, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _emailSender = emailSender;
            _dbcontext = dbcontext;
            _userManager = userManager;

        }



        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated) return BadRequest(result.Message);

            var otp = await _emailSender.SendEmailAsycn(model.Email);
            var otpandemail = new OtpAndEmail
            {
                ID = 0,
                OTP = otp,
                Email = model.Email,
            };
            _dbcontext.Set<OtpAndEmail>().Add(otpandemail);
            _dbcontext.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"your id to confirm an email is{otpandemail.ID}");
            Console.ForegroundColor = ConsoleColor.White;

            return Ok(result);

        }




        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(int ID, int OTP)
        {
            // Find the record in the OtpAndEmail table
            var user = await _dbcontext.OtpsAndEmails.FindAsync(ID);

            if (user == null)
            {
                return BadRequest("Incorrect Email");
            }
            if (user.OTP != OTP)
            {
                return BadRequest("Incorrect OTP");
            }

            var result = await _userManager.FindByEmailAsync(user.Email);
            if (result == null)
            {
                return NotFound("User not found");
            }

            result.IsEmailConfirmed = true;

            var updateResult = await _userManager.UpdateAsync(result);
            if (!updateResult.Succeeded)
            {
                return BadRequest("Error confirming email");
            }

            return Ok("Email confirmed successfully");
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
