using FluentAssertions;
using MessageService.Domain.Common.Exceptions.DomainExceptions;
using MessageService.Domain.MessageAggregate;
using MessageService.Domain.MessageAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Tests.Domain.Aggregates
{
    public class MessageTests
    {
        [Fact]
        public void Create_ValidInput_CreatesMessage()
        {
            //Arrange
            var title = new MessageTitle("Test Title");
            var content = new Content("Test Content");
            var deliveryDate = new DeliveryDate(DateTime.UtcNow.AddSeconds(65));

            //Act
            var message = Message.Create(title, content, deliveryDate);
                
            //Assert
            message.Should().NotBeNull(); 
            message.Id.Should().NotBeNull();
            message.Title.Should().Be(title);   
            message.Content.Should().Be(content);
            message.DeliveryDate.Should().Be(deliveryDate);
            message.Recipients.Should().BeEmpty();
        }

        [Fact]
        public void Create_DeliveryDateLessThanMinimumDelay_ThrowsInvalidDeliveryDateException()
        {
            //Arrange
            var title = new MessageTitle("Test Title");
            var content = new Content("Test Content");
            var deliveryDate = new DeliveryDate(DateTime.UtcNow.AddSeconds(55));

            //Act 
            Action act = () => Message.Create(title, content, deliveryDate);

            //Assert
            act.Should().Throw<InvalidDeliveryDateException>();
        }
    }
}
