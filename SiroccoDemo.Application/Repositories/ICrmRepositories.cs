using SiroccoDemo.Domain.DTOs;
using SiroccoDemo.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SiroccoDemo.Application.Repositories
{
    public interface ICrmRepository
    {
        Guid CreateAccount(Account account);
        Guid CreateContact(Contact contact);
        Guid CreateNote(Note note);
        void SetPrimaryContact(Guid accountId, Guid contactId);
        List<(Account Account, Guid? PrimaryContactId)> GetAllAccountsAndPrimaryContact();
        List<Contact> GetContactsByAccountId(Guid accountId);
        List<Note> GetNotesByRegarding(Guid regardingId);
        List<AccountContactSummaryDataDTO> GetAccountContactNoteSummaryData();
    }
}
