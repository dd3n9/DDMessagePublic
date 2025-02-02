using SchedulerService.Domain.Repositories;
using Grpc.Core;

namespace SchedulerService.Infrastructure.Common.SyncDataServices
{
    public class GrpcScheduledTaskService : GrpcScheduler.GrpcSchedulerBase
    {
        private readonly IScheduledTaskRepository _repository;

        public GrpcScheduledTaskService(IScheduledTaskRepository repository)
        {
            _repository = repository;
        }

        public override async Task<GetMessageStatusResponse> GetMessageStatus(GetMessageStatusRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.ExternalMessageId, out var externalMessageIdGuid))
            {
                return new GetMessageStatusResponse { MessageStatus = "Invalid Message ID" };
            }

            var messageStatusEnum = await _repository.GetStatusAsync(externalMessageIdGuid);
            var messageStatus = messageStatusEnum.ToString();
            return new GetMessageStatusResponse { MessageStatus = messageStatus };
        }
    }
}
