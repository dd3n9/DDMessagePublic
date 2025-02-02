using FluentAssertions;
using MessageService.Domain.Common.Exceptions.DomainExceptions;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Tests.Domain.ValueObjects
{
    public class RecipientEmailTests
    {
        [Fact]
        public void Constructor_ValidEmail_CreateRecipientEmail()
        {
            //Arrange
            var email = "test@example.com";

            //Act 
            var recipientEmail = new RecipientEmail(email);

            //Assert
            recipientEmail.Value.Should().Be(email);
        }

        [Fact]
        public void Constructor_InvalidEmailFormat_ThrowsException()
        {
            //Arrange
            var invalidEmail = "invalid-email";

            //Act
            Action act = () => new RecipientEmail(invalidEmail);

            //Assert
            act.Should().Throw<InvalidEmailException>();
        }

        [Fact]
        public void Equality_SameEmail_AreEqual()
        {
            // Arrange
            var email = "test@example.com";
            var recipientEmail1 = new RecipientEmail(email);
            var recipientEmail2 = new RecipientEmail(email);

            // Act & Assert
            recipientEmail1.Should().Be(recipientEmail2);
        }

        [Fact]
        public void Inequality_DifferentEmail_AreNotEqual()
        {
            // Arrange
            var email1 = "test1@example.com";
            var email2 = "test2@example.com";
            var recipientEmail1 = new RecipientEmail(email1);
            var recipientEmail2 = new RecipientEmail(email2);

            // Act & Assert
            recipientEmail1.Should().NotBe(recipientEmail2);
        }
    }
}
