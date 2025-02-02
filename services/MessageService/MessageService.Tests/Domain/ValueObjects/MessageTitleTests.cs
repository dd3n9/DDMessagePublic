using FluentAssertions;
using MessageService.Domain.Common.Exceptions.DomainExceptions;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Tests.Domain.ValueObjects
{
    public class MessageTitleTests
    {
        [Fact]
        public void Constructor_ValidTitle_CreateMessageTitle()
        {
            //Arrange
            var title = "Test title";

            //Act
            var messageTitle = new MessageTitle(title);

            //Assert
            messageTitle.Value.Should().Be(title);
        }

        [Fact]
        public void Constructor_NullOrEmptyTitle_ThrowsException()
        {
            //Arrange 
            string nullTitle = null;
            string emptyTitle = string.Empty;

            //Act
            Action actNull = () => new MessageTitle(nullTitle);
            Action actEmpty = () => new MessageTitle(emptyTitle);

            //Assert
            actNull.Should().Throw<EmptyMessageTitleException>();
            actEmpty.Should().Throw<EmptyMessageTitleException>();
        }

        [Fact]
        public void Equality_SameTitle_AreEqual()
        {
            // Arrange
            var title = "Test title";
            var messageTitle1 = new MessageTitle(title);
            var messageTitle2 = new MessageTitle(title);

            // Act & Assert
            messageTitle1.Should().Be(messageTitle2);
        }

        [Fact]
        public void Inequality_DifferentTitle_AreNotEqual()
        {
            // Arrange
            var title1 = "Test title 1";
            var title2 = "Test title 2";
            var messageTitle1 = new MessageTitle(title1);
            var messageTitle2 = new MessageTitle(title2);

            // Act & Assert
            messageTitle1.Should().NotBe(messageTitle2);
        }
    }
}
