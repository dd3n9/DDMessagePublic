using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyMessageTitleException : DomainException
    {
        public EmptyMessageTitleException()
            : base("Title cannot be empty.", StatusCodes.Status400BadRequest)
        {

        }
    }
}
