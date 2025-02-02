namespace MessageService.Contracts.DTO.Message
{
    public record CreateMessageDto(
        string MessageTitle,
        string Content,
        DateTime DeliveryDate,
        string OwnerEmail,
        IEnumerable<string> RecipientEmails);
}