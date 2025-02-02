using FluentAssertions;
using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.Entities;
using MessageService.Domain.MessageAggregate.Events;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Tests.Domain.Aggregates
{
    public class MessagePublishMessageCreatedEventTests
    {
        [Fact]
        public void PublishMessageCreatedEvent_RecipientExists_AddMessageCreatedDomainEventWithCorrectData()
        {
            //Arrange
            var externalOwnerId = ExternalUserId.CreateUnique();
            var deliveryDate = new DeliveryDate(DateTime.UtcNow.AddSeconds(65));
            var message = Message.Create(new MessageTitle("Title"), new Content("Content"), deliveryDate);
            var recipient1 = Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("recipient1@example.com"), RecipientType.Recipient);
            var recipient2 = Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("recipient2@example.com"), RecipientType.Recipient);
            var sender = Recipient.Create(externalOwnerId, new RecipientEmail("sender@example.com"), RecipientType.Sender);

            message.AddRecipient(recipient1);
            message.AddRecipient(recipient2);
            message.AddRecipient(sender);

            //Act
            message.PublishMessageCreatedEvent(externalOwnerId);

            //Assert
            message.DomainEvents.Should().HaveCount(1);
            var @event = message.DomainEvents.Single() as MessageCreatedDomainEvent;
            @event.Should().NotBeNull();
            @event.MessageId.Value.Should().Be(message.Id.Value);
            @event.Content.Value.Should().Be(message.Content.Value);
            @event.DeliveryDate.Value.Should().Be(message.DeliveryDate.Value);
            @event.ExternalOwnerId.Value.Should().Be(externalOwnerId.Value);
            @event.RecipientEmails.Should().BeEquivalentTo(new List<RecipientEmail> { "recipient1@example.com", "recipient2@example.com" });
        }

        [Fact]
        public void PublishMessageCreatedEvent_NoRecipientsOfTypeRecipient_AddsMessageCreatedDomainEventWithEmptyRecipientList()
        {
            //Arrange 
            var externalOwnerId = ExternalUserId.CreateUnique();
            var deliveryDate = new DeliveryDate(DateTime.UtcNow.AddSeconds(65));
            var message = Message.Create(new MessageTitle("Title"), new Content("Content"), deliveryDate);
            var sender = Recipient.Create(externalOwnerId, new RecipientEmail("sender@example.com"), RecipientType.Sender);
            message.AddRecipient(sender);

            // Act
            message.PublishMessageCreatedEvent(externalOwnerId);

            // Assert
            message.DomainEvents.Should().HaveCount(1);
            var @event = message.DomainEvents.Single() as MessageCreatedDomainEvent;
            @event.Should().NotBeNull();
            @event.RecipientEmails.Should().BeEmpty();
        }
    }
}
