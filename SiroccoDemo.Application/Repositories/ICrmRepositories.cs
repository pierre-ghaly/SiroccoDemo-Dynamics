using SiroccoDemo.Domain.Entities;
using System;

namespace SiroccoDemo.Application.Repositories
{
    public interface ICrmRepository
    {
        Guid CreateAccount(Account account);
        Guid CreateContact(Contact contact);
        Guid CreateNote(Note note);
        void SetPrimaryContact(Guid accountId, Guid contactId);
    }
}
