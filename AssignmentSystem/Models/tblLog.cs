//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AssignmentSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblLog
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public Nullable<int> SubmittedAssignmentId { get; set; }
    
        public virtual tblActivity tblActivity { get; set; }
        public virtual tblSubmittedAssignment tblSubmittedAssignment { get; set; }
    }
}
