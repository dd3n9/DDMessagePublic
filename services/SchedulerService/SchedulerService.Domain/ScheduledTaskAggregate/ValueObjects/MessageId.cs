using SchedulerService.Domain.Common.Execeptions.DomainExceptions;

namespace SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects
{
    public record MessageId
    {
        public Guid Value { get; }

        public MessageId(Guid value)
        {
            if (value == Guid.Empty)
                throw new EmptyIdValueException();

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

        public static implicit operator Guid(MessageId messageId) 
            => messageId.Value;

        public static implicit operator MessageId(Guid messageId)
            => new(messageId);
    }
}
