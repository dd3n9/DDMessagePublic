using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Common.Models;

namespace NotificationService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyContentException : DomainException
    {
        public EmptyContentException()
            : base("Content cannot be empty.", StatusCodes.Status400BadRequest)
        {

        }
    }
}
