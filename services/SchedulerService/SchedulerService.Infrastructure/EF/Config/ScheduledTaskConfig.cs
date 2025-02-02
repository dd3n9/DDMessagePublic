using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchedulerService.Domain.ScheduledTaskAggregate;
using SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects;

namespace SchedulerService.Infrastructure.EF.Config
{
    public class ScheduledTaskConfig : IEntityTypeConfiguration<ScheduledTask>
    {
        public void Configure(EntityTypeBuilder<ScheduledTask> builder)
        {
            builder.ToTable("ScheduledTask");
            builder.HasKey(st => st.Id);

            builder
                .Property(st => st.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, id => ScheduledId.Create(id));

            builder
                .Property(st => st.ExternalMessageId)
                .IsRequired()
                .HasConversion(id => id.Value, id => MessageId.Create(id));

            builder
                .Property(st => st.ExternalOwnerId)
                .IsRequired()
                .HasConversion(id => id.Value, id => UserId.Create(id));

            builder
                .Property(st => st.ScheduledTime)
                .IsRequired()
                .HasConversion(time => time.Value, time => new ScheduledTime(time));

            builder
                .Property(st => st.ScheduleStatus)
                .IsRequired()
                .HasConversion(
                    status => status.ToString(),
                    status => (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), status)
                );
        }
    }
}
