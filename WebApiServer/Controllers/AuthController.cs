using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.Text;
using WebApiServer.DTOs;
using WebApiServer.Interfaces;
using WebApiServer.Models;

namespace WebApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //public readonly IAuth _auth;

        //public AuthController(IAuth auth)
        //{
        //    _auth = auth;
        //}


        public readonly IAuth _auth;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, IAuth auth)
        {
            _auth = auth;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AuthResult>> Login(LoginDto request)
        {
            if (ModelState.IsValid) 
            { 
                if (request == null) 
                {
                    return NotFound();
                }
                else
                {
                    AuthResult result = await _auth.Login(request);
                
                    if (result != null)
                        return Ok(result);  
                }
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("Registration")]
        public async Task<ActionResult<User>> Registration(RegistrationDto request)
        {

            if (request.Password == request.PasswordConfirm)
            {
                var userExist = await _userManager.FindByEmailAsync(request.Email);
                if (userExist == null)
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
                    var callBackUrl = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Auth", new { userId = newUser.Id, code = code });
                    var body = emailBody.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callBackUrl));

                    var result = SendEmail(body, newUser.Email);

                    if (result)
                        return Ok("Please verify your email, trough the werification email we just send!");

                    return Ok("Please request an email werification link");
                    
                    
                    
                    var isCreated = await _userManager.CreateAsync(newUser, request.Password);

                    if (isCreated.Succeeded)
                        return Ok();
                }
            }





            //if (ModelState.IsValid)
            //{
            //    if (request == null)
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        bool result = await _auth.Registration(request);

            //        if (result)
            //            return Ok("Registration was succseded");
            //    }
            //}
            return BadRequest();
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(Guid userId, string code)
        {
            if(userId == null || code == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string> { "Invalid email confirmation url" }
                });
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if(user == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string> { "Invalid email parameters" }
                });
            }

            code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var status = result.Succeeded 
                ? "Thank you for confirming your email" 
                : "Your email not confirmed, please try again later";

            return Ok();
        }



        private bool SendEmail(string body, string email)
        {
            var client = new RestClient("https://api.mailgun.net/v3");
            var request = new RestRequest("", Method.Post);
            var API_KEY = _configuration.GetSection("EmailConfig:API_KEY").Value;
            client.Authenticator = new HttpBasicAuthenticator("api", API_KEY);
            request.AddParameter("domain", "sandbox6c34104b1465483a85377d7499094915.mailgun.org");
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Online Shop <OnlineShop@mail.com>");
            request.AddParameter("to", "lineageiivk@gmail.com");
            request.AddParameter("subject", "Email Verification");
            request.AddParameter("text", body);
            request.Method = Method.Post;
            var response = client.Execute(request);
            return response.IsSuccessful;
        }
    }
}
