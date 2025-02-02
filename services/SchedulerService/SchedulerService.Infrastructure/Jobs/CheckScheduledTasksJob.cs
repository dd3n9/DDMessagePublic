using Quartz;
using SchedulerService.Application.Common.Services;
using SchedulerService.Domain.Repositories;
using SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects;

namespace SchedulerService.Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class CheckScheduledTasksJob : IJob
    {
        private readonly IScheduledTaskService _scheduledTaskService;
        private readonly IScheduledTaskRepository _schedulerRepository;

        public CheckScheduledTasksJob(IScheduledTaskService scheduledTaskService, IScheduledTaskRepository schedulerRepository)
        {
            _scheduledTaskService = scheduledTaskService;
            _schedulerRepository = schedulerRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var time = DateTime.UtcNow;

            var scheduledTasks = await _scheduledTaskService.GetTasksByScheduledTimeAsync(DateTime.UtcNow);

            foreach (var task in scheduledTasks)
            {
                task.UpdateScheduleStatus(ScheduleStatus.Running);
                await _schedulerRepository.UpdateAsync(task);
            }
        }
    }
}
