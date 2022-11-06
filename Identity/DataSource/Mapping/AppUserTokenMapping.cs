using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataSource.Mapping
{
    public class AppUserTokenMapping : IEntityTypeConfiguration<AppUserTokens>
    {
        public void Configure(EntityTypeBuilder<AppUserTokens> builder)
        {

            builder.Property(i => i.LoginProvider).HasMaxLength(50);

        }
    }
}
