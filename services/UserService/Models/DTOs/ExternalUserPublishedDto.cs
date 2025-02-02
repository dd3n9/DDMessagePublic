namespace UserService.Models.DTOs
{
    public record ExternalUserPublishedDto(Guid ExternalId, string Email, string Event);
}
