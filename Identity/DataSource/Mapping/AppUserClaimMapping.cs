using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataSource.Mapping
{
    public class AppUserClaimMapping : IEntityTypeConfiguration<AppUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            builder.ToTable("AspNetUserClaim", "scrty");

            builder.Property(i => i.ClaimType).HasMaxLength(200);
            builder.HasOne(i => i.User)
                .WithMany(i => i.Claims)
                .HasForeignKey(i => i.UserId);
        }
    }
}
