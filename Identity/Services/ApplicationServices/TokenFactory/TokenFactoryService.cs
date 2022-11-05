using Identity.Common.Settings;
using Identity.Models;
using Identity.Services.ApplicationServices.Security;
using Identity.Services.IdentityServices.UserManagement;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services.ApplicationServices.TokenFactory;

public class TokenFactoryService : ITokenFactoryService
{
    private readonly IOptionsSnapshot<BearerToken> _configuration;
    private readonly IAppUserManager _userManager;

    public TokenFactoryService(IOptionsSnapshot<BearerToken> configuration, IAppUserManager userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<JWTTokenData> CreateJwtTokenAsync(AppUser user)
    {
        var (accessToken, claims) = await CreateAccessTokenAsync(user);

        var refreshToken = await CreateRefreshTokenAsync();

        return new JWTTokenData()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Claims = claims
        };
    }

    private async Task<(string, IEnumerable<Claim>)> CreateAccessTokenAsync(AppUser user)
    {
        try
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.Value.Key));

            var userClaims = new List<Claim>
                {
                    // User Id Claim
                    new Claim(JwtRegisteredClaimNames.NameId, user.Guid.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),

                    // UserName Claim
                    new Claim(JwtRegisteredClaimNames.Name , user.UserName, _configuration.Value.Issuer),
                    //// Token Id Claim
                    new Claim(JwtRegisteredClaimNames.Jti, SecurityService.CreateSecureGuid().ToString(),
                          ClaimValueTypes.String,_configuration.Value.Issuer),
                };

            var userRoleList = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoleList)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, userRole, ClaimValueTypes.String, _configuration.Value.Issuer));
            }

            var now = DateTime.UtcNow;

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration.Value.Issuer,
                Audience = _configuration.Value.Audience,
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
                Subject = new ClaimsIdentity(userClaims),
                SigningCredentials = new SigningCredentials(authSignInKey, SecurityAlgorithms.Sha256),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToke = tokenHandler.CreateToken(descriptor);
            string encryptedJwtToken = tokenHandler.WriteToken(securityToke);

            return (encryptedJwtToken, userClaims);
        }
        catch (Exception)
        {

            throw;
        }
    }

    private Task<string> CreateRefreshTokenAsync()
    {
        var claims = new List<Claim>()
            {
                // Id
                new Claim(JwtRegisteredClaimNames.Jti, SecurityService.CreateSecureGuid().ToString(),
                ClaimValueTypes.String,_configuration.Value.Issuer),

                //Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer,
                            ClaimValueTypes.String, _configuration.Value.Issuer),

                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64, _configuration.Value.Issuer),
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));

        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _configuration.Value.Issuer,
            audience: _configuration.Value.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.Sha256));

        var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(refreshTokenValue);
    }
}
