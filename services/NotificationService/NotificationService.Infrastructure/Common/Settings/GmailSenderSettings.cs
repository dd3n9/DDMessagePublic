namespace NotificationService.Infrastructure.Common.Settings
{
    public class GmailSenderSettings
    {
        public const string GmailSettingsKey = "GmailSenderSettings";

        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
    }
}
