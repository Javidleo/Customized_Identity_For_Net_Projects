using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public class AppUserClaim : IdentityUserClaim<int>
    {
        public virtual AppUser User { get; set; }
    }
}
