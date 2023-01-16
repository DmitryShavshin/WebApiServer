namespace WebApiServer.Models
{
    public class AuthResult
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }
}
