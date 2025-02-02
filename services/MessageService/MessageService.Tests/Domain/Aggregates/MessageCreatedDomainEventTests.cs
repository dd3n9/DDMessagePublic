using FluentAssertions;
using MessageService.Domain.MessageAggregate.Events;
using MessageService.Domain.MessageAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Tests.Domain.Aggregates
{
    public class MessageCreatedDomainEventTests
    {
        [Fact]
        public void Constructor_ValidInput_CreatesEvent()
        {
            //Arrange
            var messageId = MessageId.CreateUnique();
            Content content = "Test content";
            DeliveryDate deliveryDate = DateTime.UtcNow.AddDays(1);
            var externalOwnerId = ExternalUserId.CreateUnique();
            var recipientEmails = new List<RecipientEmail> { "test1@example.com", "test2@example.com" };

            //Act
            var @event = new MessageCreatedDomainEvent(messageId, content, deliveryDate, externalOwnerId, recipientEmails);

            //Assert
            @event.MessageId.Should().Be(messageId);
            @event.Content.Should().Be(content);
            @event.DeliveryDate.Should().Be(deliveryDate);
            @event.ExternalOwnerId.Should().Be(externalOwnerId);
            @event.RecipientEmails.Should().BeEquivalentTo(recipientEmails);
        }
    }
}
