using MessageService.Domain.Common.Exceptions;
using MessageService.Domain.Common.Exceptions.DomainExceptions;

namespace MessageService.Domain.MessageAggregate.ValueObjects
{
    public record MessageId
    {
        public Guid Value { get; }

        private MessageId(Guid value)
        {
            if(value == Guid.Empty)
            {
                throw new EmptyIdValueException();
            }

            Value = value;
        }

        public static MessageId Create(Guid value)
        {
            return new MessageId(value);
        }

        public static MessageId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static implicit operator Guid(MessageId movieId)
            => movieId.Value;

        public static implicit operator MessageId(Guid movieId)
            => new MessageId(movieId);
    }
}
