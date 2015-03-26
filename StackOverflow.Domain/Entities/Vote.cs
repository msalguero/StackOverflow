using System;

namespace StackOverflow.Domain.Entities
{
    public class Vote : IEntity
    {
        public Guid Id { get; set; }
        public virtual Account Voter { get; set; }
        public int Value;
        public virtual Question Question { get; set; }
        public virtual Answer Answer { get; set; }

        public Vote()
        {
            Id = Guid.NewGuid();
        }
    }
}