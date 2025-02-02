using NotificationService.Domain.NotificationAggregate.ValueObjects;

namespace NotificationService.Application.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(MessageId externalMessageId);
    }
}
