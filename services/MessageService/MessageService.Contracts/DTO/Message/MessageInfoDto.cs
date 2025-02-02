namespace MessageService.Contracts.DTO.Message
{
    public record MessageInfoDto(string MessageTitle, DateTime DeliveryDate, string Status);
}
