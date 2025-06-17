using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using SiroccoDemo.Moq.Activities;
using System;
using System.Configuration;

namespace SiroccoDemo.Moq
{
    class Program
    {
        public static IOrganizationService OrganizationService { get; private set; }

        static void Main(string[] args)
        {
            var service = GetAccessToCRM();
            CreateAccountWithContactsAndNotesMoq.ExecuteMoq(service);
        }

        public static CrmServiceClient GetAccessToCRM()
        {
            try
            {

                CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["Moq"].ConnectionString);

                return crmSvc;
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                }
                throw;
            }
        }
    }
}
