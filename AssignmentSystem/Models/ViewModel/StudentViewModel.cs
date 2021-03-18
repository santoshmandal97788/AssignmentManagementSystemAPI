using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssignmentSystem.Models.ViewModel
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Student Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Student Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Remote("EmailExists", "StudentDB", HttpMethod = "POST", ErrorMessage = "Email address already registered.")]
       
        public string Email { get; set; }
        //[Required(ErrorMessage = "Enter Student Name")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Select Student Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Enter Student Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Enter Student Address")]
        public string Address { get; set; }
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
   
        public int ToSemesterId { get; set; }
        public int ToSectionId { get; set; }
        public int? SubmittedStatus { get; set; }
        public int? FeedbackStatus { get; set; }
        public int? Marking { get; set; }
       
    }
}