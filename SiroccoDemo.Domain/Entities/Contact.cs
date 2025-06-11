using System;

namespace SiroccoDemo.Domain.Entities
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid AccountId { get; set; }
    }
}
