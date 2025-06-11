using System;

namespace SiroccoDemo.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? PrimaryContactId { get; set; }
    }
}
