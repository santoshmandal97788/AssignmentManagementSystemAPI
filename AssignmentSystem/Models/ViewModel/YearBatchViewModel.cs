using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class YearBatchViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Year/batch")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Invalid Date")]
        public string Year_Batch { get; set; }
    }
}