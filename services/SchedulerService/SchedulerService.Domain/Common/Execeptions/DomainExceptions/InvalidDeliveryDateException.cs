using Microsoft.AspNetCore.Http;
using SchedulerService.Domain.Common.Models;

namespace SchedulerService.Domain.Common.Execeptions.DomainExceptions
{
    public class InvalidDeliveryDateException : DomainException
    {
        public InvalidDeliveryDateException()
           : base("Delivery date must be at least 1 minute in the future.", StatusCodes.Status400BadRequest)
        {

        }
    }
}
