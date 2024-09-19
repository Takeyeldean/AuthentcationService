using AuthentcationServiceForTradingMarket.Helpers;
using AuthentcationServiceForTradingMarket.Interfaces;
using AuthentcationServiceForTradingMarket.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthentcationServiceForTradingMarket.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly IEmailSender _emailSender;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt,IEmailSender emailSender)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _emailSender = emailSender;
        }
        
        // Register Function
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            // Check on Email and Username should be uniqe
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AuthModel { Message = "Email not valid" };
            }
            if (await _userManager.FindByNameAsync(model.Username) != null)
            {
                return new AuthModel { Message = "Username not valid" };
            }
           

            var user = new ApplicationUser
            {

                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsEmailConfirmed = false,
            };
            var resutl = await _userManager.CreateAsync(user, model.password);// Add it
          
          
            if (!resutl.Succeeded) 
            {
                string errors = "";          
                
                foreach(var error in resutl.Errors)
                {
                    errors += $"{error}, ";

                    //Console.WriteLine($"Error Code: {error.Code}");
                    //Console.WriteLine($"Error Description: {error.Description}");
                }
                return new AuthModel { Message = errors };
            }
            
            // Default role is User Role
            await _userManager.AddToRoleAsync(user,"User");


            // Create Token
            var jwtSecurityToken = await CreateJwtToken(user);

            // Return authentcation model
            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                IsEmailConfirmed = user.IsEmailConfirmed,
            };


        }

        // Login Function
        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authmodel = new AuthModel();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authmodel.Message = "Email or password is wrong";
                return authmodel;
            }
            if(!user.IsEmailConfirmed)
            {
                authmodel.Message = "Please confirme your email first";
                return authmodel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var roleslist = await _userManager.GetRolesAsync(user);

            authmodel.IsAuthenticated = true;
            authmodel.Email = user.Email;
            authmodel.ExpiresOn = jwtSecurityToken.ValidTo;
            authmodel.IsAuthenticated = true;
            authmodel.Roles = roleslist.ToList();
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authmodel.Username = user.UserName;


            return authmodel;

        }

        private async Task<SecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>(); 

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
            audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;

        }
    }
}
