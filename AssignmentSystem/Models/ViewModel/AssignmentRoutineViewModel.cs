using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class AssignmentRoutineViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Assignment Name")]
        public string Assignment_Name { get; set; }
        [Required(ErrorMessage = "Select Subject Teacher")]
        public int Teacher_Id { get; set; }
        public string Teacher_Name { get; set; }
        [Required(ErrorMessage = "Select Release Date")]
        public DateTime? Assignment_Release_Date { get; set; }
        [Required(ErrorMessage = "Select Assignmnet Deadline ")]
        public DateTime? Deadline { get; set; }
        [Required(ErrorMessage = "Select Section ")]
        public int Section_Id { get; set; }
        public string Section_Name { get; set; }
        [Required(ErrorMessage = "Select Faculty")]
        public int Faculty_Id { get; set; }
        public string Faculty_Name { get; set; }
        [Required(ErrorMessage = "Select Semester ")]
        public int Semester_Id { get; set; }
        public string Semester_Name { get; set; }
        [Required(ErrorMessage = "Select Year Batch ")]
        public int? Year_Batch_Id { get; set; }
        public string Year_Batch { get; set; }
        public int? SubmittedStatus { get; set; }
        public int? CheckedStatus { get; set; }
        public int? FeedbackStatus { get; set; }
        public int? Marking { get; set; }
        public int? AssignmentLoaction { get; set; }

        public string Name { get; set; }
        public string ContentType { get; set; }

        //[Required(ErrorMessage = "Select File")]
        // public byte[] Data { get; set; }
        //[Display(Name = "Data")]
        // public HttpPostedFileBase postedFile { get; set; }
        // public varbinary(Max) File { get; set; }
        public byte[] Data { get; set; }

    }
   
}