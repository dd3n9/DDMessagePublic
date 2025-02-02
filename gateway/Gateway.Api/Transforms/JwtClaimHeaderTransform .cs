using Yarp.ReverseProxy.Transforms;

namespace Gateway.Api.Transforms
{
    public class JwtClaimHeaderTransform : RequestTransform
    {
        private readonly string _claimType;
        private readonly string _headerName;

        public JwtClaimHeaderTransform(string claimType, string headerName)
        {
            _claimType = claimType;
            _headerName = headerName;
        }

        public override ValueTask ApplyAsync(RequestTransformContext context)
        {
            var claimValue = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == _claimType)?.Value;

            if (!string.IsNullOrEmpty(claimValue))
            {
                context.ProxyRequest.Headers.Remove(_headerName);
                context.ProxyRequest.Headers.TryAddWithoutValidation(_headerName, claimValue);
            }

            return ValueTask.CompletedTask;
        }
    }
}
