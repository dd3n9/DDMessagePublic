using FluentAssertions;
using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.Entities;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Tests.Domain.Aggregates
{
    public class MessageAddRecipientsTests
    {
        [Fact]
        public void AddRecipients_MultipleNewRecipients_AddsAllToList()
        {
            //Arrange
            var message = Message.Create(new MessageTitle("Title"), new Content("Content"), new DeliveryDate(DateTime.UtcNow.AddSeconds(65)));
            var recipients = new List<Recipient>
            {
                Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("test1@example.com"), RecipientType.Recipient),
                Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("test2@example.com"), RecipientType.Recipient)
            };

            //Act
            message.AddRecipients(recipients);

            //Assert
            message.Recipients.Should().HaveCount(2);
            message.Recipients.Should().Contain(recipients[0]);
            message.Recipients.Should().Contain(recipients[1]);
        }

        [Fact]
        public void AddRecipients_IncludesExistingRecipientEmail_AddOnlyNew()
        {
            //Arrange
            var message = Message.Create(new MessageTitle("Title"), new Content("Content"), new DeliveryDate(DateTime.UtcNow.AddSeconds(65)));
            var existingRecipient = Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("test1@example.com"), RecipientType.Recipient);
            message.AddRecipient(existingRecipient);

            var newRecipients = new List<Recipient>
            {
                existingRecipient, 
                Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("test2@example.com"), RecipientType.Recipient)
            };

            //Act 
            message.AddRecipients(newRecipients);

            //Assert
            message.Recipients.Should().HaveCount(2);
            message.Recipients.Should().Contain(existingRecipient);
            message.Recipients.Should().Contain(newRecipients[1]);
        }
    }
}
