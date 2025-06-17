using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using SiroccoDemo.Application.Models;
using SiroccoDemo.Infrastructure.Repositories;
using SiroccoDemo.Infrastructure.Services;
using SiroccoDemo.Infrastructure.Validations;

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

            var service = new CreateAccountWithContactsAndNotesService(crmRepository, OrganizationService, accountValidator, contactValidator, noteValidator);

            if (!Context.InputParameters.Contains("AccountData"))
            {
                throw new InvalidPluginExecutionException("Required input parameter 'AccountData' is missing.");
            }

            var accountData = Context.InputParameters["AccountData"] as string;
            var accountNotes = Context.InputParameters.Contains("AccountNotes") ? Context.InputParameters["AccountNotes"] as string : null;
            var primaryContactData = Context.InputParameters.Contains("PrimaryContactData") ? Context.InputParameters["PrimaryContactData"] as string : null;
            var secondaryContactData = Context.InputParameters.Contains("SecondaryContactData") ? Context.InputParameters["SecondaryContactData"] as string : null;

            if (string.IsNullOrEmpty(accountData))
            {
                throw new InvalidPluginExecutionException("AccountData cannot be null or empty.");
            }

            var accountModel = new CreateAccountWithContactsAndNotesModel();

            accountModel.Account = new AccountInput
            {
                Name = accountData
            };

            if (!string.IsNullOrEmpty(accountNotes))
            {
                accountModel.AccountNotes = JsonConvert.DeserializeObject<NoteInput[]>(accountNotes);
            }
            else
            {
                accountModel.AccountNotes = new NoteInput[0];
            }

            if (!string.IsNullOrEmpty(primaryContactData))
            {
                var primaryContactWithNotes = JsonConvert.DeserializeObject<ContactInput>(primaryContactData);
                
                accountModel.PrimaryContact = new ContactInput
                {
                    FirstName = primaryContactWithNotes.FirstName,
                    LastName = primaryContactWithNotes.LastName,
                    Email = primaryContactWithNotes.Email,
                    Notes = null
                };
                
                if (primaryContactWithNotes.Notes != null && primaryContactWithNotes.Notes.Length > 0)
                {
                    accountModel.PrimaryContactNotes = primaryContactWithNotes.Notes;
                }
                else
                {
                    accountModel.PrimaryContactNotes = new NoteInput[0];
                }
            }
            else
            {
                accountModel.PrimaryContact = null;
                accountModel.PrimaryContactNotes = new NoteInput[0];
            }

            if (!string.IsNullOrEmpty(secondaryContactData))
            {
                accountModel.SecondaryContacts = JsonConvert.DeserializeObject<ContactInput[]>(secondaryContactData);
            }
            else
            {
                accountModel.SecondaryContacts = new ContactInput[0];
            }

            ExecuteWithExceptionHandling(() => service.CreateAccountWithContactsAndNotes(accountModel));
        }
    }
}
