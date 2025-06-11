using System;

namespace SiroccoDemo.Domain.DTOs
{
    public class CreateAccountWithContactsAndNotesDTO
    {
        public Guid Account { get; set; }
        public Guid PrimaryContact { get; set; }
        public Guid[] SecondaryContacts { get; set; }
        public Guid[] PrimaryContactNotes { get; set; }
        public Guid[] AccountNotes { get; set; }
    }
}
