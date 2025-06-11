using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
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
                var result = service.GetAccountContactNoteSummary();
                
                Context.OutputParameters["Result"] = JsonConvert.SerializeObject(result, Formatting.None);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
