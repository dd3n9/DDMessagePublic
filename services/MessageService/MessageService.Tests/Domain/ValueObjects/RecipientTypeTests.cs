using FluentAssertions;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Tests.Domain.ValueObjects
{
    public class RecipientTypeTests
    {
        [Fact]
        public void EnumHasCorrectValues()
        {
            RecipientType.Recipient.Should().Be(RecipientType.Recipient);
            RecipientType.Sender.Should().Be(RecipientType.Sender);
        }
    }
}
