using FluentResults;
using NotificationService.Domain.NotificationAggregate.ValueObjects;

namespace NotificationService.Domain.Common.Exceptions
{
    public class ApplicationErrors
    {
        public static class Recipient
        {
            public static readonly Error NotFound = new Error("Recipient was not found.")
                .WithMetadata("ErrorCode", "Recipient.NotFound");
            public static readonly Error AlreadyExists = new Error("Recipient already exists.")
               .WithMetadata("ErrorCode", "Recipient.AlreadyExists");
            public static Error InvalidNewDeliveryStatus(DeliveryStatus currentStatus, DeliveryStatus newStatus)
                => new Error($"Cannot transition from {currentStatus} to {newStatus}.")
                    .WithMetadata("ErrorCode", "Recipient.InvalidNewDeliveryStatus");
        }
    }
}
