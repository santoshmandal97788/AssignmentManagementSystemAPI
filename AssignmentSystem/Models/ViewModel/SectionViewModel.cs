using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class SectionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Section Name")]
        public string Sec_Name { get; set; }
    }
}