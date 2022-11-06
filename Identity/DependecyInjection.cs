using Identity.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddIdentity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServices();

            services.AddApplicationServices(configuration);

            services.AddTokenServices(configuration);

            return services;
        }

    }
}