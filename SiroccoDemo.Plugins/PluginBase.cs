using Microsoft.Xrm.Sdk;
using System;

namespace SiroccoDemo.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        protected internal IOrganizationService OrganizationService { get; private set; }
        protected internal IPluginExecutionContext Context { get; private set; }

        public void Execute(IServiceProvider serviceProvider)

        {
            Context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            OrganizationService = serviceFactory.CreateOrganizationService(Context.UserId);

            try
            {
                ExtendedExecute();
            }
            catch (Exception exception)
            {
                throw new InvalidPluginExecutionException(exception.Message);
            }
        }

        public abstract void ExtendedExecute();
    }
}
