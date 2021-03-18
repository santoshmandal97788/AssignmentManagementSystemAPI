using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class FacultyViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Faculty Name")]
        public string Faculty_Name { get; set; }
    }
}