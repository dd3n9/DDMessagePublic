using SchedulerService.Domain.ScheduledTaskAggregate;
using SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects;
using System.Linq.Expressions;

namespace SchedulerService.Domain.Repositories
{
    public interface IScheduledTaskRepository
    {
        Task<ScheduledTask?> GetByIdAsync(ScheduledId scheduledId);
        Task<bool> ExistsByMessageId(MessageId messageId);
        Task<IEnumerable<ScheduledTask>> GetAllAsync();
        Task<IEnumerable<ScheduledTask>> GetAllAsync(Expression<Func<ScheduledTask, bool>> predicate);
        Task<ScheduleStatus> GetStatusAsync(MessageId externalMessageId);
        Task AddAsync(ScheduledTask scheduledTask);
        Task UpdateAsync(ScheduledTask scheduledTask);
        Task DeleteAsync(ScheduledTask scheduledTask);
    }
}
