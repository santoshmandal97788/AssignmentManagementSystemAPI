using AssignmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace AssignmentSystem.Controllers
{
    [AuthorizeAttribute]
    [Authorize(Roles = "admin")]
    public class TotalCountController : ApiController
    {
        TotalCount tc = new TotalCount();
        public JsonResult Get()
        {
            int totalStudent = 0;
            int totalTeacher = 0;
            int totalSubmittedAssignment = 0;
            int totalRoutine = 0;
            totalStudent = Convert.ToInt32(tc.GetTotalStudent());
            totalSubmittedAssignment = Convert.ToInt32(tc.GetTotalSubmittedAssignment());
            totalTeacher = Convert.ToInt32(tc.GetTotalTeacher());
            totalRoutine= Convert.ToInt32(tc.GetTotalRoutine());
            return new JsonResult
            {
                Data = new { totalStudent = totalStudent, totalTeacher = totalTeacher, totalSubmittedAssignment = totalSubmittedAssignment, totalRoutine= totalRoutine },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
           
        }
    }
}
