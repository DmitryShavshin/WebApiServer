using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiServer.DTOs;
using WebApiServer.Interfaces;
using WebApiServer.Models;

namespace WebApiServer.Repository
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResult> Login(LoginDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Login);
            if (existingUser != null)
            {
                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, request.Password);
                if (!isCorrect)
                    return (new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() { "Invalid Credentials" }
                    }); 

                var jwtToken = GenerateJwtToken(existingUser);
                return (new AuthResult()
                {
                    Id = existingUser.Id,
                    Result = true,
                    Token = jwtToken
                });

            }
            return (new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Invalid payload" }
            });

        }

        public async Task<bool> Registration(RegistrationDto request)
        {
            if (request.Password == request.PasswordConfirm) 
            { 
                var userExist = await _userManager.FindByEmailAsync(request.Email);
                if(userExist == null)
                {
                    // Create new user
                    User newUser = new User()
                    {
                        UserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = false
                    };

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var emailBody = "Please confirm your email address <a href \" #URL#\">Click heare";
                    //var callBackUrl = Request.Scheme

                    var isCreated = await _userManager.CreateAsync(newUser, request.Password);

                    if (isCreated.Succeeded)
                        return true;
                }
            }
            return false;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    //new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
