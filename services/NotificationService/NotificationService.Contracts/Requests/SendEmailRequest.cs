namespace NotificationService.Contracts.Requests
{
    public record SendEmailRequest(string Recipient, string Body);
}
