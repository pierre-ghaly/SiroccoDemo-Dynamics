using Newtonsoft.Json;
using SiroccoDemo.Infrastructure.Repositories;
using SiroccoDemo.Infrastructure.Services;

namespace SiroccoDemo.Plugins
{
    public class GetAccountContactNoteSummary : PluginBase
    {
        public override void ExtendedExecute()
        {
            var crmRepository = new CrmRepository(OrganizationService);

            var service = new GetAccountContactNoteSummaryService(crmRepository);

            var result = ExecuteWithExceptionHandling(() => service.GetAccountContactNoteSummary());
            
            Context.OutputParameters["Result"] = JsonConvert.SerializeObject(result, Formatting.None);
        }
    }
}
