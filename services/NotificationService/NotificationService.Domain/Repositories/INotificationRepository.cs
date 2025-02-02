using NotificationService.Domain.NotificationAggregate;
using NotificationService.Domain.NotificationAggregate.ValueObjects;

namespace NotificationService.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification?> GetByIdAsync(NotificationId notificationId);
        Task<Notification?> GetByMessageIdAsync(MessageId externalMessageId); 
        Task<bool> ExistsByMessageId(MessageId externalMessageId);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Notification notification);   
    }
}
