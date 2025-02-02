using NotificationService.Application.Services;
using NotificationService.Contracts.Requests;
using NotificationService.Domain.NotificationAggregate.ValueObjects;
using NotificationService.Domain.Repositories;

namespace NotificationService.Infrastructure.Common.Services
{
    internal sealed class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMailService _mailService;

        public NotificationService(INotificationRepository notificationRepository, IMailService mailService)
        {
            _notificationRepository = notificationRepository;
            _mailService = mailService;
        }

        public async Task SendNotificationAsync(MessageId externalMessageId)
        {
            var notification = await _notificationRepository.GetByMessageIdAsync(externalMessageId);

            if(notification is null)
            {
                Console.WriteLine($"Notification with ExternalMessageId {externalMessageId} not found.");
                return;
            }

            foreach (var recipient in notification.Recipients)
            {
                if (recipient.DeliveryStatus == DeliveryStatus.Pending)
                {
                    var sendEmailRequest = new SendEmailRequest(
                        recipient.RecipientEmail,
                        notification.Content);

                    var sendResult = await _mailService.SendEmailAsync(sendEmailRequest);

                    if (sendResult.IsSuccess)
                    {
                        notification.UpdateDelivaryStatus(recipient.Id, DeliveryStatus.Sent);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to send email to {recipient.RecipientEmail.Value}: {sendResult}");
                        notification.UpdateDelivaryStatus(recipient.Id, DeliveryStatus.Failed);
                    }
                }
            }

            await _notificationRepository.UpdateAsync(notification);
        }
    }
}
