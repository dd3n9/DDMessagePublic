
namespace NotificationService.Domain.Common.Models
{
    public class AggregateRoot<TId> : Entity<TId>
       where TId : notnull
    {

        protected AggregateRoot(TId id) : base(id)
        {
        }

        protected AggregateRoot() { }

        protected void AddEvent(IDomainEvent domainEvent)
        {
            AddDomainEvents(domainEvent);
        }
    }
}
