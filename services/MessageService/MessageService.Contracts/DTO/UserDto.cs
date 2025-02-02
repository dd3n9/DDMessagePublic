namespace MessageService.Contracts.DTO
{
    public record UserDto(Guid? ExternalUserId, string Email);
}
