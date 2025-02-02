using FluentResults;
using SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects;

namespace SchedulerService.Domain.Common.Execeptions
{
    public class ApplicationErrors
    {
        public static class ScheduledTask
        {
            public static Error InvalidNewScheduleStatus(ScheduleStatus scheduleStatus, ScheduleStatus newStatus)
                => new Error($"Cannot transition from {scheduleStatus} to {newStatus}.")
                    .WithMetadata("ErrorCode", "ScheduledTask.InvalidNewScheduleStatus");

        }
    }
}
