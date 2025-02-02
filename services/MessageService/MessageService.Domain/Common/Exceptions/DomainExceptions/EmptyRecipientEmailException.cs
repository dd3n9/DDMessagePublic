using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyRecipientEmailException : DomainException
    {
        public EmptyRecipientEmailException()
            : base("Email cannot be empty.", StatusCodes.Status400BadRequest)
        {

        }
    }
}
