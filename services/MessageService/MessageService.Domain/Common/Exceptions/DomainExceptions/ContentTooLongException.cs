using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class ContentTooLongException : DomainException
    {
        public ContentTooLongException(int maxLength)
            : base($"Content cannot be longer than {maxLength} characters.", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
