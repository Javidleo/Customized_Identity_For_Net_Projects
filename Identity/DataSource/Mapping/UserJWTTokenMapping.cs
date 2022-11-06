using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataSource.Mapping
{
    public class UserJWTTokenMapping : IEntityTypeConfiguration<UserJWTToken>
    {
        public void Configure(EntityTypeBuilder<UserJWTToken> builder)
        {
            builder.HasKey(i => i.jwt_Id);

            builder.Property(i => i.usr_guid).IsRequired();
            builder.Property(i => i.jwt_RefreshToken).IsRequired();
            builder.Property(i => i.jwt_RefreshTokenExpiresDateTime).IsRequired();
        }
    }
}
