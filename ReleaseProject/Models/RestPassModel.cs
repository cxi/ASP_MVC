using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReleaseProject.Models
{
    public class RestPassModel
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "LoginEmail: ")]
        public string UserEmail { get; set; }
    }
}