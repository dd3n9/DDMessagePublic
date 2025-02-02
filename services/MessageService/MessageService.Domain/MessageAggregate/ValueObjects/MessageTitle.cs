using MessageService.Domain.Common.Exceptions.DomainExceptions;

namespace MessageService.Domain.MessageAggregate.ValueObjects
{
    public record MessageTitle
    {
        public string Value { get; }

        public MessageTitle(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new EmptyMessageTitleException();

            Value = value;
        }

        public static implicit operator MessageTitle(string value)
            => new(value);

        public static implicit operator string(MessageTitle messageTitle)
            => messageTitle.Value;
    }
}
