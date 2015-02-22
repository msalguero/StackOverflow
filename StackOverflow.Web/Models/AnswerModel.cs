﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class AnswerModel
    {
        public string OwnerName { get; set; }
        public int Votes { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
        public Guid Id { get;  set; }
        public bool Correct { get; set; }
        public Guid QuestionId { get; set; }
    }
}