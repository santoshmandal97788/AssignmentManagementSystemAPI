using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Usename Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

    }
}