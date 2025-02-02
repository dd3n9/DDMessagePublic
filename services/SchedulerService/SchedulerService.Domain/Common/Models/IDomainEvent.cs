using MediatR;

namespace SchedulerService.Domain.Common.Models
{
    public interface IDomainEvent : INotification
    {
    }
}
