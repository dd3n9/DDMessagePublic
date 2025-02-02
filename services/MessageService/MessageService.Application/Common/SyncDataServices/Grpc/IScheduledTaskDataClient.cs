using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Application.Common.SyncDataServices.Grpc
{
    public interface IScheduledTaskDataClient
    {
        Task<string> GetMessageStatusAsync(MessageId messageId);
    }
}
