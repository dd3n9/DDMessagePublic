using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Common.Models;

namespace NotificationService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyIdValueException : DomainException
    {
        public EmptyIdValueException()
            : base("Id cannot be null", StatusCodes.Status400BadRequest)
        {
        }
    }
}
