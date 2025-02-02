using Gateway.Api.Models;
using Gateway.Api.Models.DTOs;
using System.Security.Claims;

namespace Gateway.Api.Interfaces.Authentication
{
    public interface ITokenProvider
    {
        Task<AuthResult> GenerateToken(AuthenticationDto authenticationDto);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
