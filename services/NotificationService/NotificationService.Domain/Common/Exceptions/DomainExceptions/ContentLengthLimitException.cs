using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Common.Models;

namespace NotificationService.Domain.Common.Exceptions.DomainExceptions
{
    public class ContentLengthLimitException : DomainException
    {
        public ContentLengthLimitException(int maxLength)
          : base($"Content cannot be longer than {maxLength} characters.", StatusCodes.Status400BadRequest)
        {

        }
    }
}
