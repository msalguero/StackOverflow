using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StackOverflow.Web.Controllers;

namespace StackOverflow.Web.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Capital]
        [Minimum(8)]
        [Maximum(16)]
        [Vocal]
        [Number]
        [RepeatedChar]
        [ExcludeChar("/.,!@#$%")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Capital]
        [Minimum(8)]
        [Maximum(16)]
        [Vocal]
        [Number]
        [RepeatedChar]
        [ExcludeChar("/.,!@#$%")]
        public string ConfirmPassword { get; set; }
        public Guid Id { get; set; }
    }
}