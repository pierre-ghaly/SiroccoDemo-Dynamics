using System;

namespace SiroccoDemo.Domain.Exceptions
{
    public class CrmException : ExceptionBase
    {
        public string CrmErrorCode { get; }
        public string EntityType { get; }
        public Guid? EntityId { get; }

        public CrmException(string message, string crmErrorCode = null, string entityType = null, Guid? entityId = null)
            : base(message, "CRM_ERROR")
        {
            CrmErrorCode = crmErrorCode;
            EntityType = entityType;
            EntityId = entityId;
        }

        public CrmException(string message, Exception innerException, string crmErrorCode = null, string entityType = null, Guid? entityId = null)
            : base(message, innerException, "CRM_ERROR")
        {
            CrmErrorCode = crmErrorCode;
            EntityType = entityType;
            EntityId = entityId;
        }

        public static CrmException ConnectionFailed(Exception innerException)
        {
            return new CrmException(
                "Failed to connect to CRM system. Please check your connection settings.",
                innerException,
                "CONNECTION_FAILED"
            );
        }

        public static CrmException EntityCreationFailed(string entityType, Exception innerException)
        {
            return new CrmException(
                $"Failed to create {entityType} entity in CRM.",
                innerException,
                "ENTITY_CREATION_FAILED",
                entityType
            );
        }
    }
} 