using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Moq;
using SiroccoDemo.Plugins;
using System;

namespace SiroccoDemo.Moq.Activities
{
    public static class CreateAccountWithContactsAndNotesMoq
    {
        public static void ExecuteMoq(CrmServiceClient crmService)
        {
            var factoryMock = new Mock<IOrganizationServiceFactory>();
            var pluginContextMock = new Mock<IPluginExecutionContext>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            factoryMock.Setup(f => f.CreateOrganizationService(It.IsAny<Guid>())).Returns(crmService);

            var inputParameters = new ParameterCollection();
            inputParameters.Add("AccountData", "Company ABC Moq");
            inputParameters.Add("AccountNotes", "[{\"Title\": \"Note Text\", \"Description\": \"Account-level note.\"}]");
            inputParameters.Add("PrimaryContactData", "{\"FirstName\": \"John\", \"LastName\": \"Primary\", \"Email\": \"john.primary@example.com\", \"Notes\": [{\"Title\": \"Note 1 Text\", \"Description\": \"First note for primary contact.\"}, {\"Title\": \"Note 2 Text\", \"Description\": \"Second note for primary contact.\"}]}");
            inputParameters.Add("SecondaryContactData", "[{\"FirstName\": \"Jane\", \"LastName\": \"Secondary\", \"Email\": \"jane.secondary@example.com\"}]");

            pluginContextMock.Setup(c => c.InputParameters).Returns(inputParameters);

            serviceProviderMock.Setup(s => s.GetService(typeof(IOrganizationServiceFactory))).Returns(factoryMock.Object);
            serviceProviderMock.Setup(s => s.GetService(typeof(IPluginExecutionContext))).Returns(pluginContextMock.Object);

            var plugin = new CreateAccountWithContactsAndNotes();
            plugin.Execute(serviceProviderMock.Object);
            
            Console.WriteLine("CreateAccountWithContactsAndNotes Plugin Moq Executed Successfully.");
        }
    }
}
