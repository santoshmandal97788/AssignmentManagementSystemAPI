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

    public class PassStudentController : ApiController
    {
        TotalCount tc = new TotalCount();
        [System.Web.Http.Route("api/Student/Marks")]
      
        public JsonResult GetStudentMarking(int? year, int? faculty, int? semester, int? section)
        {
            //int totalPass = 0;
            //int totalMerit = 0;
            //int totalDistinction = 0;
            //int totalFail = 0;

             int totalPass = Convert.ToInt32(tc.GetTotalPassStudent(year, faculty, semester, section));
           int totalMerit = Convert.ToInt32(tc.GetTotalMeritStudent(year, faculty, semester, section));
           int totalDistinction = Convert.ToInt32(tc.GetTotalDistictionStudent(year, faculty, semester, section));
           int totalFail = Convert.ToInt32(tc.GetTotalFailStudent(year, faculty, semester, section));
            
            return new JsonResult
            {
                Data = new { totalPass = totalPass, totalMerit = totalMerit, totalDistinction = totalDistinction, totalFail = totalFail },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        [System.Web.Http.Route("api/Student/byfaculty")]
        public JsonResult GetStudentbyFaculty(int? yearid)
        {
            AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
            var tblFaculty = _db.tblFaculties.ToList();
            Dictionary<string, int> data = new Dictionary<string, int>();
            foreach (var items in tblFaculty)
            {
                var total = 0;
                if (yearid != null)
                {
                    total = _db.tblStudents.Where(s => s.Faculty_Id == items.Faculty_Id && s.YearBatchId == yearid).Count();

                }
                else
                {
                    total = _db.tblStudents.Where(s => s.Faculty_Id == items.Faculty_Id).Count();
                }
                data.Add(items.Faculty_Name, total);
               
            }
            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        [System.Web.Http.Route("api/Student/byyear")]
        public JsonResult GetStudentbyYear(int? facultyid)
        {
            AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
            var tblYearBatch = _db.tblYearBatches.ToList();
            Dictionary<string, int> data = new Dictionary<string, int>();
            foreach (var items in tblYearBatch)
            {
                var total = 0;
                if (facultyid != null)
                {
                    total = _db.tblStudents.Where(s => s.YearBatchId == items.YearBatchId && s.Faculty_Id == facultyid).Count();

                }
                else
                {
                    total = _db.tblStudents.Where(s => s.YearBatchId == items.YearBatchId).Count();
                }
                data.Add(items.Year_Batch, total);

            }
            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

    }
}
