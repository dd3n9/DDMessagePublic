using MessageService.Domain.Common.Exceptions.DomainExceptions;
using MessageService.Domain.Common.Models;
using MessageService.Domain.MessageAggregate.Entities;
using MessageService.Domain.MessageAggregate.Events;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Domain.MessageAggregate
{
    public class Message : AggregateRoot<MessageId>
    {
        public MessageTitle Title { get; set; }
        public Content Content { get; private set; }
        public DeliveryDate DeliveryDate { get; private set; }

        private readonly List<Recipient> _recipients = new();
        
        public IReadOnlyList<Recipient> Recipients => _recipients.AsReadOnly();

        private const int MinimumDelayInSeconds = 60;
        private Message(MessageId messageId,
            MessageTitle title,
            Content content, 
            DeliveryDate deliveryDate) : base(messageId) 
        {
            var curTime = DateTime.UtcNow;

            if (deliveryDate < curTime.AddSeconds(MinimumDelayInSeconds))
            {
                throw new InvalidDeliveryDateException();
            }

            Title = title;
            Content = content;
            DeliveryDate = deliveryDate;
        }

        private Message()
        {

        }

        public static Message Create(
            MessageTitle title,
            Content content,
            DeliveryDate deliveryDate)
        {
            var message = new Message(
                MessageId.CreateUnique(), 
                title,
                content, 
                deliveryDate);

            return message;
        }

        public void AddRecipient(Recipient recipient)
        {
            var isRecipientExists = _recipients.Any(r => r.RecipientEmail == recipient.RecipientEmail);
            if (isRecipientExists) 
                return;

            _recipients.Add(recipient);
        }

        public void AddRecipients(IEnumerable<Recipient> recipients)
        {
            foreach (var recipient in recipients)
            {
                var isRecipientExists = _recipients.Any(r => r.RecipientEmail == recipient.RecipientEmail);
                if (isRecipientExists)
                    continue;

                _recipients.Add(recipient);
            }
        }

        public void PublishMessageCreatedEvent(ExternalUserId externalOwnerId)
        {
            var recipientEmails = _recipients
                .Where(r => r.RecipientType == RecipientType.Recipient)
                .Select(r => r.RecipientEmail)
                .ToList();

            AddEvent(new MessageCreatedDomainEvent(Id.Value, Content.Value, DeliveryDate.Value, externalOwnerId.Value, recipientEmails));
        }
    }
}
