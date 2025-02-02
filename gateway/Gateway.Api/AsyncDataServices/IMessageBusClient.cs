using Gateway.Api.Models.DTOs;

namespace Gateway.Api.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewUser(UserPublishedDto userPublishedDto);
    }
}
