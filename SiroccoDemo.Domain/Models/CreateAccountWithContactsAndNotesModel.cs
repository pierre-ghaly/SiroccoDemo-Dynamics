namespace SiroccoDemo.Application.Models
{
    public class CreateAccountWithContactsAndNotesModel
    {
        public AccountInput Account { get; set; }
        public ContactInput PrimaryContact { get; set; }
        public ContactInput[] SecondaryContacts { get; set; }
        public NoteInput[] PrimaryContactNotes { get; set; }
        public NoteInput[] AccountNotes { get; set; }
    }
}
