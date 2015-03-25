using System;

namespace StackOverflow.Domain.Entities
{
    public class Vote : IEntity
    {
        public Guid Id { get; set; }
        public Account Voter { get; set; }
        public int Value;
        public Question Question { get; set; }
        public Answer Answer { get; set; }

        public Vote()
        {
            Id = Guid.NewGuid();
        }
    }
}