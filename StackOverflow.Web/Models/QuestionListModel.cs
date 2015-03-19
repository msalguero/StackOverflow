using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class QuestionListModel
    {
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public int Votes { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid OwnerId { get; set; }
        public Guid QuestionId { get; set; }
        public bool IsAnswered { get; set; }
        public int AnswersCount { get; set; }
        public int Views { get; set; }
    }
}