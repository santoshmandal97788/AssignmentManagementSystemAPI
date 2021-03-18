using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class StudentRoutineViewModel
    {
        public int ID { get; set; }
        public int StudentId { get; set; }
        public int RoutineId { get; set; }


        public string Assignment_Name { get; set; }
      
        public int Teacher_Id { get; set; }
        public string Teacher_Name { get; set; }
 
        public string Assignment_Release_Date { get; set; }
    
        public string Deadline { get; set; }
     
        public int Section_Id { get; set; }
        public string Section_Name { get; set; }
        
        public int Faculty_Id { get; set; }
        public string Faculty_Name { get; set; }
      
        public int Semester_Id { get; set; }
        public string Semester_Name { get; set; }
      
        public int? Year_Batch_Id { get; set; }
        public string Year_Batch { get; set; }
        public int RemainingDays { get; set; }

    }
}