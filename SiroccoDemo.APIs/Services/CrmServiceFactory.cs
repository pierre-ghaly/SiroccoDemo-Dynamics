using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using SiroccoDemo.Domain.Exceptions;
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
                    throw new CrmException("CRM connection string not found in web.config", "MISSING_CONNECTION_STRING");
                }

                var crmServiceClient = new CrmServiceClient(connectionString);
                
                if (!crmServiceClient.IsReady)
                {
                    throw CrmException.ConnectionFailed(new InvalidOperationException(crmServiceClient.LastCrmError));
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
            catch (CrmException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw CrmException.ConnectionFailed(ex);
            }
        }
    }
} 