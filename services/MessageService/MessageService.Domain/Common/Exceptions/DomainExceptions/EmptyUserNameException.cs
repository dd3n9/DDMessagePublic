using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyUserNameException : DomainException
    {
        public EmptyUserNameException()
            : base("User Name cannot be empty", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
