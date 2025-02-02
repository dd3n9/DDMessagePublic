using MediatR;
using MessageService.Application.Common.AsyncDataServices;
using MessageService.Contracts.DTO.Message;
using MessageService.Domain.MessageAggregate.Events;

namespace MessageService.Application.Messages.Events
{
    internal sealed class MessageCreatedDomainEventHandler
        : INotificationHandler<MessageCreatedDomainEvent>
    {
        private readonly IMessageBusClient _messageBusClient;

        public MessageCreatedDomainEventHandler(IMessageBusClient messageBusClient)
        {
            _messageBusClient = messageBusClient;
        }

        public Task Handle(MessageCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _messageBusClient.PublishNewMessageScheduled(new MessageSchedulerPublishedDto(
                    notification.MessageId, 
                    notification.ExternalOwnerId,
                    notification.DeliveryDate
                ));
            _messageBusClient.PublishNewMessageNotification(new MessageNotificationPublishedDto(
                    notification.MessageId, 
                    notification.Content, 
                    notification.DeliveryDate,
                    notification.RecipientEmails.Select(re => re.Value).ToList()
                ));


            return Task.CompletedTask;
        }
    }
}
