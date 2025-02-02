using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.NotificationAggregate;
using NotificationService.Domain.NotificationAggregate.ValueObjects;
using NotificationService.Domain.Repositories;
using NotificationService.Infrastructure.EF.Context;

namespace NotificationService.Infrastructure.EF.Repositories
{
    internal sealed class NotificationRepository : INotificationRepository
    {
        private readonly DbSet<Notification> _notifications;
        private readonly AppDbContext _appDbContext;

        public NotificationRepository(AppDbContext appDbContext)
        {
            _notifications = appDbContext.Notifications;
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Notification notification)
        {
            await _notifications.AddAsync(notification);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            _notifications.Remove(notification);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByMessageId(MessageId externalMessageId)
        {
            return await _notifications.AnyAsync(n => n.ExternalMessageId == externalMessageId);
        }

        public async Task<Notification?> GetByIdAsync(NotificationId notificationId)
        {
            var result = await _notifications
                .Include(n => n.Recipients)
                .SingleOrDefaultAsync(n => n.Id == notificationId);

            return result;
        }

        public async Task<Notification?> GetByMessageIdAsync(MessageId externalMessageId)
        {
            var result = await _notifications
                .Include(n => n.Recipients)
                .SingleOrDefaultAsync(n => n.ExternalMessageId == externalMessageId);

            return result;
        }

        public async Task UpdateAsync(Notification notification)
        {
            _notifications.Update(notification);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
