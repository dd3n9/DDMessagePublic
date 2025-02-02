using Mapster;
using NotificationService.Contracts.DTO;
using NotificationService.Domain.NotificationAggregate;

namespace NotificationService.Api.Mapping
{
    public class NoficationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Notification, NotificationPublishedDto>();
        }
    }
}
