using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.ApplicationServices.UserPasswordLogin
{
    public interface IPasswordLoginService
    {

        Task<JWTTokenData> LoginAsync(string userName, string password);
        Task LogOutAsync(string? refreshToken);
        Task<(IdentityResult, JWTTokenData)> RegisterAsync(string firstName, string lastName, string phoneNumber, string userName,
                                                                        string password, string nationalCode);
    }
}