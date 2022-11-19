using Identity.Common;
using Identity.Common.Exceptions;
using Identity.Models;
using Identity.Services.ApplicationServices.Authentication;
using Identity.Services.ApplicationServices.Role;
using Identity.Services.ApplicationServices.TokenFactory;
using Identity.Services.ApplicationServices.TokenStore;
using Identity.Services.IdentityServices.SignInManagement;
using Identity.Services.IdentityServices.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Identity.Services.ApplicationServices.UserPasswordLogin
{
    public class PasswordLoginService : IPasswordLoginService
    {
        private readonly IAppSignInManager _appSignInManger;
        private readonly IAppUserManager _userManager;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleService _roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenFactoryService _tokenFactoryService;
        public PasswordLoginService(IAppSignInManager appSignInManger, IAppUserManager userManager, ITokenStoreService tokenStoreService,
                                    IAuthenticationService authenticationService, IRoleService roleService, IHttpContextAccessor httpContextAccessor,
                                    ITokenFactoryService tokenFactoryService)
        {
            _appSignInManger = appSignInManger;
            _userManager = userManager;
            _tokenStoreService = tokenStoreService;
            _authenticationService = authenticationService;
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
            _tokenFactoryService = tokenFactoryService;
        }

     
        public async Task<(IdentityResult, JWTTokenData)> RegisterAsync(string firstName,string lastName,string phoneNumber,string userName,
                                                                        string password,string nationalCode)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if (user is not null)
                throw new ConflictException("this email used before by another user");

            user = AppUser.Create(firstName, lastName, userName, phoneNumber, userName, nationalCode);

            var result = await _userManager.CreateAsync(user, password);
            await _roleService.AddUserRoleAsync("Customer", user.UserName);
            if(result.Succeeded)
            {
                var token = await _tokenFactoryService.CreateJwtTokenAsync(user);
                return (IdentityResult.Success, token);
            }
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
