using FluentResults;

namespace NotificationService.Infrastructure.Exceptions
{
    public class InfrastructureErrors
    {
        public static class MailService
        {
            public static Error UnexpectedError(string message)
                  => new Error($"Unexpected mail service error: {message}")
                    .WithMetadata("ErrorCode", "MailService.UnexpectedError");
        }
    }
}
