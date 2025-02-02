using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyUserEmailException : DomainException
    {
        public EmptyUserEmailException()
            : base("User Email cannot be empty", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
