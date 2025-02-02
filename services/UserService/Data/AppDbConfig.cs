using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using UserService.Models;

namespace UserService.Data
{
    public class AppDbConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder
               .Property(u => u.UserName)
               .HasMaxLength(64)
               .IsRequired(); 

            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .Property(u => u.HashedPassword)
                .IsRequired();
        }
    }
}
