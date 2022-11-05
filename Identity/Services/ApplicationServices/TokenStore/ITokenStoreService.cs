using Identity.Models;

namespace Identity.Services.ApplicationServices.TokenStore
{
    public interface ITokenStoreService
    {
        Task AddUserTokenAsync(AppUser user, string refreshToken);
        Task AddUserTokenAsync(UserJWTToken token);
        Task<UserJWTToken> FindByRefreshTokenAsync(string refreshToken);
        Task RevokeUserTokens(Guid? userGuid, string? refreshToken);
    }
}