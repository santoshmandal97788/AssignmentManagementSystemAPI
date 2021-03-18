using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage ="Old Password is Required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage ="New Password is Required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="Confirm New Password is Required")]
        [Compare("NewPassword", ErrorMessage = "Password Mismatch")]
        public string ConfirmNewPassword { get; set; }
    }
}