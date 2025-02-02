using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Common.Models;

namespace NotificationService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyRecipientEmailException : DomainException
    {
        public EmptyRecipientEmailException()
            : base("Email cannot be empty.", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
