using SchedulerService.Application.Common.Services;
using SchedulerService.Domain.Repositories;
using SchedulerService.Domain.ScheduledTaskAggregate;

namespace SchedulerService.Infrastructure.Common.Services
{
    internal sealed class ScheduledTaskService : IScheduledTaskService
    {
        private readonly IScheduledTaskRepository _scheduledTaskRepository;

        public ScheduledTaskService(IScheduledTaskRepository scheduledTaskRepository)
        {
            _scheduledTaskRepository = scheduledTaskRepository;
        }

        public async Task<IEnumerable<ScheduledTask>> GetTasksByScheduledTimeAsync(DateTime scheduledTime)
        {
            var normalizedTime = new DateTime(
                scheduledTime.Year, scheduledTime.Month, scheduledTime.Day,
                scheduledTime.Hour, scheduledTime.Minute, 0, DateTimeKind.Utc);

            return (await _scheduledTaskRepository.GetAllAsync())
             .Where(m => m.ScheduledTime.Value == normalizedTime);
        }
    }
}
