namespace NotificationService.Contracts.DTO
{
    public record NotificationPublishedDto(
        Guid ExternalMessageId,
        string Content,
        DateTime DeliveryDate,
        IEnumerable<string> RecipientsEmail
        );
}
