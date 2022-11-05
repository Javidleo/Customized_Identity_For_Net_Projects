using Identity.Common.Exceptions;
using Identity.Common.Settings;
using Identity.Models;
using Identity.Repository;
using Microsoft.Extensions.Options;

namespace Identity.Services.ApplicationServices.TokenStore
{
    public class TokenStoreService : ITokenStoreService
    {
        private readonly IOptionsSnapshot<BearerToken> _configuration;
        private readonly UserJwtTokenRepository _userTokenRepository;

        public TokenStoreService(IOptionsSnapshot<BearerToken> configuration, UserJwtTokenRepository userTokenRepository)
        {
            _configuration = configuration;
            _userTokenRepository = userTokenRepository;
        }

        public async Task AddUserTokenAsync(AppUser user, string refreshToken)
        {
            var token = UserJWTToken
                 .Create(user.Guid, refreshToken, DateTime.UtcNow.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes));

            await AddUserTokenAsync(token);
        }

        public async Task AddUserTokenAsync(UserJWTToken token)
        {
            try
            {
                await InvalidateUserTokenByIdAsync(token.usr_guid);

                await _userTokenRepository.AddAsync(token);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task InvalidateUserTokenByIdAsync(Guid usr_guid)
        => await _userTokenRepository.RemoveUserTokensAsync(usr_guid);


        public async Task<UserJWTToken> FindByRefreshTokenAsync(string refreshToken)
        {
            return await _userTokenRepository.FindTokenByRefreshTokenAsync(refreshToken) ?? null;
        }

        public async Task RevokeUserTokens(Guid? userGuid, string? refreshToken)
        {
            if (userGuid == Guid.Empty)
            {
                await InvalidateUserTokenByIdAsync(userGuid.Value);
                return;
            }

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var token = await _userTokenRepository.FindTokenByRefreshTokenAsync(refreshToken);
                if (token is null)
                    throw new NotFoundException("token notfound");

                await InvalidateUserTokenByIdAsync(token.usr_guid);
                return;
            }
            throw new NotAcceptableException("invalid request");
        }
    }
}
