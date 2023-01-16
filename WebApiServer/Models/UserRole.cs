using Microsoft.AspNetCore.Identity;

namespace WebApiServer.Models
{
    public class UserRole: IdentityUserRole<Guid>
    {
        public DateTime AddedToRole { get; set; }
    }
}
