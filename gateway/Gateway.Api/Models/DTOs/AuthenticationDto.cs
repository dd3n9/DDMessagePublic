namespace Gateway.Api.Models.DTOs
{
    public record AuthenticationDto(Guid ExternalId, string Email);
}
