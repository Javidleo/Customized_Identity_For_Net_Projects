using Identity.Common;
using Identity.Common.Exceptions;
using Identity.Models;
using Identity.Services.ApplicationServices.Authentication;
using Identity.Services.ApplicationServices.Role;
using Identity.Services.ApplicationServices.TokenStore;
using Identity.Services.IdentityServices.SignInManagement;
using Identity.Services.IdentityServices.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Identity.Services.ApplicationServices.UserPasswordLogin
{
    public class PasswordLoginService
    {
        private readonly IAppSignInManager _appSignInManger;
        private readonly IAppUserManager _userManager;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleService _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PasswordLoginService(IAppSignInManager appSignInManger, IAppUserManager userManager, ITokenStoreService tokenStoreService,
                                    IAuthenticationService authenticationService, IRoleService roleService, IHttpContextAccessor httpContextAccessor)
        {
            _appSignInManger = appSignInManger;
            _userManager = userManager;
            _tokenStoreService = tokenStoreService;
            _authenticationService = authenticationService;
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(IdentityResult, JWTTokenData, string)> HandleAsync(string userName, string Password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is not null)
            {
                if (user.EmailConfirmed == false)
                {
                    // sending Email to user email again and let it know we are waiting for verify email

                    return (IdentityResult.Success, null, "Please verify your Email Address");
                }
                var token = await _authenticationService.GenerateToken(user);
                if (token is null)
                    throw new NotAcceptableException("invalid token");

                return (IdentityResult.Success, token, "You Already Have an Account");
            }
            else
            {
                var registerResult = await RegisterAsync(userName, Password);

                return (registerResult.Item1, registerResult.Item2, "successfully registerd");
            }

        }

        public async Task<(IdentityResult, JWTTokenData)> RegisterAsync(string userName, string password)
        {
            //var user = AppUser.Create(userName);
            //var result = await _userManager.CreateAsync(user, password);
            //if (result.Errors.Any())
            //    return (result, null);

            //var userRoleAssignment = await _roleService.SetUserRole("Customer", user.Id);
            //if(userRoleAssignment.Succeeded)
            //{
            //    var token = await _authenticationService.GenerateTokenAsync(user);
            //    return (IdentityResult.Success, token);
            //}
            return (IdentityResult.Failed(), null);
        }
        public async Task<JWTTokenData> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                throw new NotFoundException("user notfound");

            var signInResult = await _appSignInManger.PasswordSignInAsync(user, password, false, true);
            if (signInResult.Succeeded)
            {
                await _tokenStoreService.RevokeUserTokens(user.Guid, null);

                //var token = await _authenticationService.GenerateTokenAsync(user);
                //if (token is null)
                //    throw new NotAcceptableException("null token");

                //return token;
            }
            if (signInResult.IsLockedOut)
                throw new ForbiddenException("User Account Locked");

            if (signInResult.IsNotAllowed)
                throw new ForbiddenException("user not allow to login");

            if (signInResult.RequiresTwoFactor)
                throw new ForbiddenException("user enabled two factor its not implemented yet");

            return null;
        }

        public async Task LogOutAsync(string? refreshToken)
        {
            var claimIdentity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            string guid = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Regex.IsMatch(guid, CustomRegex.Guid))
                throw new NotAcceptableException("invalid userId");

            var userGuid = new Guid(guid);

            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new NotAcceptableException("invalid request");

            await _tokenStoreService.RevokeUserTokens(userGuid, refreshToken);
        }


    }
}
