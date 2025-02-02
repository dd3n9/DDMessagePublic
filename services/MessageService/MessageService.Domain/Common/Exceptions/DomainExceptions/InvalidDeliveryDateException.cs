using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class InvalidDeliveryDateException : DomainException
    {
        public InvalidDeliveryDateException()
            : base("Delivery date must be at least 1 minute in the future.", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
