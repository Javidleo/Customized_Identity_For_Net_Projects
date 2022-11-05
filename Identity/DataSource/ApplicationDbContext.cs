using Identity.DataSource.Mapping;
using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataSource
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,
        AppRole,
        int,
        AppUserClaim,
        AppUserRole,
        AppUserLogin,
        AppRoleClaim,
        AppUserTokens>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserMapping());
            builder.ApplyConfiguration(new AppUserRoleMapping());
            builder.ApplyConfiguration(new UserJWTTokenMapping());
            builder.ApplyConfiguration(new AppRoleClaimMapping());
            builder.ApplyConfiguration(new AppRoleMapping());
            builder.ApplyConfiguration(new AppUserClaimMapping());
            builder.ApplyConfiguration(new AppUserLoginMapping());
            base.OnModelCreating(builder);

        }

        public DbSet<UserJWTToken> UserJWTToken { get; set; }
    }
}
