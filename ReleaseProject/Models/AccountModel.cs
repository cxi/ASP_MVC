using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReleaseProject.Models
{
    public class AccountModel
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "LoginEmail: ")]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(200)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
    }
}