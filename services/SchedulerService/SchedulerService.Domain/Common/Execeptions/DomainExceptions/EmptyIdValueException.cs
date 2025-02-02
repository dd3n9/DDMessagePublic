using Microsoft.AspNetCore.Http;
using SchedulerService.Domain.Common.Models;

namespace SchedulerService.Domain.Common.Execeptions.DomainExceptions
{
    public class EmptyIdValueException : DomainException
    {
        public EmptyIdValueException()
            : base("Id cannot be null", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
