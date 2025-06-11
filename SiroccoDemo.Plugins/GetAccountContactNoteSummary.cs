using Microsoft.Xrm.Sdk;
using SiroccoDemo.Infrastructure.Repositories;
using SiroccoDemo.Infrastructure.Services;
using System;

namespace SiroccoDemo.Plugins
{
    public class GetAccountContactNoteSummary : PluginBase
    {
        public override void ExtendedExecute()
        {
            var crmRepository = new CrmRepository(OrganizationService);

            var service = new GetAccountContactNoteSummaryService(crmRepository);

            try
            {
                service.GetAccountContactNoteSummary();
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("An unexpected error occurred: " + ex.Message);
            }

        }
    }
}
