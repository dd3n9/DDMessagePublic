using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerService.Domain.Common.Models
{
    public abstract class DomainException : Exception
    {
        public int StatusCode { get; }

        protected DomainException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode; 
        }
    }
}
