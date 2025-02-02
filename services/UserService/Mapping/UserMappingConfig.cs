using Mapster;
using UserService.Models;
using UserService.Models.DTOs;

namespace UserService.Mapping
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserPublishedDto>();
            config.NewConfig<User, GetUserByEmailResponse>()
                .Map(dest => dest.ExternalUserId, src => src.Id.ToString())
                .Map(dest => dest.Email, src => src.Email);
        }
    }
}
