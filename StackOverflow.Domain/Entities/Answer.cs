using System;

namespace StackOverflow.Domain.Entities
{
    public class Answer : IEntity
    {
        public Guid Id { get; private set; }
        public int Votes { get; set; }
        public string Description { get; set; }
        public Account Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Correct { get; set; }

        public Answer()
        {
            Id = Guid.NewGuid();
        }
    }
}