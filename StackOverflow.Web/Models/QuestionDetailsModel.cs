using System;
using System.Collections.Generic;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Web.Models
{
    public class QuestionDetailsModel
    {
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public int Votes { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
        public Guid Id { get; set; }

        public ICollection<AnswerModel> Answers { get; set; }
        public bool IsAnswered { get; set; }
        public int Views { get; set; }
    }
}