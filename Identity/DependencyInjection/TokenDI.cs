using Identity.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity.DependencyInjection
{
    public static class TokenDI
    {
        public static IServiceCollection AddTokenServices(this IServiceCollection services, IConfiguration configuration)
        {


            var tokenValidationParameters = new TokenValidationParameters
            {

                ValidIssuer = configuration["BearerToken:Issuer"],
                ValidAudience = configuration["BearerToken:Audience"],
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["BearerToken:Key"])),
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = false,
            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
            }); // if you want to add authentication with google you need to add the next line of code as an extension method 

            //.AddGoogle(options =>
            // {
            //     options.ClientId = "client id ";
            //     options.ClientSecret = "client secret";
            // });


            services.AddOptions<BearerToken>()
            .Bind(configuration.GetSection("BearerToken"))
            .Validate(bearerToken =>
            {
                return bearerToken.AccessTokenExpirationMinutes < bearerToken.RefreshTokenExpirationMinutes;
            }, "Refresh Token lifetime should be more than access Token lifetime");


            return services;
        }
    }
}
