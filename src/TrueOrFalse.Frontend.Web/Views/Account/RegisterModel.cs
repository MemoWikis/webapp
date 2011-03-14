using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Frontend.Web.Models 
{
    public class RegisterModel : ModelBase
    {
        [Required]
        [DisplayName("Benutzername")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

    }
}