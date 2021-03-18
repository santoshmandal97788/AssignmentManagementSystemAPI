//using AssignmentSystem.Filters;
using AssignmentSystem.Models;
using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace AssignmentSystem.Controllers
{
//    [AuthorizeAttribute]
//    [Authorize(Roles = "admin")]
    public class SubmittedAssignmentController : ApiController
    {
        SubmittedAssignmentDB subDB = new SubmittedAssignmentDB();
        [System.Web.Http.Route("api/SubmittedAssignment/List")]
        public List<SubmittedAssignmentViewModel> Get()
        {
            return subDB.ListAll();
        }
        public JsonResult GetStudentSubmittedAssignmentList(string sortColumnName = "Routine_Id", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "", int? routineid=0 )
        {
            List<SubmittedAssignmentViewModel> List = new List<SubmittedAssignmentViewModel>();
            int totalPage = 0;
            int totalRecord = 0;

            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                var emp = subDB.ListAll().Select(a => a);
               // var emp = subDB.ListAll().Where(e => e.Routine_Id == routineid);
                if (routineid==null)
                {
                     emp = subDB.ListAll().Select(a=>a);
                }
                else
                {
                     emp = subDB.ListAll().Select(a => a).Where(e => e.Routine_Id == routineid);
                }
              


                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(a => a.Assignment_Name.Contains(searchText) ||a.TeacherName.Contains(searchText) || a.StudentName.Contains(searchText) || a.FacultyName.Contains(searchText) || a.SectionName.Contains(searchText) || a.SemesterName.Contains(searchText) || a.YearBatch.Contains(searchText) || a.Submitted_Date.ToString().Contains(searchText) || a.Checked_Status.ToString().Contains(searchText) || a.Feedback_Status.ToString().Contains(searchText) || a.Marking.ToString().Contains(searchText));
                }
                totalRecord = emp.Count();
                if (pageSize > 0)
                {
                    totalPage = totalRecord / pageSize + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    List = emp.OrderBy(sortColumnName + " " + sortOrder).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                }
                else
                {
                    List = emp.ToList();
                }
            }

            return new JsonResult
            {
                //Data = new { List = List, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage},
                Data = new { List = List, totalRecord = totalRecord, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage, pageSize = pageSize },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public SubmittedAssignmentViewModel Get(int ID)
        {
            var subassign = subDB.ListAll().Find(a => a.Id.Equals(ID));
            return subassign;
        }
        //List Student Submitted Assignment By StudentId
        public List<SubmittedAssignmentViewModel> GetAssignmentByStudentId(int studentid)
        {
            return subDB.ListAllByStudent(studentid);
        }
        //Log List of Assignment by Assignment 
        [System.Web.Http.Route("api/AssignmentLog")]
        public List<LogViewModel> GetLogByAssignment(int assignmentid)
        {
            return subDB.LogListByAssignmentId(assignmentid);
        }

        public HttpResponseMessage Post([Bind(Include = "Routine_Id,Model")]SubmittedAssignmentViewModel sub)
        {
            string message = "Assignment Submitted Successfully";
            if (ModelState.IsValid)
            {
                bool isexists = subDB.StudentAssignmentMatch(sub.Student_Id, sub.Routine_Id);
                if (!isexists)
                {
                    ModelState.AddModelError("Routine_Id", "This Assignment is not Assigned to selected Student");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    subDB.Add(sub);
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        public HttpResponseMessage Put([Bind(Include = "Routine_Id,Model")]SubmittedAssignmentViewModel sub)
        {
            AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
            string message = "Assignment Updated Successfully";
            if (ModelState.IsValid)
            {
                bool isexists = subDB.StudentAssignmentMatch(sub.Student_Id, sub.Routine_Id);
                if (!isexists)
                {
                    ModelState.AddModelError("Routine_Id", "This Assignment is not Assigned to selected Student");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    tblSubmittedAssignment tb = _db.tblSubmittedAssignments.Where(a => a.ID == sub.Id).FirstOrDefault();
                    if (tb.Assignmnet_Location == 1 && sub.Assignmnet_Location == 2)
                    {
                        ModelState.AddModelError("Routine_Id", "This Assignment can not be directly handover to student from teacher Location");
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                    else if (tb.Assignmnet_Location == 2 && sub.Assignmnet_Location == 1)
                    {
                        ModelState.AddModelError("Routine_Id", "The Assignment can not be directly handover to Teacher  from Student Location");
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                    else
                    {
                        subDB.Update(sub);
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }

                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        public HttpResponseMessage Delete(int ID)
        {
            var subassign = subDB.ListAll().Find(a => a.Id.Equals(ID));
            string message = "Assignment Deleted Successfully";
            string messagenodata = "Data Not found/ Might be Deleted or Removed";
            HttpResponseMessage response;
            if (subassign == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messagenodata);
            }
            else
            {
                subDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.OK, message);
            }
            return response;

           
        }

    }
}
