using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiServer.Models
{
    public class User: IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastSignInDate { get; set; }



        //public virtual IEnumerable<Order> Orders { get; set; }
        //public virtual UserHomeAddress HomeAddress { get; set; }
        //public virtual UserWorkAddress WorkAddress { get; set; }
        //public virtual UserContact UserContact { get; set; }
    }
}
