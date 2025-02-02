using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain.NotificationAggregate;
using NotificationService.Domain.NotificationAggregate.ValueObjects;

namespace NotificationService.Infrastructure.EF.Config
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(x => x.Id);

            builder
                .Property(n => n.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, id => NotificationId.Create(id));

            builder
                .Property(n => n.Content)
                .IsRequired()
                .HasConversion(c => c.Value, c => new Content(c));

            builder
                .Property(n => n.DeliveryDate)
                .IsRequired()
                .HasConversion(c => c.Value, c => new DeliveryDate(c));

            builder
                .Property(n => n.ExternalMessageId)
                .IsRequired()
                .HasConversion(c => c.Value, c => MessageId.Create(c));

            builder.OwnsMany(n => n.Recipients, rb =>
            {
                rb.ToTable("Recipients");

                rb.HasKey(r => r.Id);

                rb.Property(r => r.Id)
                    .ValueGeneratedNever()
                    .HasConversion(id => id.Value, id => RecipientId.Create(id));

                rb.Property(r => r.RecipientEmail)
                    .IsRequired()
                    .HasConversion(rm => rm.Value, rm => new RecipientEmail(rm));

                rb.Property(r => r.DeliveryStatus)
                    .IsRequired()
                    .HasConversion(
                        status => status.ToString(),
                        status => (DeliveryStatus)Enum.Parse(typeof(DeliveryStatus), status));
            });
        }
    }
}
