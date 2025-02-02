﻿
namespace MessageService.Domain.Common.Models
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
