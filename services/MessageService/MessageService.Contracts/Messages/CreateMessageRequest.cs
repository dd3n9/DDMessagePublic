namespace MessageService.Contracts.Messages
{
    public record CreateMessageRequest(
        string MessageTitle,
        string Content, 
        DateTime DeliveryDate, 
        IEnumerable<string> RecipientEmails);
}
