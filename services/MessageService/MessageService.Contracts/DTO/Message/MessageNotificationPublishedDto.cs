namespace MessageService.Contracts.DTO.Message
{
    public record MessageNotificationPublishedDto(
        Guid ExternalMessageId,
        string Content,
        DateTime DeliveryDate,
        IEnumerable<string> RecipientsEmail,
        string Event = "Message_Published.Notification"
        );
}
