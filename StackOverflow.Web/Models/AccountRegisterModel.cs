using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using StackOverflow.Web.Controllers;

namespace StackOverflow.Web.Models
{
    public class AccountRegisterModel
    {
        [Required]
        [Maximum(50)]
        [Minimum(2)]
        public string Name { get; set; }
        [Required]
        [Maximum(50)]
        [Minimum(2)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        /*
        [Capital]
        [Minimum(8)]
        [Maximum(16)]
        [Vocal]
        [Number]
        [RepeatedChar]
        [ExcludeChar("/.,!@#$%")]
          */
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        /*
        [Capital]
        [Minimum(8)]
        [Maximum(16)]
        [Vocal]
        [Number]
        [RepeatedChar]
        [ExcludeChar("/.,!@#$%")]
        */
        public string ConfirmPassword { get; set; }
    }
}