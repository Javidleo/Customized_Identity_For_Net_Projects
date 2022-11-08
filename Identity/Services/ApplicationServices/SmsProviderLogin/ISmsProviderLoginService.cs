using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.ApplicationServices.SmsProviderLogin
{
    public interface ISmsProviderLoginService
    {
        Task<(IdentityResult?, string?, bool)> Handle(string phoneNumber);
        Task<JWTTokenData> Login(string phoneNumber, string code);
    }
}
