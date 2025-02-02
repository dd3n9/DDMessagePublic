using Grpc.Net.Client;
using MapsterMapper;
using MessageService.Application.Common.SyncDataServices.Grpc;
using MessageService.Domain.MessageAggregate.ValueObjects;
using MessageService.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using SchedulerService;
namespace MessageService.Infrastructure.Common.SyncDataServices.Grpc
{
    internal sealed class ScheduledTaskDataClient : IScheduledTaskDataClient
    {
        private readonly GrpcScheduledTaskSettings _grpcSettings;
        private readonly IMapper _mapper;

        public ScheduledTaskDataClient(IOptions<GrpcScheduledTaskSettings> grpcSettings, IMapper mapper)
        {
            _grpcSettings = grpcSettings.Value;
            _mapper = mapper;
        }

        public async Task<string> GetMessageStatusAsync(MessageId messageId)
        {
            Console.WriteLine($"--> Calling GRPC Service {_grpcSettings.GrpcScheduledTask}");
            var channel = GrpcChannel.ForAddress(_grpcSettings.GrpcScheduledTask);
            var client = new GrpcScheduler.GrpcSchedulerClient(channel);
            var request = new GetMessageStatusRequest();
            request.ExternalMessageId = messageId.Value.ToString();

            try
            {
                var reply = await client.GetMessageStatusAsync(request);
                return reply.MessageStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC server {ex.Message}");
                return null;
            }
        }
    }
}
