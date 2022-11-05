using Identity.Models;
using Identity.ViewModels;

namespace Identity.Services.ApplicationServices.User
{
    public interface IUserService
    {
        Task<List<UserListViewModel>> GetAllUsers();
        Task<UserInfoViewModel> GetUserInfo(Guid userGuid);
        Task LogOut(string? refreshToken);
        Task<string> VerificationTokenGeneratorAsync(AppUser user, string phoneNumber);
        Task<(bool, AppUser?)> VerifyPhoneNumberAsync(string phoneNumber, string code);
    }
}