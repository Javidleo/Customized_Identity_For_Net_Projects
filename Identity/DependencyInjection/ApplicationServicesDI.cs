using Identity.DataSource;
using Identity.Repository;
using Identity.Services.ApplicationServices.Authentication;
using Identity.Services.ApplicationServices.ExternalProviderLogin;
using Identity.Services.ApplicationServices.Role;
using Identity.Services.ApplicationServices.Security;
using Identity.Services.ApplicationServices.TokenFactory;
using Identity.Services.ApplicationServices.TokenStore;
using Identity.Services.ApplicationServices.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.DependencyInjection
{
    public static class ApplicationServicesDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ITokenFactoryService, TokenFactoryService>();
            services.AddTransient<ITokenStoreService, TokenStoreService>();

            services.AddTransient<IExternalProviderLoginService, ExternalProviderLoginService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<SecurityService>();

            services.AddTransient<UserJwtTokenRepository>();


            // DbContext Connection 
            services.AddDbContext<ApplicationDbContext>(options =>
                                  options.UseSqlServer(configuration.GetConnectionString("Default")));

            return services;
        }
    }
}
