using Mapster;
using MessageService.Contracts.DTO;
using UserService;

namespace MessageService.Api.Mapping
{
    public class MessageMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GetUserByEmailResponse, UserDto>()
                .Map(dest => dest.ExternalUserId, src => string.IsNullOrEmpty(src.ExternalUserId)
                                                        ? (Guid?)null : Guid.Parse(src.ExternalUserId));
        }
    }
}
