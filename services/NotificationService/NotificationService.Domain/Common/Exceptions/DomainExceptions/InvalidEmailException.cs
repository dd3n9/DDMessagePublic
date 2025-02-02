using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Common.Models;

namespace NotificationService.Domain.Common.Exceptions.DomainExceptions
{
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException()
            : base("Invalid email format.", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
