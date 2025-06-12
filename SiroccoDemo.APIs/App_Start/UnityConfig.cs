using Microsoft.Xrm.Sdk;
using SiroccoDemo.APIs.Services;
using SiroccoDemo.Application.Repositories;
using SiroccoDemo.Application.Services;
using SiroccoDemo.Application.Validations;
using SiroccoDemo.Infrastructure.Repositories;
using SiroccoDemo.Infrastructure.Services;
using SiroccoDemo.Infrastructure.Validations;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace SiroccoDemo.APIs
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register CRM Organization Service
            container.RegisterFactory<IOrganizationService>(c => CrmServiceFactory.CreateOrganizationService());

            // Register repositories
            container.RegisterType<ICrmRepository, CrmRepository>();

            // Register validators
            container.RegisterType<IAccountValidator, AccountValidator>();
            container.RegisterType<IContactValidator, ContactValidator>();
            container.RegisterType<INoteValidator, NoteValidator>();

            // Register services
            container.RegisterType<ICreateAccountWithContactsAndNotesService, CreateAccountWithContactsAndNotesService>();
            container.RegisterType<IGetAccountContactNoteSummaryService, GetAccountContactNoteSummaryService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
} 