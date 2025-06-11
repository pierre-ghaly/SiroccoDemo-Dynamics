using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using SiroccoDemo.Application.Models;
using SiroccoDemo.Infrastructure.Repositories;
using SiroccoDemo.Infrastructure.Services;
using SiroccoDemo.Infrastructure.Validations;
using System;

namespace SiroccoDemo.Plugins
{
    public class CreateAccountWithContactsAndNotes : PluginBase
    {
        public override void ExtendedExecute()
        {
            var crmRepository = new CrmRepository(OrganizationService);

            var accountValidator = new AccountValidator();
            var contactValidator = new ContactValidator();
            var noteValidator = new NoteValidator();

            var service = new CreateAccountWithContactsAndNotesService(crmRepository, accountValidator, contactValidator, noteValidator);

            // Get input parameters from the execution context
            if (!Context.InputParameters.Contains("AccountData"))
            {
                throw new InvalidPluginExecutionException("Required input parameters are missing.");
            }

            var accountData = Context.InputParameters["AccountName"] as string;
            var accountNotes = Context.InputParameters["AccountNotes"] as NoteInput[];
            var primaryContactData = Context.InputParameters["PrimaryContactData"] as ContactInput;
            var secondaryContactData = Context.InputParameters["SecondaryContactData"] as ContactInput[];

            var accountModel = new CreateAccountWithContactsAndNotesModel();

            accountModel.Account.Name = JsonConvert.DeserializeObject<string>(accountData);
            accountModel.AccountNotes = JsonConvert.DeserializeObject<NoteInput[]>(accountNotes.ToString());
            accountModel.PrimaryContact = JsonConvert.DeserializeObject<ContactInput>(primaryContactData.ToString());
            accountModel.SecondaryContacts = JsonConvert.DeserializeObject<ContactInput[]>(secondaryContactData.ToString());

            try
            {
                service.CreateAccountWithContactsAndNotes(accountModel);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
