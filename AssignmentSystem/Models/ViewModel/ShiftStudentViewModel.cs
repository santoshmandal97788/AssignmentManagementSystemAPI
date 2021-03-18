using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class ShiftStudentViewModel
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Select Student Year/Batch")]
        public int? Year_Batch_Id { get; set; }
        public string Year_Batch { get; set; }
        [Required(ErrorMessage = "Select Student Faculty")]
        public int? Faculty_Id { get; set; }
        public string Faculty_Name { get; set; }
        [Required(ErrorMessage = "Select Student Section")]
        public int? Section_Id { get; set; }
        public string Sec_Name { get; set; }
        [Required(ErrorMessage = "Select Student Semester")]
        public int? Semester_Id { get; set; }
        public string Semester_Name { get; set; }
        [Required(ErrorMessage = "Select Student Semester To Be Shifted ")]
        public int? ToSemesterId { get; set; }
        [Required(ErrorMessage = "Select Student Section To Be Shifted ")]
        public int? ToSectionId { get; set; }
    }
}