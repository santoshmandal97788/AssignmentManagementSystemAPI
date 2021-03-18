using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models.ViewModel
{
    public class Admin
    {
        public int Admin_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? Role { get; set; }
    }
}