using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException()
            : base("Invalid email format.", StatusCodes.Status400BadRequest)
        {

        }
    }
}
