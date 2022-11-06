using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataSource.Mapping
{
    public class AppUserLoginMapping : IEntityTypeConfiguration<AppUserLogin>
    {
        public void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            builder.Property(i => i.LoginProvider).HasMaxLength(50);
            builder.Property(i => i.ProviderDisplayName).HasMaxLength(250);

        }
    }
}
