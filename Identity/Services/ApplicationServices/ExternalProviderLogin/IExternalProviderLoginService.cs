using Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Services.ApplicationServices.ExternalProviderLogin
{
    public interface IExternalProviderLoginService
    {
        ChallengeResult GetExternalProviderProperties(string loginProvider, string url);
        Task<JWTTokenData> ExternalProviderLoginAsync(string loginProvider);
    }
}
