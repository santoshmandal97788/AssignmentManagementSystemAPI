using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Teacher Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Teacher Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter Teacher Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Enter Teacher Address")]
        public string Address { get; set; }
    }
}