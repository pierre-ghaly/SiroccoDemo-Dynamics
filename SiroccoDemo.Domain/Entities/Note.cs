using System;

namespace SiroccoDemo.Domain.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        public string NoteText { get; set; }
        public Guid RegardingId { get; set; }
        public string RegardingType { get; set; }
    }
}
