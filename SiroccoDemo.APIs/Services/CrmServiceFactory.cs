using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;

namespace SiroccoDemo.APIs.Services
{
    public static class CrmServiceFactory
    {
        public static IOrganizationService CreateOrganizationService()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["CrmConnectionString"]?.ConnectionString;
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("CRM connection string not found in web.config");
                }

                var crmServiceClient = new CrmServiceClient(connectionString);
                
                if (!crmServiceClient.IsReady)
                {
                    throw new InvalidOperationException($"Failed to connect to CRM: {crmServiceClient.LastCrmError}");
                }

                if (crmServiceClient.OrganizationWebProxyClient != null)
                {
                    return crmServiceClient.OrganizationWebProxyClient;
                }
                else if (crmServiceClient.OrganizationServiceProxy != null)
                {
                    return crmServiceClient.OrganizationServiceProxy;
                }
                else
                {
                    throw new InvalidOperationException("No organization service proxy available from CRM Service Client");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating CRM Organization Service", ex);
            }
        }
    }
} 