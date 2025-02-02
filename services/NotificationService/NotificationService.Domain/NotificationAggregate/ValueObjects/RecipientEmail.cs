using NotificationService.Domain.Common.Exceptions.DomainExceptions;

namespace NotificationService.Domain.NotificationAggregate.ValueObjects
{
    public record RecipientEmail
    {
        public string Value { get; }

        public RecipientEmail(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new EmptyRecipientEmailException();

            if(!IsValidEmail(value))
                throw new InvalidEmailException();

            Value = value;
        }

        public static implicit operator RecipientEmail(string value)
            => new(value);

        public static implicit operator string(RecipientEmail recipientMail)
            => recipientMail.Value;


        private static bool IsValidEmail(string email)
        {
            var emailRegex = new System.Text.RegularExpressions.Regex(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase
                ); 

            return emailRegex.IsMatch( email );
        }
    }
}
