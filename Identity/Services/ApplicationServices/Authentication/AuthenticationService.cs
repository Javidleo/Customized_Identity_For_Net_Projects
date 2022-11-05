using Identity.Common.Exceptions;
using Identity.Models;
using Identity.Services.ApplicationServices.TokenFactory;
using Identity.Services.ApplicationServices.TokenStore;
using Identity.Services.IdentityServices.UserManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Identity.Services.ApplicationServices.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAppUserManager _userManager;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IAppUserManager userManager, ITokenFactoryService tokenFactoryService,
                            ITokenStoreService tokenStoreService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenFactoryService = tokenFactoryService;
            _tokenStoreService = tokenStoreService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<JWTTokenData> GenerateToken(AppUser user)
        {
            var result = await _tokenFactoryService.CreateJwtTokenAsync(user);
            if (result is not null)
            {
                await _tokenStoreService.AddUserTokenAsync(user, result.RefreshToken);

                var httpContext = _httpContextAccessor.HttpContext;
                httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(result.Claims, JwtBearerDefaults.AuthenticationScheme));
            }
            return result;
        }

        // this method let you generate new tokens by your valid refreshToken
        public async Task<JWTTokenData> RefreshToken(string refreshToken)
        {
            var token = await _tokenStoreService.FindByRefreshTokenAsync(refreshToken);
            if (token is null)
                throw new SecurityTokenValidationException();

            var userInfo = await _userManager.FindByIdAsync(token.usr_guid);
            if (userInfo is null)
                throw new NotFoundException("user notfound");

            var result = await _tokenFactoryService.CreateJwtTokenAsync(userInfo);
            await _tokenStoreService.AddUserTokenAsync(userInfo, result.RefreshToken);

            var httpContext = _httpContextAccessor.HttpContext;
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(result.Claims, JwtBearerDefaults.AuthenticationScheme));

            return result;
        }
    }
}
