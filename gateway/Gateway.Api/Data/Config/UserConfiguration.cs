using Gateway.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gateway.Api.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder
                .Property(u => u.ExternalUserId);

            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .Property(u => u.HashedPassword)
                .IsRequired();

            builder
               .HasMany(u => u.RefreshTokens)
               .WithOne()
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
