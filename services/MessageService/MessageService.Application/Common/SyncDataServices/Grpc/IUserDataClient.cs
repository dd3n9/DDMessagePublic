using MessageService.Contracts.DTO;
using MessageService.Domain.MessageAggregate.ValueObjects;

namespace MessageService.Application.Common.SyncDataServices.Grpc
{
    public interface IUserDataClient
    {
        Task<UserDto?> GetUserByEmailAsync(RecipientEmail email);
    }
}
