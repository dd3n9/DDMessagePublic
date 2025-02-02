using SchedulerService.Domain.Common.Execeptions.DomainExceptions;

namespace SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects
{
    public record UserId
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            if (value == Guid.Empty)
                throw new EmptyIdValueException();

            Value = value;
        }

        public static UserId Create(Guid value)
        {
            return new UserId(value);
        }

        public static UserId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static implicit operator Guid(UserId value)
            => value.Value;

        public static implicit operator UserId(Guid value)
            => new(value);
    }
}
