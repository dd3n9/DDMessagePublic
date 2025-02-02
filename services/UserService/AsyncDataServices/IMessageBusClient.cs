using UserService.Models.DTOs;

namespace UserService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishExternalUser(ExternalUserPublishedDto publishedDto);
    }
}
