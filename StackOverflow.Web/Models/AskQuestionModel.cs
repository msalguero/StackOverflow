using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.Models
{
    public class AskQuestionModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}