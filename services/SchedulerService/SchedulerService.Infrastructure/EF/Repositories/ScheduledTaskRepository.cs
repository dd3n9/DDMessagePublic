using Microsoft.EntityFrameworkCore;
using SchedulerService.Domain.Repositories;
using SchedulerService.Domain.ScheduledTaskAggregate;
using SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects;
using SchedulerService.Infrastructure.EF.Context;
using System.Linq.Expressions;

namespace SchedulerService.Infrastructure.EF.Repositories
{
    internal sealed class ScheduledTaskRepository : IScheduledTaskRepository
    {
        private readonly DbSet<ScheduledTask> _scheduledTasks;  
        private readonly AppDbContext _appDbContext;

        public ScheduledTaskRepository(AppDbContext appDbContext)
        {
            _scheduledTasks = appDbContext.ScheduledTasks;
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(ScheduledTask scheduledTask)
        {
            await _scheduledTasks.AddAsync(scheduledTask);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ScheduledTask scheduledTask)
        {
            _scheduledTasks.Remove(scheduledTask);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByMessageId(MessageId externalMessageId)
        {
            return await _scheduledTasks.AnyAsync(task => task.ExternalMessageId == externalMessageId);
        }

        public async Task<IEnumerable<ScheduledTask>> GetAllAsync()
        {
            return await _scheduledTasks.ToListAsync();
        }

        public async Task<IEnumerable<ScheduledTask>> GetAllAsync(Expression<Func<ScheduledTask, bool>> predicate)
        {
            return await _scheduledTasks.Where(predicate).ToListAsync();
        }

        public async Task<ScheduledTask?> GetByIdAsync(ScheduledId scheduledId)
        {
            return await _scheduledTasks.SingleOrDefaultAsync(task => task.Id == scheduledId);
        }

        public async Task<ScheduleStatus> GetStatusAsync(MessageId externalMessageId)
        {
            return await _scheduledTasks
                .Where(s => s.ExternalMessageId == externalMessageId)
                .Select(s => s.ScheduleStatus)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(ScheduledTask scheduledTask)
        {
            _scheduledTasks.Update(scheduledTask);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
