using System;
using System.Collections.Generic;

namespace StackOverflow.Domain.Entities
{
    public class Answer : IEntity
    {
        public Guid Id { get; private set; }
        public int Votes { get; set; }
        public string Description { get; set; }
        public virtual Account Owner { get; set; }
        public Question Question { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Correct { get; set; }
        public virtual ICollection<Vote> Voters { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } 

        public Answer()
        {
            Id = Guid.NewGuid();
        }
    }
}