using FluentAssertions;
using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.Entities;
using MessageService.Domain.MessageAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Tests.Domain.Aggregates
{
    public class MessageAddRecipientTests
    {
        [Fact]
        public void AddRecipient_NewRecipient_AddsRecipientToList()
        {
            //Arrange
            var message = Message.Create(new MessageTitle("Title"), new Content("Content"), new DeliveryDate(DateTime.UtcNow.AddSeconds(65)));
            var recipient = Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("test@example.com"), RecipientType.Recipient);

            //Act
            message.AddRecipient(recipient);

            //Assert
            message.Recipients.Should().HaveCount(1);
            message.Recipients.Should().Contain(recipient);
        }

        [Fact]
        public void AddRecipient_ExistingRecipientEmail_DoesNotAddDuplicate()
        {
            //Arrange
            var message = Message.Create(new MessageTitle("Title"), new Content("Content"), new DeliveryDate(DateTime.UtcNow.AddSeconds(65)));
            var recipient1 = Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("test@example.com"), RecipientType.Recipient);
            var recipient2 = Recipient.Create(ExternalUserId.CreateUnique(), new RecipientEmail("test@example.com"), RecipientType.Recipient);

            //Act
            message.AddRecipient(recipient1);
            message.AddRecipient(recipient2);

            //Assert
            message.Recipients.Should().HaveCount(1);
            message.Recipients.Should().Contain(recipient1);
        }
    }
}
