using MessageService.Contracts.DTO.Message;

namespace MessageService.Application.Common.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewMessageNotification(MessageNotificationPublishedDto publishedDto);
        void PublishNewMessageScheduled(MessageSchedulerPublishedDto publishedDto);
    }
}
