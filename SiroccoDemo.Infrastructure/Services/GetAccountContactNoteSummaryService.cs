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
            var summaryData = _crmRepository.GetAccountContactNoteSummaryData();

            var accountGroups = summaryData.GroupBy(x => x.AccountId);

            var returnDTO = new List<GetAccountContactNoteSummaryDTO>();

            foreach (var accountGroup in accountGroups)
            {
                var itemDTO = new GetAccountContactNoteSummaryDTO
                {
                    AccountName = accountGroup.First().AccountName
                };

                var accountHasContacts = accountGroup.Any(x => x.HasContact);

                if (accountHasContacts)
                {
                    var contactGroups = accountGroup.Where(x => x.HasContact).GroupBy(x => x.ContactId);

                    foreach (var contactGroup in contactGroups)
                    {
                        var contactData = contactGroup.First();

                        if (contactData.IsPrimaryContact)
                        {
                            itemDTO.PrimaryContact = new PrimaryContactWithNotes
                            {
                                FirstName = contactData.ContactFirstName,
                                LastName = contactData.ContactLastName,
                                Email = contactData.ContactEmail,
                                Notes = contactGroup
                                    .Where(x => !string.IsNullOrEmpty(x.NoteDescription))
                                    .Select(x => x.NoteDescription)
                                    .ToList()
                            };
                        }
                        else
                        {
                            itemDTO.SecondaryContacts.Add(new SecondaryContact
                            {
                                FirstName = contactData.ContactFirstName,
                                LastName = contactData.ContactLastName,
                                Email = contactData.ContactEmail
                            });
                        }
                    }
                }

                returnDTO.Add(itemDTO);
            }

            return returnDTO;
        }
    }
}
