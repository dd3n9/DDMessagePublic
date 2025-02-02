using SchedulerService.Domain.Common.Execeptions.DomainExceptions;

namespace SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects
{
    public record ScheduledId
    {
        public Guid Value { get; }

        public ScheduledId(Guid value)
        {
            if (value == Guid.Empty)
                throw new EmptyIdValueException();

            Value = value;
        }

        public static ScheduledId Create(Guid value)
        {
            return new ScheduledId(value);  
        }

        public static ScheduledId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static implicit operator Guid(ScheduledId value)
            => value.Value;

        public static implicit operator ScheduledId(Guid value)
            => new (value);
    }
}
