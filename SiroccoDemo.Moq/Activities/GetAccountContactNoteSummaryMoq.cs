using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Moq;
using SiroccoDemo.Plugins;
using System;

namespace SiroccoDemo.Moq.Activities
{
    public static class GetAccountContactNoteSummaryMoq
    {
        public static void ExecuteMoq(CrmServiceClient crmService)
        {
            var factoryMock = new Mock<IOrganizationServiceFactory>();
            var pluginContextMock = new Mock<IPluginExecutionContext>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            factoryMock.Setup(f => f.CreateOrganizationService(It.IsAny<Guid>())).Returns(crmService);

            var outputParameters = new ParameterCollection();
            pluginContextMock.Setup(c => c.OutputParameters).Returns(outputParameters);

            serviceProviderMock.Setup(s => s.GetService(typeof(IOrganizationServiceFactory))).Returns(factoryMock.Object);
            serviceProviderMock.Setup(s => s.GetService(typeof(IPluginExecutionContext))).Returns(pluginContextMock.Object);

            var plugin = new GetAccountContactNoteSummary();
            plugin.Execute(serviceProviderMock.Object);

            Console.WriteLine("GetAccountContactNoteSummary Plugin Moq Executed Successfully.");

            if (outputParameters.Contains("Result"))
            {
                var result = outputParameters["Result"] as string;
                Console.WriteLine($"Result: {result}");
            }
            else
            {
                Console.WriteLine("No Result");
            }
        }
    }
}
