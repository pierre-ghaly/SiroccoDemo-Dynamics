using System;

namespace SiroccoDemo.Domain.Exceptions
{
    public class TransactionFailedException : ExceptionBase
    {
        public string OperationName { get; }
        public int OperationCount { get; }
        public string TransactionId { get; }

        public TransactionFailedException(string message, string operationName = null, int operationCount = 0)
            : base(message, "TRANSACTION_FAILED")
        {
            OperationName = operationName;
            OperationCount = operationCount;
            TransactionId = Guid.NewGuid().ToString("N").Substring(0, 8); // Short transaction ID for logging
        }

        public TransactionFailedException(string message, Exception innerException, string operationName = null, int operationCount = 0)
            : base(message, innerException, "TRANSACTION_FAILED")
        {
            OperationName = operationName;
            OperationCount = operationCount;
            TransactionId = Guid.NewGuid().ToString("N").Substring(0, 8);
        }
    }
} 