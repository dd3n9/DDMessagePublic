using FluentResults;
using NotificationService.Domain.Common.Exceptions;
using NotificationService.Domain.Common.Models;
using NotificationService.Domain.NotificationAggregate.ValueObjects;

namespace NotificationService.Domain.NotificationAggregate.Entities
{
    public class Recipient : Entity<RecipientId>
    {
        public RecipientEmail RecipientEmail { get; private set; }
        public DeliveryStatus DeliveryStatus { get; private set; }

        private Recipient(RecipientId recipientId,
            RecipientEmail recipientEmail
            ) : base(recipientId)
        {
            RecipientEmail = recipientEmail;
            DeliveryStatus = DeliveryStatus.Pending;
        }

        private Recipient() { }

        internal static Recipient Create(RecipientEmail recipientEmail)
        {
            var recipient = new Recipient(
                RecipientId.CreateUnique(),
                recipientEmail);

            return recipient;
        }

        internal Result UpdateDeliveryStatus(DeliveryStatus newStatus)
        {
            if ((int)DeliveryStatus > (int)DeliveryStatus.Sent ||
                (int)newStatus <= (int)DeliveryStatus)
                return Result.Fail(ApplicationErrors.Recipient.InvalidNewDeliveryStatus(DeliveryStatus, newStatus));

            DeliveryStatus = newStatus;
            return Result.Ok();
        }
    }
}
