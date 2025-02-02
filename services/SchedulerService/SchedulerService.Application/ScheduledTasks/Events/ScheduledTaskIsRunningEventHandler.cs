using MediatR;
using SchedulerService.Application.Common.AsyncDataServices;
using SchedulerService.Contracts.DTO;
using SchedulerService.Domain.ScheduledTaskAggregate.Events;

namespace SchedulerService.Application.ScheduledTasks.Events
{
    internal sealed class ScheduledTaskIsRunningEventHandler
        : INotificationHandler<ScheduledTaskIsRunning>
    {
        private readonly IMessageBusClient _messageBusClient;

        public ScheduledTaskIsRunningEventHandler(IMessageBusClient messageBusClient)
        {
            _messageBusClient = messageBusClient;
        }

        public Task Handle(ScheduledTaskIsRunning notification, CancellationToken cancellationToken)
        {
            _messageBusClient.PublishScheduledStatusChangedToRunning(new ScheduledStatusRunningPublishedDto(
                notification.externalMessageId));

            return Task.CompletedTask;
        }
    }
}
