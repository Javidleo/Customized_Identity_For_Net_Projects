using Identity.DataSource;
using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Services.IdentityServices.SignInManagement
{
    public class AppSignInManager : SignInManager<AppUser>, IAppSignInManager
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<AppUser> _users;
        private readonly DbSet<AppUserLogin> _appUserLogin;

        public AppSignInManager(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor,
                                IUserClaimsPrincipalFactory<AppUser> claimsFactory,
                                IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<AppUser>> logger,
                                IAuthenticationSchemeProvider schemes, IUserConfirmation<AppUser> confirmation,
                                    ApplicationDbContext context)
                                            : base(userManager, contextAccessor, claimsFactory, optionsAccessor,
                                                    logger, schemes, confirmation)
        {
            _context = context;
            _users = _context.Set<AppUser>();
            _appUserLogin = _context.Set<AppUserLogin>();
        }

        public async Task<SignInResult> SmsVerificationSignIn(int userId)
        {
            try
            {
                await _appUserLogin.AddAsync(new AppUserLogin()
                {
                    ProviderKey = Guid.NewGuid().ToString(),
                    LoginProvider = "Message Login",
                    UserId = userId

                });
                await _context.SaveChangesAsync();
                return SignInResult.Success;
            }
            catch (Exception)
            {
                return SignInResult.Failed;
            }
        }
    }
}
