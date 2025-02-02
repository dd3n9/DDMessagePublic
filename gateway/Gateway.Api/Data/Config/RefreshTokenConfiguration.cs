using Gateway.Api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Data.Config
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(rt => rt.JwtId)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(rt => rt.AddedDate)
                .IsRequired();

            builder.Property(rt => rt.ExpiryDate)
                .IsRequired();

            builder.HasIndex(rt => rt.Token)
                .IsUnique();
        }
    }
}
