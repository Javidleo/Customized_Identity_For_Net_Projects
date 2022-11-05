using Identity.DataSource;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.Services.IdentityServices.UserStore
{
    public class AppUserStore : UserStore<AppUser, AppRole, ApplicationDbContext, int, AppUserClaim, AppUserRole,
                                            AppUserLogin, AppUserTokens, AppRoleClaim>
    {
        public AppUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        protected override AppUserClaim CreateUserClaim(AppUser user, Claim claim)
        {
            var userClaim = new AppUserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        protected override AppUserLogin CreateUserLogin(AppUser user, UserLoginInfo login)
        {
            return new AppUserLogin
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey,
            };
        }

        protected override AppUserRole CreateUserRole(AppUser user, AppRole role)
        {
            return new AppUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
        }

        protected override AppUserTokens CreateUserToken(AppUser user, string loginProvider, string name, string value)
        {
            return new AppUserTokens
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
        }
    }
}
