using Gateway.Api.Configurations;
using Gateway.Api.Interfaces.Authentication;
using Gateway.Api.Interfaces.Services;
using Gateway.Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Gateway.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPasswordHasher _passwordHasher;
        private readonly CookiesConfig _cookiesConfig;

        public AuthController(IUserService userService,
            ITokenService tokenService,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher, 
            IOptions<CookiesConfig> cookiesConfig)
        {
            _userService = userService;
            _tokenService = tokenService;
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
            _cookiesConfig = cookiesConfig.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var existingUser = await _userService.GetUserByEmail(registerDto.Email);

            if (existingUser is not null)
                return Conflict("User already exists");

            await _userService.AddUserAsync(registerDto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.GetUserByEmail(loginDto.Email);
            if (user.ExternalUserId is null || !_passwordHasher.Verify(user.HashedPassword, loginDto.Password))
                return Unauthorized();

            var authResult = await _tokenProvider.GenerateToken(new AuthenticationDto(
                user.ExternalUserId.Value,
                user.Email));

            HttpContext.Response.Cookies.Append(_cookiesConfig.CookiesKey, authResult.RefreshToken);

            return Ok(authResult.Token);
        }

        [Route("refreshToken")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            var refreshToken = HttpContext.Request.Cookies[_cookiesConfig.CookiesKey];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized();

            var principal = _tokenProvider.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email");

            if (emailClaim == null || string.IsNullOrWhiteSpace(emailClaim.Value))
                return Unauthorized();

            var user = await _userService.GetUserByEmail(emailClaim.Value);
            var storedRefreshToken = await _tokenService.FindRefreshTokenAsync(refreshToken);

            if (user.ExternalUserId is null || storedRefreshToken is null || storedRefreshToken.Token != refreshToken || storedRefreshToken.ExpiryDate < DateTime.UtcNow)
                return Unauthorized();

            var authResult = await _tokenProvider.GenerateToken(new AuthenticationDto(
                user.ExternalUserId.Value,
                user.Email));

            HttpContext.Response.Cookies.Append(_cookiesConfig.CookiesKey, authResult.RefreshToken);

            return Ok(authResult.Token);
        }

        [Authorize]
        [HttpDelete]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized();

            var user = await _userService.GetUserByEmail(userEmail);

            if (user.ExternalUserId is null)
                return Unauthorized();

            await _tokenService.RevokeTokenAsync(user.Id);

            return Ok();
        }
    }
}
