using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataSource.Mapping
{
    public class AppUserMapping : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AspNetUser", "scrty");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Guid).ValueGeneratedOnAdd();
            builder.Property(i => i.FirstName).IsRequired(false).HasMaxLength(100);
            builder.Property(i => i.LastName).IsRequired(false).HasMaxLength(100);
            builder.Property(i => i.NationalCode).IsRequired(false).HasMaxLength(10);
            builder.Property(i => i.Email).IsRequired(false).HasMaxLength(250);
            builder.Property(i => i.NormalizedEmail).IsRequired(false).HasMaxLength(250);
            builder.Property(i => i.PersianBirthDate).IsRequired(false).HasMaxLength(10);
            builder.Property(i => i.PasswordHash).HasMaxLength(256);
            builder.Property(i => i.SecurityStamp).HasMaxLength(50);
            builder.Property(i => i.ConcurrencyStamp).HasMaxLength(50);
            builder.Property(i => i.PhoneNumber).HasMaxLength(11);
            builder.Property(i => i.LockoutEnd).IsRequired(false);
            builder.Property(i => i.BirthDate).IsRequired(false);
            builder.Property(i => i.Gender).IsRequired(false);
            builder.HasIndex(i => i.Guid).IsUnique();

        }
    }
}
