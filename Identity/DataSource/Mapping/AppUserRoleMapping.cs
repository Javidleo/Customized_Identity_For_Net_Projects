using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataSource.Mapping
{
    public class AppUserRoleMapping : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("AspNetUserRole", "scrty");

            builder.HasKey(i => new { i.UserId, i.RoleId, i.FromDate });

            builder
                .HasOne(i => i.Role)
                .WithMany(i => i.UserRoles)
                .HasForeignKey(i => i.RoleId);

            builder.HasOne(i => i.User)
                   .WithMany(i => i.UserRoles)
                   .HasForeignKey(i => i.UserId);
        }
    }
}
