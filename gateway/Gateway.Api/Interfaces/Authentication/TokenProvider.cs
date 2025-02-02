using Gateway.Api.Configurations;
using Gateway.Api.Data;
using Gateway.Api.Interfaces.Services;
using Gateway.Api.Models;
using Gateway.Api.Models.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Gateway.Api.Interfaces.Authentication
{
    public class TokenProvider : ITokenProvider
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IUserService _userService;
        private readonly AppDbContext _appDbContext;

        public TokenProvider(IOptions<JwtConfig> jwtConfig,
            IUserService userService,
            AppDbContext appDbContext)
        {
            _jwtConfig = jwtConfig.Value;
            _userService = userService;
            _appDbContext = appDbContext;
        }

        public async Task<AuthResult> GenerateToken(AuthenticationDto authenticationDto)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);


            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                   new Claim("Id", authenticationDto.ExternalId.ToString()),
                   new Claim(JwtRegisteredClaimNames.Sub, authenticationDto.Email),
                   new Claim(JwtRegisteredClaimNames.Email, authenticationDto.Email),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),
                SigningCredentials = signingCredentials,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var userId = await _userService.GetUserIdByExternalId(authenticationDto.ExternalId);
            if (userId is null)
                throw new Exception("User not found");

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = GenerateRefreshToken(),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                UserId = userId.Value
            };

            await _appDbContext.RefreshTokens.AddAsync(refreshToken);
            await _appDbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var secret = _jwtConfig.Secret;

            var validation = new TokenValidationParameters()
            {
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = true
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
    }
}
