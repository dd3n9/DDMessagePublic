using MessageService.Domain.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Domain.Common.Exceptions.DomainExceptions
{
    public class EmptyContentException : DomainException
    {
        public EmptyContentException()
            :base("Content cannot be empty.", StatusCodes.Status400BadRequest)
        {
            
        }
    }
}
