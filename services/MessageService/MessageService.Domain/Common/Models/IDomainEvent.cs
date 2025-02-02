using MediatR;

namespace MessageService.Domain.Common.Models
{
    public interface IDomainEvent : INotification
    {
    }
}
