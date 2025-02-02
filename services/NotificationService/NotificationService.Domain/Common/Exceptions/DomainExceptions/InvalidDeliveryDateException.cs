using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Common.Models;

namespace NotificationService.Domain.Common.Exceptions.DomainExceptions
{
    public class InvalidDeliveryDateException : DomainException
    {
        public InvalidDeliveryDateException()
           : base("Delivery date must be at least 1 minute in the future.", StatusCodes.Status400BadRequest)
        {

        }
    }
}
