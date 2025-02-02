using MessageService.Domain.Common.Exceptions.DomainExceptions;

namespace MessageService.Domain.MessageAggregate.ValueObjects
{
    public record RecipientId
    {
        public Guid Value { get; }

        private RecipientId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new EmptyIdValueException();
            }

            Value = value;
        }

        public static RecipientId Create(Guid value)
        {
            return new RecipientId(value);
        }

        public static RecipientId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static implicit operator Guid(RecipientId value)
            => value.Value;

        public static implicit operator RecipientId(Guid value)
            => new RecipientId(value);
    }
}
