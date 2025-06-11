using SiroccoDemo.Application.Models;
using SiroccoDemo.Domain.DTOs;

namespace SiroccoDemo.Application.Services
{
    public interface ICreateAccountWithContactsAndNotesService
    {
        CreateAccountWithContactsAndNotesDTO CreateAccountWithContactsAndNotes(CreateAccountWithContactsAndNotesModel model);
    }
}
