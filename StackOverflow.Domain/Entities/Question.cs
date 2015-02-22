using System;
using System.Collections;
using System.Collections.Generic;

namespace StackOverflow.Domain.Entities
{
    public class Question : IEntity
    {
        public Guid Id { get; private set; }
        public int Votes { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public virtual Account Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public bool IsAnswered { get; set; }

        public Question()
        {
            Id = Guid.NewGuid();
        }
    }
}