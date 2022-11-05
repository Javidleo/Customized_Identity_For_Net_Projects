using Identity.DataSource;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.Services.IdentityServices.RoleStore
{
    public class AppRoleStore : RoleStore<AppRole, ApplicationDbContext, int, AppUserRole, AppRoleClaim>
    {
        public AppRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        protected override AppRoleClaim CreateRoleClaim(AppRole role, Claim claim)
        {
            return new AppRoleClaim
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
            };
        }

    }
}
