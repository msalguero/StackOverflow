using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace StackOverflow.Web.Models
{
    public class AccountRegisterModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}