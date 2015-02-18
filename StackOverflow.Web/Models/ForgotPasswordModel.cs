using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}