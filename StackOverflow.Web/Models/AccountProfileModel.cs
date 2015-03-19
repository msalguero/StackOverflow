using System;
using System.Collections.Generic;

namespace StackOverflow.Web.Models
{
    public class AccountProfileModel 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int QuestionsAsked { get; set; }
        public int Answers { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public IEnumerable<QuestionListModel> QuestionList { get; set; }
        public IEnumerable<AnswerModel> AnswerList { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastProfileViewDate { get; set; }
        public int ProfileViews { get; set; }
    }
}