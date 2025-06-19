using System;
using System.Collections.Generic;

namespace SiroccoDemo.Domain.DTOs
{
    public class GetAccountContactNoteSummaryDTO
    {
        public string AccountName { get; set; }
        public PrimaryContactWithNotes PrimaryContact { get; set; }
        public List<SecondaryContact> SecondaryContacts { get; set; } = new List<SecondaryContact>();
    }

    public class PrimaryContactWithNotes
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Notes { get; set; } = new List<string>();
    }

    public class SecondaryContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class AccountContactSummaryDataDTO
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public Guid? PrimaryContactId { get; set; }
        public Guid ContactId { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public bool IsPrimaryContact { get; set; }
        public string NoteDescription { get; set; }
        public bool HasContact { get; set; }
    }
}
