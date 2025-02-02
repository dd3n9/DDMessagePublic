using FluentResults;
using NotificationService.Domain.Common.Exceptions;
using NotificationService.Domain.Common.Exceptions.DomainExceptions;
using NotificationService.Domain.Common.Models;
using NotificationService.Domain.NotificationAggregate.Entities;
using NotificationService.Domain.NotificationAggregate.ValueObjects;

namespace NotificationService.Domain.NotificationAggregate
{
    public class Notification : AggregateRoot<NotificationId>
    {
        public Content Content { get; private set; }
        public DeliveryDate DeliveryDate { get; private set; }
        public MessageId ExternalMessageId { get; private set; }

        private readonly List<Recipient> _recipients = new();
        public IReadOnlyList<Recipient> Recipients => _recipients.AsReadOnly();

        private const int MinimumDelayInSeconds = 60;
        private Notification(NotificationId notificationId,
            Content content, 
            DeliveryDate deliveryDate, 
            MessageId externalMessageId, 
            List<Recipient> recipients) : base(notificationId)
        {
            var curTime = DateTime.UtcNow;

            if (deliveryDate < curTime.AddSeconds(MinimumDelayInSeconds))
            {
                throw new InvalidDeliveryDateException();
            }

            Content = content;
            DeliveryDate = deliveryDate;
            ExternalMessageId = externalMessageId;
            _recipients = recipients;
        }

        private Notification() { }

        public static Notification Create(Content content,
            DeliveryDate deliveryDate,
            MessageId externalMessageId,
            List<Recipient> recipients)
        {
            var notification = new Notification(NotificationId.CreateUnique(),
                content,
                deliveryDate,
                externalMessageId,
                recipients);

            return notification;
        }

        public static Notification Create(Content content,
            DeliveryDate deliveryDate,
            MessageId externalMessageId,
            IEnumerable<RecipientEmail> recipientEmails) 
        {
            var recipients = recipientEmails
                .Select(Recipient.Create)
                .ToList(); 

            var notification = new Notification(NotificationId.CreateUnique(),
                content,
                deliveryDate,
                externalMessageId,
                recipients);

            return notification;
        }

        public Result AddRecipient(RecipientEmail recipientEmail)
        {
            if (_recipients.Any(r => r.RecipientEmail == recipientEmail))
                return Result.Fail(ApplicationErrors.Recipient.AlreadyExists);

            _recipients.Add(Recipient.Create(recipientEmail));
            return Result.Ok();
        }

        public Result UpdateDelivaryStatus(RecipientId recipientId ,DeliveryStatus deliveryStatus)
        {
            var recipient = _recipients.SingleOrDefault(r => r.Id == recipientId);
            if (recipient is null)
                return Result.Fail(ApplicationErrors.Recipient.NotFound);

            return recipient.UpdateDeliveryStatus(deliveryStatus);
        }
    }
}
