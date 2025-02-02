using SchedulerService.Contracts.DTO;

namespace SchedulerService.Application.Common.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishScheduledStatusChangedToRunning(ScheduledStatusRunningPublishedDto publishedDto);
    }
}
