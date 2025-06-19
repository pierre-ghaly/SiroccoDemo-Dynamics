using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SiroccoDemo.Application.Repositories;
using SiroccoDemo.Domain.DTOs;
using SiroccoDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiroccoDemo.Infrastructure.Repositories
{
    public class CrmRepository : ICrmRepository
    {
        private readonly IOrganizationService _service;
        public CrmRepository(IOrganizationService service) => _service = service;

        private T GetAliasedValue<T>(Entity entity, string alias, string attributeName)
        {
            var key = $"{alias}.{attributeName}";
            if (!entity.Contains(key))
                return default;

            var aliasedValue = entity[key] as AliasedValue;
            if (aliasedValue?.Value == null)
                return default;

            if (typeof(T) == typeof(Guid?) && aliasedValue.Value is Guid guidValue)
                return (T)(object)guidValue;

            return (T)aliasedValue.Value;
        }

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

        public List<(Account Account, Guid? PrimaryContactId)> GetAllAccountsAndPrimaryContact()
        {
            var query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("name", "primarycontactid")
            };

            var result = _service.RetrieveMultiple(query);
            return result.Entities.Select(e =>
            {
                var account = new Account
                {
                    Id = e.Id,
                    Name = e.GetAttributeValue<string>("name")
                };

                Guid? primaryContactId = null;
                if (e.Contains("primarycontactid"))
                {
                    var primaryContactRef = e.GetAttributeValue<EntityReference>("primarycontactid");
                    primaryContactId = primaryContactRef?.Id;
                }

                return (Account: account, PrimaryContactId: primaryContactId);
            }).ToList();
        }

        public List<Contact> GetContactsByAccountId(Guid accountId)
        {
            var query = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet("firstname", "lastname", "emailaddress1"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("parentcustomerid", ConditionOperator.Equal, accountId)
                    }
                }
            };

            var result = _service.RetrieveMultiple(query);
            return result.Entities.Select(e => new Contact
            {
                Id = e.Id,
                FirstName = e.GetAttributeValue<string>("firstname"),
                LastName = e.GetAttributeValue<string>("lastname"),
                Email = e.GetAttributeValue<string>("emailaddress1"),
                AccountId = accountId
            }).ToList();
        }

        public List<Note> GetNotesByRegarding(Guid regardingId)
        {
            var query = new QueryExpression("annotation")
            {
                ColumnSet = new ColumnSet("notetext"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("objectid", ConditionOperator.Equal, regardingId),
                    }
                }
            };

            var result = _service.RetrieveMultiple(query);
            return result.Entities.Select(e => new Note
            {
                Id = e.Id,
                Description = e.GetAttributeValue<string>("notetext"),
                RegardingId = regardingId,
            }).ToList();
        }

        public List<AccountContactSummaryDataDTO> GetAccountContactNoteSummaryData()
        {
            var query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("name", "primarycontactid"),
                LinkEntities =
                {
                    new LinkEntity("account", "contact", "accountid", "parentcustomerid", JoinOperator.LeftOuter)
                    {
                        EntityAlias = "contact",
                        Columns = new ColumnSet("contactid", "firstname", "lastname", "emailaddress1"),
                        LinkEntities =
                        {
                            new LinkEntity("contact", "annotation", "contactid", "objectid", JoinOperator.LeftOuter)
                            {
                                EntityAlias = "note",
                                Columns = new ColumnSet("notetext"),
                                LinkCriteria = new FilterExpression
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression("objecttypecode", ConditionOperator.Equal, "contact")
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = _service.RetrieveMultiple(query);
            return result.Entities.Select(e =>
            {
                var accountId = e.Id;
                var accountName = e.Contains("name") 
                    ? e.GetAttributeValue<string>("name") 
                    : string.Empty;
                var primaryContactId = e.Contains("primarycontactid") 
                    ? e.GetAttributeValue<EntityReference>("primarycontactid")?.Id 
                    : null;

                var contactId = GetAliasedValue<Guid?>(e, "contact", "contactid");
                var contactFirstName = GetAliasedValue<string>(e, "contact", "firstname");
                var contactLastName = GetAliasedValue<string>(e, "contact", "lastname");
                var contactEmail = GetAliasedValue<string>(e, "contact", "emailaddress1");
                var noteDescription = GetAliasedValue<string>(e, "note", "notetext");

                return new AccountContactSummaryDataDTO
                {
                    AccountId = accountId,
                    AccountName = accountName,
                    PrimaryContactId = primaryContactId,
                    ContactId = contactId ?? Guid.Empty,
                    ContactFirstName = contactFirstName,
                    ContactLastName = contactLastName,
                    ContactEmail = contactEmail,
                    IsPrimaryContact = contactId.HasValue && primaryContactId.HasValue && primaryContactId.Value == contactId.Value,
                    NoteDescription = noteDescription,
                    HasContact = contactId.HasValue
                };
            }).ToList();
        }
    }
}
