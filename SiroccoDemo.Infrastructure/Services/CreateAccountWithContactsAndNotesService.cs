using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Repositories;
using SiroccoDemo.Application.Services;
using SiroccoDemo.Application.Validations;
using SiroccoDemo.Domain.DTOs;
using SiroccoDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiroccoDemo.Infrastructure.Services
{
    public class CreateAccountWithContactsAndNotesService : ICreateAccountWithContactsAndNotesService
    {
        private readonly ICrmRepository _crmRepository;
        private readonly IOrganizationService _organizationService;
        private readonly IAccountValidator _accountValidator;
        private readonly IContactValidator _contactValidator;
        private readonly INoteValidator _noteValidator;

        public CreateAccountWithContactsAndNotesService
            (
            ICrmRepository crmRepository,
            IOrganizationService organizationService,
            IAccountValidator accountValidator,
            IContactValidator contactValidator,
            INoteValidator noteValidator
            )
        {
            _crmRepository = crmRepository;
            _organizationService = organizationService;
            _accountValidator = accountValidator;
            _contactValidator = contactValidator;
            _noteValidator = noteValidator;
        }

        public CreateAccountWithContactsAndNotesDTO CreateAccountWithContactsAndNotes(CreateAccountWithContactsAndNotesModel model)
        {
            _accountValidator.Validate(model.Account);

            if (model.PrimaryContact == null && model.PrimaryContactNotes != null && model.PrimaryContactNotes.Length > 0)
            {
                throw new ArgumentException("Cannot create primary contact notes without providing a primary contact.");
            }

            if (model.PrimaryContact != null)
            {
                _contactValidator.Validate(model.PrimaryContact);
            }

            if (model.SecondaryContacts != null)
            {
                foreach (var contactInput in model.SecondaryContacts)
                {
                    _contactValidator.Validate(contactInput);
                }
            }

            if (model.PrimaryContactNotes != null)
            {
                foreach (var noteInput in model.PrimaryContactNotes)
                {
                    _noteValidator.Validate(noteInput);
                }
            }

            if (model.AccountNotes != null)
            {
                foreach (var noteInput in model.AccountNotes)
                {
                    _noteValidator.Validate(noteInput);
                }
            }

            try
            {
                return ExecuteInTransaction(model);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Transaction failed during account creation process.", ex);
            }
        }

        private CreateAccountWithContactsAndNotesDTO ExecuteInTransaction(CreateAccountWithContactsAndNotesModel model)
        {
            var requests = new List<OrganizationRequest>();
            var accountId = Guid.NewGuid();
            var primaryContactId = Guid.Empty;
            var secondaryContactIds = new List<Guid>();
            var primaryContactNoteIds = new List<Guid>();
            var accountNoteIds = new List<Guid>();

            var accountEntity = new Entity("account", accountId)
            {
                ["name"] = model.Account.Name
            };
            requests.Add(new CreateRequest { Target = accountEntity });

            if (model.PrimaryContact != null)
            {
                primaryContactId = Guid.NewGuid();
                var primaryContactEntity = new Entity("contact", primaryContactId)
                {
                    ["firstname"] = model.PrimaryContact.FirstName,
                    ["lastname"] = model.PrimaryContact.LastName,
                    ["emailaddress1"] = model.PrimaryContact.Email,
                    ["parentcustomerid"] = new EntityReference("account", accountId)
                };
                requests.Add(new CreateRequest { Target = primaryContactEntity });

                var updateAccountEntity = new Entity("account", accountId)
                {
                    ["primarycontactid"] = new EntityReference("contact", primaryContactId)
                };
                requests.Add(new UpdateRequest { Target = updateAccountEntity });
            }

            if (model.SecondaryContacts != null)
            {
                foreach (var contactInput in model.SecondaryContacts)
                {
                    var secondaryContactId = Guid.NewGuid();
                    var secondaryContactEntity = new Entity("contact", secondaryContactId)
                    {
                        ["firstname"] = contactInput.FirstName,
                        ["lastname"] = contactInput.LastName,
                        ["emailaddress1"] = contactInput.Email,
                        ["parentcustomerid"] = new EntityReference("account", accountId)
                    };
                    requests.Add(new CreateRequest { Target = secondaryContactEntity });
                    secondaryContactIds.Add(secondaryContactId);
                }
            }

            if (model.PrimaryContact != null && model.PrimaryContactNotes != null)
            {
                foreach (var noteInput in model.PrimaryContactNotes)
                {
                    var noteId = Guid.NewGuid();
                    var noteEntity = new Entity("annotation", noteId)
                    {
                        ["subject"] = noteInput.Title,
                        ["notetext"] = noteInput.Description,
                        ["objectid"] = new EntityReference("contact", primaryContactId)
                    };
                    requests.Add(new CreateRequest { Target = noteEntity });
                    primaryContactNoteIds.Add(noteId);
                }
            }

            if (model.AccountNotes != null)
            {
                foreach (var noteInput in model.AccountNotes)
                {
                    var noteId = Guid.NewGuid();
                    var noteEntity = new Entity("annotation", noteId)
                    {
                        ["subject"] = noteInput.Title,
                        ["notetext"] = noteInput.Description,
                        ["objectid"] = new EntityReference("account", accountId)
                    };
                    requests.Add(new CreateRequest { Target = noteEntity });
                    accountNoteIds.Add(noteId);
                }
            }

            var transactionRequest = new ExecuteTransactionRequest
            {
                Requests = new OrganizationRequestCollection()
            };

            foreach (var request in requests)
            {
                transactionRequest.Requests.Add(request);
            }

            var transactionResponse = (ExecuteTransactionResponse)_organizationService.Execute(transactionRequest);

            return new CreateAccountWithContactsAndNotesDTO
            {
                Account = accountId,
                PrimaryContact = model.PrimaryContact != null ? primaryContactId : (Guid?)null,
                SecondaryContacts = secondaryContactIds.ToArray(),
                PrimaryContactNotes = primaryContactNoteIds.ToArray(),
                AccountNotes = accountNoteIds.ToArray()
            };
        }
    }
}

