using MessageService.Domain.Common.Models;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Domain.MessageAggregate.Entities
{
    public class Recipient : Entity<RecipientId>
    {
        public ExternalUserId? ExternalUserId { get; private set; }
        public RecipientEmail RecipientEmail { get; private set; }
        public RecipientType RecipientType { get; private set; }

        private Recipient(RecipientId recipientId,
            ExternalUserId? externalUserId,
            RecipientEmail recipientEmail,
            RecipientType recipientType) : base(recipientId)
        {
            ExternalUserId = externalUserId;
            RecipientEmail = recipientEmail;
            RecipientType = recipientType;
        }

        private Recipient() { }

        public static Recipient Create(ExternalUserId? externalUserId,
            RecipientEmail recipientEmail,
            RecipientType recipientType)
        {
            var recipient = new Recipient(
                RecipientId.CreateUnique(),
                externalUserId,
                recipientEmail,
                recipientType);

            return recipient;
        }

    }
}
