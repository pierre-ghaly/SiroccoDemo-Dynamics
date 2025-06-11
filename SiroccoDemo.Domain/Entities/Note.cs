using System;

namespace SiroccoDemo.Domain.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RegardingEntityName { get; set; }
        public Guid RegardingId { get; set; }
    }
}
