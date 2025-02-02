using MessageService.Domain.Common.Exceptions.DomainExceptions;

namespace MessageService.Domain.MessageAggregate.ValueObjects
{
    public record ExternalUserId
    {
        public Guid Value { get; }

        private ExternalUserId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new EmptyIdValueException();
            }

            Value = value;
        }

        public static ExternalUserId Create(Guid value)
        {
            return new ExternalUserId(value);
        }

        public static ExternalUserId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static implicit operator Guid(ExternalUserId value)
            => value.Value;

        public static implicit operator ExternalUserId(Guid value)
            => new ExternalUserId(value);
    }
}
