namespace Gateway.Api.Models.DTOs
{
    public record UserPublishedDto(string UserName, string Email, string HashedPassword, string Event);
}
