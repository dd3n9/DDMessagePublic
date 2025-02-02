namespace SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects
{
    public enum ScheduleStatus
    {
        Scheduled = 0,    // Task is scheduled
        Running = 1,      // Task is running
        Completed = 2,    // Task was completed successfully
        Failed = 3,       // Task was completed with an error
        Cancelled = 4     // Task canceled
    }
}
