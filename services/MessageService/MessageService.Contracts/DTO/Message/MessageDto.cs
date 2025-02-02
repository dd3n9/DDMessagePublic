namespace MessageService.Contracts.DTO.Message
{
    public record MessageDto(
        string MessageTitle, 
        string Content, 
        DateTime DeliveryDate, 
        string Status, 
        IEnumerable<string> RecipientEmails);
}
