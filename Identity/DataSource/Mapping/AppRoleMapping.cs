using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataSource.Mapping
{
    public class AppRoleMapping : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {

            builder.Property(i => i.Description).HasMaxLength(1000);
            builder.Property(i => i.ConcurrencyStamp).HasMaxLength(50);
        }
    }
}
