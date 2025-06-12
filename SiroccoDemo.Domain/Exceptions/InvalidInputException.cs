using System;
using System.Collections.Generic;

namespace SiroccoDemo.Domain.Exceptions
{
    public class InvalidInputException : ExceptionBase
    {
        public string PropertyName { get; }
        public object InvalidValue { get; }
        public List<string> ValidationErrors { get; }

        public InvalidInputException(string message, string propertyName = null, object invalidValue = null)
            : base(message, "INVALID_INPUT")
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
            ValidationErrors = new List<string> { message };
        }

        public InvalidInputException(string message, Exception innerException, string propertyName = null)
            : base(message, innerException, "INVALID_INPUT")
        {
            PropertyName = propertyName;
            ValidationErrors = new List<string> { message };
        }

        public InvalidInputException(List<string> validationErrors)
            : base($"Input validation failed with {validationErrors.Count} error(s)", "INVALID_INPUT")
        {
            ValidationErrors = validationErrors ?? new List<string>();
        }
    }
} 