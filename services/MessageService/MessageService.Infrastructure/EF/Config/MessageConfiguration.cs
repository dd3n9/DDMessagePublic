using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessageService.Infrastructure.EF.Config
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(x => x.Id);

            builder
                .Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, id => MessageId.Create(id));

            builder
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(256)
                .HasConversion(t => t.Value, t => new MessageTitle(t));

            builder
                .Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(2056)
                .HasConversion(c => c.Value, c => new Content(c));

            builder
                .Property(m => m.DeliveryDate)
                .IsRequired()
                .HasConversion(dd => dd.Value, dd => new DeliveryDate(dd));

            builder
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();


            builder.OwnsMany(m => m.Recipients, rb =>
            {
                rb.ToTable("Recipients");

                rb.HasKey(r => r.Id);

                rb.Property(r => r.Id)
                    .ValueGeneratedNever()
                    .HasConversion(id => id.Value, id => RecipientId.Create(id));

                rb.Property(r => r.ExternalUserId)
                    .HasConversion(id => id.Value, id => ExternalUserId.Create(id));

                rb.Property(r => r.RecipientEmail)
                    .IsRequired()
                    .HasConversion(email => email.Value, email => new RecipientEmail(email));

                rb.Property(r => r.RecipientType)
                    .IsRequired()
                    .HasConversion(
                        type => type.ToString(),
                        type => (RecipientType)Enum.Parse(typeof(RecipientType), type)
                    );

                rb.Property(r => r.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
