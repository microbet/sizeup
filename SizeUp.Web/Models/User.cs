using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SizeUp.Core;
using SizeUp.Core.Identity;
namespace SizeUp.Web.Models
{
    public class User
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        public bool IsValid(string _email, string _password)
        {
            return Identity.ValidateUser(_email.Trim(), _password.Trim());
        }
    }
}