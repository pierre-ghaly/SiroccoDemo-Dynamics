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
}
