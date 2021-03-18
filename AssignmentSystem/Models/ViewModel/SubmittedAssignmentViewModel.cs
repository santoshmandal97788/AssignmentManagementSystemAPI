using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class SubmittedAssignmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Select Assignment Name")]
        public int? Routine_Id { get; set; }
        //[Required(ErrorMessage = "Please Select Assignment Name")]
        public string Assignment_Name { get; set; }
        public string TeacherName { get; set; }
        public int Section_Id { get; set; }
        public string SectionName { get; set; }
        public int Faculty_Id { get; set; }
        public string FacultyName { get; set; }
        public int Semester_Id { get; set; }
        public string SemesterName { get; set; }
        public int Year_Batch_Id { get; set; }
        public string YearBatch { get; set; }
        [Required(ErrorMessage = "Please Select Student Name")]
        public int? Student_Id { get; set; }
        //[Required(ErrorMessage = "Please Select Student Name")]
        public string StudentName { get; set; }
        
        public DateTime? Submitted_Date { get; set; }
        //[Required(ErrorMessage = "Please Choose Checked Status")]
        public int? Checked_Status { get; set; }
        //[Required(ErrorMessage = "Please Choose Feedback Status")]
        public int? Feedback_Status { get; set; }
        //[Required(ErrorMessage = "Please Choose Marking")]
        public int? Marking { get; set; }
        //[Required(ErrorMessage = "Please Choose Assignment Location")]
        public int? Assignmnet_Location { get; set; }
        public int onTime { get; set; }
    }
}