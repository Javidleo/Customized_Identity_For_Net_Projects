using Identity.Common;
using Identity.Common.Exceptions;
using Identity.Models;
using Identity.Services.ApplicationServices.Authentication;
using Identity.Services.IdentityServices.SignInManagement;
using Identity.Services.IdentityServices.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Services.ApplicationServices.ExternalProviderLogin
{
    public class ExternalProviderLoginService : IExternalProviderLoginService
    {
        private readonly IAppSignInManager _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAppUserManager _userManager;

        public ExternalProviderLoginService(IAppSignInManager signInManager, IAuthenticationService authenticationService,
                                                IAppUserManager userManager)
        {
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        // this method take the name of the provider you want to use as the login provider and your 
        public ChallengeResult GetExternalProviderProperties(string loginProvider, string url)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(loginProvider, url);

            return new ChallengeResult(loginProvider, properties);
        }


        public async Task<JWTTokenData> ExternalProviderLoginAsync(string loginProvider)
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();

            string email = loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value ?? null;
            if (email is null)
                throw new BadRequestException("invalid request / Empty Email Address");

            string firstName = loginInfo.Principal.FindFirst(ClaimTypes.GivenName)?.Value ?? null;
            string lastName = loginInfo.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? null;

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null) // register and login new User
            {
                var registrationResult = await LoginNewUser(user.Email, user.FirstName, user.LastName, loginInfo);
                if (registrationResult is null)
                    throw new NotAcceptableException("null Token");

                return registrationResult;
            }
            else // login existing User
            {
                var loginResult = await LoginExistingUser(user, loginProvider, loginInfo.ProviderKey);
                if (loginResult is null)
                    throw new NotAcceptableException("null token");

                return loginResult;
            }
        }

        private async Task<JWTTokenData> LoginNewUser(string email, string? firstName, string? lastName, ExternalLoginInfo loginInfo)
        {
            // because we login from external providers we can accept the email address as a valid one;
            // so in this create method we set the emailConfirmed true by default;
            var user = AppUser.Create(email, firstName, lastName);

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                // adding user Login
                await _userManager.AddLoginAsync(user, loginInfo);
                // sign in with out password
                await _signInManager.SignInAsync(user, false);
                // generate token for user
                return await _authenticationService.GenerateToken(user);
            }
            // if having any error , then => 
            var errorMessage = IdentityErrorHandler.MergeErrorMessages(result.Errors);
            throw new NotAcceptableException(errorMessage);
        }

        private async Task<JWTTokenData> LoginExistingUser(AppUser? user, string loginProvider, string providerKey)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, false);
            if (signInResult.Succeeded)
            {
                var token = await _authenticationService.GenerateToken(user);
                return token;
            }
            if (signInResult.IsLockedOut)
                throw new NotAcceptableException("user account is locked");

            if (signInResult.IsNotAllowed)
                throw new NotAcceptableException("user not allow to do this action");

            if (signInResult.RequiresTwoFactor)
            {
                // do something about two factor authentication
            }
            return null;
        }
    }
}
