using Identity.Models;

namespace Identity.Services.ApplicationServices.Authentication
{
    public interface IAuthenticationService
    {
        Task<JWTTokenData> GenerateToken(AppUser user);
        Task<JWTTokenData> RefreshToken(string refreshToken);
    }
}