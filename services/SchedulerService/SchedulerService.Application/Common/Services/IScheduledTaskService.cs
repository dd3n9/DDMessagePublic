using SchedulerService.Domain.ScheduledTaskAggregate;

namespace SchedulerService.Application.Common.Services
{
    public interface IScheduledTaskService
    {
        Task<IEnumerable<ScheduledTask>> GetTasksByScheduledTimeAsync(DateTime scheduledTime);
    }
}
