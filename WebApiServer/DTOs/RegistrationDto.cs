using System.ComponentModel.DataAnnotations;

namespace WebApiServer.DTOs
{
    public class RegistrationDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}
