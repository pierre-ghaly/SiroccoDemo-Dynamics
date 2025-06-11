using Microsoft.Xrm.Sdk;
using SiroccoDemo.Application.Repositories;
using SiroccoDemo.Domain.Entities;
using System;

namespace SiroccoDemo.Infrastructure.Repositories
{
    internal class CrmRepository : ICrmRepository
    {
        private readonly IOrganizationService _service;
        public CrmRepository(IOrganizationService service) => _service = service;

        public Guid CreateAccount(Account account)
        {
            var entity = new Entity("account")
            {
                ["name"] = account.Name
            };
            return _service.Create(entity);
        }

        public Guid CreateContact(Contact contact)
        {
            var entity = new Entity("contact")
            {
                ["firstname"] = contact.FirstName,
                ["lastname"] = contact.LastName,
                ["emailaddress1"] = contact.Email,
                ["parentcustomerid"] = new EntityReference("account", contact.AccountId)
            };
            return _service.Create(entity);
        }

        public Guid CreateNote(Note note)
        {
            var entity = new Entity("annotation")
            {
                ["subject"] = note.Title,
                ["notetext"] = note.Description,
                ["objectid"] = new EntityReference(note.RegardingEntityName, note.RegardingId)
            };
            return _service.Create(entity);
        }

        public void SetPrimaryContact(Guid accountId, Guid contactId)
        {
            var account = new Entity("account", accountId)
            {
                ["primarycontactid"] = new EntityReference("contact", contactId)
            };
            _service.Update(account);
        }
    }
}
