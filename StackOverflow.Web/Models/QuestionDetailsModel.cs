using System;

namespace StackOverflow.Web.Models
{
    public class QuestionDetailsModel
    {
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public int Votes { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
    }
}