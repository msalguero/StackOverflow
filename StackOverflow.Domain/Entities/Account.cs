﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace StackOverflow.Domain.Entities
{
    public class Account : IEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set;}
        public int QuestionsAsked { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; } 

        public Account()
        {
            Id = Guid.NewGuid();
        }
    }
}