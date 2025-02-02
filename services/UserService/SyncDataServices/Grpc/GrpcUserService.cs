using Grpc.Core;
using MapsterMapper;
using UserService.Repositories;

namespace UserService.SyncDataServices.Grpc
{
    public class GrpcUserService : GrpcUser.GrpcUserBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GrpcUserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public override async Task<GetUserByEmailResponse> GetUserByEmail(GetUserByEmailRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email); 
            var response = new GetUserByEmailResponse { Email = request.Email };

            if (user != null)
            {
                response.ExternalUserId = user.Id.ToString();
            }
            
            return response;
        }
    }
}
