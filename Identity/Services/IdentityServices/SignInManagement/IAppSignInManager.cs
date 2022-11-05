using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.IdentityServices.SignInManagement
{
    public interface IAppSignInManager
    {
        #region Base Methods
        UserManager<AppUser> UserManager { get; set; }
        Task SignInAsync(AppUser user, bool isPersistent, string authenticationMethod = null);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure);
        #endregion
        Task<SignInResult> SmsVerificationSignIn(int userId);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
    }
}
