using System;

namespace SiroccoDemo.Domain.Exceptions
{
    public class BusinessRuleException : ExceptionBase
    {
        public string RuleName { get; }
        public string EntityType { get; }

        public BusinessRuleException(string message, string ruleName = null, string entityType = null)
            : base(message, "BUSINESS_RULE_VIOLATION")
        {
            RuleName = ruleName;
            EntityType = entityType;
        }

        public BusinessRuleException(string message, Exception innerException, string ruleName = null, string entityType = null)
            : base(message, innerException, "BUSINESS_RULE_VIOLATION")
        {
            RuleName = ruleName;
            EntityType = entityType;
        }
    }
} 