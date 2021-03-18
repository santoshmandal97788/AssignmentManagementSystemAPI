using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class SemesterViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Semester Name")]
        public string Semester_Name { get; set; }
    }
}