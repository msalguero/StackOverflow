using System.ComponentModel.DataAnnotations;
using StackOverflow.Web.Controllers;

namespace StackOverflow.Web.Models
{
    public class AskQuestionModel
    {
        [Required]
        [Maximum(50)]
        [MinimumWords(3)]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [MinimumWords(5)]
        public string Description { get; set; }
    }
}