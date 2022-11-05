using Identity.Models;

namespace Identity.Services.ApplicationServices.TokenFactory
{
    public interface ITokenFactoryService
    {
        Task<JWTTokenData> CreateJwtTokenAsync(AppUser user);
    }
}