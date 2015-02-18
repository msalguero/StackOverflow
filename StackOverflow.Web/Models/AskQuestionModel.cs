using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.Models
{
    public class AskQuestionModel
    {
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}