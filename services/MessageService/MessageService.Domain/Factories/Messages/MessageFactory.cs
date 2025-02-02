using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.Entities;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Domain.Factories.Messages
{
    public sealed class MessageFactory : IMessageFactory
    {
        public async Task<Message> CreateAsync(MessageTitle messageTitle, Content content, DeliveryDate deliveryDate, RecipientEmail ownerEmail, IEnumerable<RecipientEmail> recipientEmails, Func<RecipientEmail, Task<ExternalUserId?>> resolveUserIdAsync)
        {
            var message = Message.Create(messageTitle, content, deliveryDate);

            var senderUserId = await resolveUserIdAsync(ownerEmail.Value);
            var sender = Recipient.Create(senderUserId, ownerEmail, RecipientType.Sender);
            message.AddRecipient(sender);

            foreach(var email in recipientEmails)
            {
                var recipientUserId = await resolveUserIdAsync(email.Value);
                var recipient = Recipient.Create(recipientUserId, email, RecipientType.Recipient);
                message.AddRecipient(recipient);    
            }

            message.PublishMessageCreatedEvent(senderUserId);

            return message;
        }
    }
}
