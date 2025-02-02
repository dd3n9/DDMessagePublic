using Grpc.Net.Client;
using MapsterMapper;
using MessageService.Application.Common.SyncDataServices.Grpc;
using MessageService.Contracts.DTO;
using MessageService.Domain.MessageAggregate.ValueObjects;
using MessageService.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using UserService;

namespace MessageService.Infrastructure.Common.SyncDataServices.Grpc
{
    internal sealed class UserDataClient : IUserDataClient
    {
        private readonly GrpcUserSettings _grpcUserSettings;
        private readonly IMapper _mapper;

        public UserDataClient(IOptions<GrpcUserSettings> grpcUserSettings, IMapper mapper)
        {
            _grpcUserSettings = grpcUserSettings.Value;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetUserByEmailAsync(RecipientEmail email)
        {
            Console.WriteLine($"--> Calling GRPC Service {_grpcUserSettings.GrpcUser}");
            var channel = GrpcChannel.ForAddress(_grpcUserSettings.GrpcUser);
            var client = new GrpcUser.GrpcUserClient(channel);
            var request = new GetUserByEmailRequest();
            request.Email = email.Value;

            try
            {
                var reply = await client.GetUserByEmailAsync(request);
                return _mapper.Map<UserDto>(reply);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC server {ex.Message}");
                return null;
            }
        }
    }
}
