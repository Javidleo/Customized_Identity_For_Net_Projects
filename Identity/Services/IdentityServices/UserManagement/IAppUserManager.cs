using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.Services.IdentityServices.UserManagement
{
    public interface IAppUserManager
    {
        #region Base Methods
        IPasswordHasher<AppUser> PasswordHasher { get; set; }
        Task<AppUser> FindByIdAsync(int userId);
        Task<AppUser> FindByIdAsync(Guid userGuid);
        Task<AppUser> FindByEmailAsync(string email);
        Task<AppUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo login);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<IdentityResult> CreateAsync(AppUser user);
        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<AppUser> FindByNameAsync(string name);
        Task<IdentityResult> SetLockoutEnabledAsync(AppUser adminUser, bool enabled);
        Task<AppUser> GetUserAsync(ClaimsPrincipal user);
        Task<IdentityResult> SetEmailAsync(AppUser user, string email);
        Task<string> GetSecurityStampAsync(AppUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(AppUser user);
        Task<IdentityResult> SetUserNameAsync(AppUser user, string userName);
        Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<string> GenerateChangePhoneNumberTokenAsync(AppUser user, string phoneNumber);
        Task<bool> VerifyChangePhoneNumberTokenAsync(AppUser user, string token, string phoneNumber);
        Task<IdentityResult> ChangePhoneNumberAsync(AppUser user, string phoneNumber, string token);
        Task<bool> IsPhoneNumberConfirmedAsync(AppUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token);
        Task<bool> IsEmailConfirmedAsync(AppUser user);
        Task<string> GenerateChangeEmailTokenAsync(AppUser user, string newEmail);
        Task<IdentityResult> ChangeEmailAsync(AppUser user, string newEmail, string token);
        #endregion
        public DbSet<AppUser> UserDbSetGenerator();
        Task<AppUser> FindByPhoneNumberAsync(string phoneNumber);
        Task<AppUser> FindByPhoneNumberAsync(string phoneNumber, bool verified);
        AppUser FindByPhoneNumber(string phoneNumber, bool verified);
        Task<bool> PhoneNumberExistAsync(string phoneNumber, bool verified);
        Task<IdentityResult> AddUserRoleAsync(int roleId, int userId, DateTime fromDate, DateTime? toDate);
        Task<bool> UserExistAsync(int userId);

    }
}
