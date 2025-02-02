using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Domain.Factories.Messages
{
    public interface IMessageFactory
    {
        Task<Message> CreateAsync(
            MessageTitle messageTitle,
            Content content, 
            DeliveryDate deliveryDate,
            RecipientEmail ownerEmail,
            IEnumerable<RecipientEmail> recipientEmails,
            Func<RecipientEmail, Task<ExternalUserId?>> resolveUserIdAsync);
    }
}
