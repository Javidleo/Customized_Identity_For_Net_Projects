using Identity.DataSource;
using Identity.Models;
using Identity.Services.IdentityServices.RoleManagement;
using Identity.Services.IdentityServices.RoleStore;
using Identity.Services.IdentityServices.SignInManagement;
using Identity.Services.IdentityServices.UserManagement;
using Identity.Services.IdentityServices.UserStore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.DependencyInjection
{
    public static class IdentityServicesDI
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<IAppRoleManager, AppRoleManager>();
            services.AddScoped<IAppSignInManager, AppSignInManager>();


            // Custom Role Store
            services.AddScoped<RoleStore<AppRole, ApplicationDbContext, int, AppUserRole, AppRoleClaim>, AppRoleStore>();

            // Custom User Store
            services.AddScoped<UserStore<AppUser, AppRole, ApplicationDbContext, int, AppUserClaim, AppUserRole, AppUserLogin,
                                            AppUserTokens, AppRoleClaim>>();

            // Custom UserManager
            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<UserManager<AppUser>, AppUserManager>();

            // Custom RoleManager
            services.AddScoped<IAppRoleManager, AppRoleManager>();
            services.AddScoped<RoleManager<AppRole>, AppRoleManager>();

            // Custom SignIn SignInManager
            services.AddScoped<IAppSignInManager, AppSignInManager>();
            services.AddScoped<SignInManager<AppUser>, AppSignInManager>();


            // adding Microsoft.AspNetCore.Identity Service
            services.AddIdentity<AppUser, AppRole>(options =>
            {

            }).AddUserStore<AppUserStore>()
            .AddUserManager<AppUserManager>()
            .AddRoleStore<AppRoleStore>()
            .AddRoleManager<AppRoleManager>()
            .AddSignInManager<AppSignInManager>()
            .AddDefaultTokenProviders();


            return services;
        }
    }
}
