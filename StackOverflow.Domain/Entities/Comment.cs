using System;

namespace StackOverflow.Domain.Entities
{
    public class Comment : IEntity
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public virtual Account Owner { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }

        public Comment()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
    }
}