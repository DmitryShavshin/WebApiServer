using Microsoft.Extensions.Configuration;
using RestSharp.Authenticators;
using RestSharp;

namespace WebApiServer.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration= configuration;
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
