using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyIdValueException : DomainException
    {
        public EmptyIdValueException()
            : base("Id cannot be null", StatusCodes.Status400BadRequest)
        {   
        }
    }
}
