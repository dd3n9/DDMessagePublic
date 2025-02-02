using SchedulerService.Domain.Common.Models;
using SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects;

namespace SchedulerService.Domain.ScheduledTaskAggregate.Events
{
    public record ScheduledTaskIsRunning(MessageId externalMessageId) : IDomainEvent;
}
