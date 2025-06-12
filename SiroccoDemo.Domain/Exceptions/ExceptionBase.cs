using System;

namespace SiroccoDemo.Domain.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public string ErrorCode { get; }
        public DateTime OccurredAt { get; }

        protected ExceptionBase(string message, string errorCode = null) 
            : base(message)
        {
            ErrorCode = errorCode ?? GetType().Name;
            OccurredAt = DateTime.UtcNow;
        }

        protected ExceptionBase(string message, Exception innerException, string errorCode = null) 
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? GetType().Name;
            OccurredAt = DateTime.UtcNow;
        }
    }
} 