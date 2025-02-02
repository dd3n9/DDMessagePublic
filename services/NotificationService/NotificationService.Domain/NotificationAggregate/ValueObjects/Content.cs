using NotificationService.Domain.Common.Exceptions.DomainExceptions;

namespace NotificationService.Domain.NotificationAggregate.ValueObjects
{
    public record Content
    {
        private const int MaxLength = 2056;
        public string Value { get; }

        public Content(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new EmptyContentException();

            if (value.Length > MaxLength)
                throw new ContentLengthLimitException(MaxLength);

            Value = value;
        }

        public static implicit operator string(Content content)
            => content.Value;

        public static implicit operator Content(string content)
            => new(content);
    }
}
