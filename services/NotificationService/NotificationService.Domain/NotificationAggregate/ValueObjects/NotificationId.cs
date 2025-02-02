using NotificationService.Domain.Common.Exceptions.DomainExceptions;

namespace NotificationService.Domain.NotificationAggregate.ValueObjects
{
    public record NotificationId
    {
        public Guid Value { get; }

        private NotificationId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new EmptyIdValueException();
            }

            Value = value;
        }

        public static NotificationId Create(Guid value)
        {
            return new NotificationId(value);
        }

        public static NotificationId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static implicit operator Guid(NotificationId movieId)
            => movieId.Value;

        public static implicit operator NotificationId(Guid movieId)
            => new NotificationId(movieId);
    }
}
