using MessageService.Domain.Common.Models;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Domain.MessageAggregate.Events
{
    public record MessageCreatedDomainEvent(
        MessageId MessageId,
        Content Content,
        DeliveryDate DeliveryDate,
        ExternalUserId ExternalOwnerId,
        IEnumerable<RecipientEmail> RecipientEmails
    ) : IDomainEvent;
}
