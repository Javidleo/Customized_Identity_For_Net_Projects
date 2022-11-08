using Identity.Common;
using Identity.Common.Exceptions;
using Identity.Models;
using Identity.Services.ApplicationServices.TokenStore;
using Identity.Services.IdentityServices.UserManagement;
using Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace Identity.Services.ApplicationServices.User
{
    public class UserService : IUserService
    {
        private readonly IAppUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenStoreService _tokenStoreService;
        private DbSet<AppUser> _users;
        public UserService(IAppUserManager appUserManager, ITokenStoreService tokenStoreService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = appUserManager;
            _tokenStoreService = tokenStoreService;
            _httpContextAccessor = httpContextAccessor;
            _users = _userManager.UserDbSetGenerator();
        }

        public async Task<string> VerificationTokenGeneratorAsync(AppUser user, string phoneNumber)
        {
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);

            if (token is null)
                throw new NotAcceptableException("null token");

            return token;
        }

        public async Task LogOut(string? refreshToken)
        {
            var claimIdentity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;

            string guid = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Regex.IsMatch(guid, CustomRegex.Guid))
                throw new NotAcceptableException("invlid userId");

            var userGuid = new Guid(guid);

            if (userGuid == Guid.Empty && string.IsNullOrWhiteSpace(refreshToken))
                throw new NotAcceptableException("invalid request");

            await _tokenStoreService.RevokeUserTokens(userGuid, refreshToken);
        }



        public async Task<(bool, AppUser?)> VerifyPhoneNumberAsync(string phoneNumber, string code)
        {
            var user = await _userManager.FindByPhoneNumberAsync(phoneNumber);
            if (user is null)
                throw new NotFoundException("user notfound");

            var verified = await _userManager.VerifyChangePhoneNumberTokenAsync(user, code, phoneNumber);
            if (!verified)
                return (false, null);

            if (user.PhoneNumberConfirmed == false) // make it verified 
            {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            return (true, user);
        }

        public async Task<List<UserListViewModel>> GetAllUsers()
        {
            var userList = await _users.Select(i => new UserListViewModel
            {
                FirstName = i.FirstName,
                LastName = i.LastName,
                PhoneNumber = i.PhoneNumber,
                PhoneNumberConfirmed = i.PhoneNumberConfirmed,
                TwoFactorAuthenticationEnabled = i.TwoFactorEnabled
            }).AsNoTracking().ToListAsync();

            return userList;
        }

        public async Task<UserInfoViewModel> GetUserInfo(Guid userGuid)
        {
            var user = await _users.Where(i => i.Guid == userGuid).Select(i => new UserInfoViewModel
            {
                FirstName = i.FirstName,
                LastName = i.LastName,
                UserName = i.UserName,
                PhoneNumberVerified = i.PhoneNumberConfirmed,
                RequireTwoFactorAuthentication = i.TwoFactorEnabled,
                IsLockedOut = i.LockoutEnabled
            }).FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundException("user notfound");

            return user;
        }
    }
}
