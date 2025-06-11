using SiroccoDemo.Application.Repositories;
using SiroccoDemo.Application.Services;
using SiroccoDemo.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace SiroccoDemo.Infrastructure.Services
{
    public class GetAccountContactNoteSummaryService : IGetAccountContactNoteSummaryService
    {
        private readonly ICrmRepository _crmRepository;

        public GetAccountContactNoteSummaryService(ICrmRepository crmRepository)
        {
            _crmRepository = crmRepository;
        }

        public List<GetAccountContactNoteSummaryDTO> GetAccountContactNoteSummary()
        {
            var returnDTO = new List<GetAccountContactNoteSummaryDTO>();

            var accountsAndPrimaryContact = _crmRepository.GetAllAccountsAndPrimaryContact();

            foreach (var (account, primaryContactId) in accountsAndPrimaryContact)
            {
                var itemDTO = new GetAccountContactNoteSummaryDTO
                {
                    AccountName = account.Name
                };

                var contacts = _crmRepository.GetContactsByAccountId(account.Id);

                foreach (var contact in contacts)
                {
                    if (primaryContactId.HasValue && contact.Id == primaryContactId.Value)
                    {
                        var notes = _crmRepository.GetNotesByRegarding(contact.Id);
                        itemDTO.PrimaryContact = new PrimaryContactWithNotes
                        {
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Email = contact.Email,
                            Notes = notes.Select(n => n.Description).ToList()
                        };
                    }
                    else
                    {
                        itemDTO.SecondaryContacts.Add(new SecondaryContact
                        {
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Email = contact.Email
                        });
                    }
                }
                returnDTO.Add(itemDTO);
            }

            return returnDTO;
        }
    }
}
