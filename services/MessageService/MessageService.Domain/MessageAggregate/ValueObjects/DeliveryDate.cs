using MessageService.Domain.Common.Exceptions.DomainExceptions;

namespace MessageService.Domain.MessageAggregate.ValueObjects
{
    public record DeliveryDate
    {
        public DateTime Value { get; }

        public DeliveryDate(DateTime value)
        {
            Value = value;
        }

        public static implicit operator DeliveryDate(DateTime value) 
            => new (value);

        public static implicit operator DateTime(DeliveryDate value)
            => value.Value;
    }
}
