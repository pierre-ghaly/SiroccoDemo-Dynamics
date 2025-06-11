using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Repositories;
using SiroccoDemo.Application.Services;
using SiroccoDemo.Application.Validations;
using SiroccoDemo.Domain.DTOs;
using SiroccoDemo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SiroccoDemo.Infrastructure.Services
{
    public class CreateAccountWithContactsAndNotesService : ICreateAccountWithContactsAndNotesService
    {
        private readonly ICrmRepository _crmRepository;
        private readonly IAccountValidator _accountValidator;
        private readonly IContactValidator _contactValidator;
        private readonly INoteValidator _noteValidator;

        public CreateAccountWithContactsAndNotesService
            (
            ICrmRepository crmRepository,
            IAccountValidator accountValidator,
            IContactValidator contactValidator,
            INoteValidator noteValidator
            )
        {
            _crmRepository = crmRepository;
            _accountValidator = accountValidator;
            _contactValidator = contactValidator;
            _noteValidator = noteValidator;
        }

        public CreateAccountWithContactsAndNotesDTO CreateAccountWithContactsAndNotes(CreateAccountWithContactsAndNotesModel model)
        {
            _accountValidator.Validate(model.Account);
            var account = new Account
            {
                Name = model.Account.Name
            };
            account.Id = _crmRepository.CreateAccount(account);

            _contactValidator.Validate(model.PrimaryContact);
            var primaryContact = new Contact
            {
                FirstName = model.PrimaryContact.FirstName,
                LastName = model.PrimaryContact.LastName,
                Email = model.PrimaryContact.Email,
                AccountId = account.Id
            };
            primaryContact.Id = _crmRepository.CreateContact(primaryContact);

            _crmRepository.SetPrimaryContact(account.Id, primaryContact.Id);

            var secondaryContactIds = new List<Guid>();
            foreach (var contactInput in model.SecondaryContacts)
            {
                _contactValidator.Validate(contactInput);
                var secondaryContact = new Contact
                {
                    FirstName = contactInput.FirstName,
                    LastName = contactInput.LastName,
                    Email = contactInput.Email,
                    AccountId = account.Id
                };
                secondaryContact.Id = _crmRepository.CreateContact(secondaryContact);
                secondaryContactIds.Add(secondaryContact.Id);
            }

            var primaryContactNoteIds = new List<Guid>();
            foreach (var noteInput in model.PrimaryContactNotes)
            {
                _noteValidator.Validate(noteInput);
                var note = new Note
                {
                    Title = noteInput.Title,
                    Description = noteInput.Description,
                    RegardingEntityName = "contact",
                    RegardingId = primaryContact.Id,
                };
                note.Id = _crmRepository.CreateNote(note);
                primaryContactNoteIds.Add(note.Id);
            }

            var accountNoteIds = new List<Guid>();
            foreach (var noteInput in model.AccountNotes)
            {
                _noteValidator.Validate(noteInput);
                var note = new Note
                {
                    Title = noteInput.Title,
                    Description = noteInput.Description,
                    RegardingEntityName = "account",
                    RegardingId = account.Id,
                };
                note.Id = _crmRepository.CreateNote(note);
                accountNoteIds.Add(note.Id);
            }

            return new CreateAccountWithContactsAndNotesDTO
            {
                Account = account.Id,
                PrimaryContact = primaryContact.Id,
                SecondaryContacts = secondaryContactIds.ToArray(),
                PrimaryContactNotes = primaryContactNoteIds.ToArray(),
                AccountNotes = accountNoteIds.ToArray()
            };
        }
    }
}
