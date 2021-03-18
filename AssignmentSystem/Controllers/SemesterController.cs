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
    [AuthorizeAttribute]
    [Authorize(Roles = "admin")]
    public class SemesterController : ApiController
    {
        SemesterDB semDB = new SemesterDB();
        [System.Web.Http.Route("api/Semester/List")]
        public List<SemesterViewModel> Get()
        {
            return semDB.ListAll();
        }
        public JsonResult GetSemesterList(string sortColumnName = "Semester_Name", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<SemesterViewModel> List = new List<SemesterViewModel>();
            int totalPage = 0;
            int totalRecord = 0;


            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                var emp = semDB.ListAll().Select(a => a);
                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(x => x.Semester_Name.Contains(searchText));
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

        public SemesterViewModel Get(int ID)
        {
            var semester = semDB.ListAll().Find(s => s.Id.Equals(ID));
            return semester;
        }

        public HttpResponseMessage Post([Bind(Include = "Semester_Name,Model")]SemesterViewModel svm)
        {
            //AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            string message = "New Semester Added Successfully";
            bool isexists = semDB.CheckSemesterExist(svm.Semester_Name);
            if (isexists)
            {
                ModelState.AddModelError("Semester_Name", "SemesterName Already Exist");
            }
            if (ModelState.IsValid)
            {
                semDB.Add(svm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public HttpResponseMessage Put([Bind(Include = "Semester_Name,Model")]SemesterViewModel svm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            var semester = semDB.ListAll().Find(s => s.Id.Equals(svm.Id));
            string message = "Semester Updated Successfully";
            tblSemester tb = db.tblSemesters.Where(s => s.Semester_Id == svm.Id).FirstOrDefault();
            bool isexists = semDB.CheckSemesterExist(svm.Semester_Name);
            if (isexists)
            {
                if (tb.Semester_Name == svm.Semester_Name)
                {
                    if (ModelState.IsValid)
                    {
                        semDB.Update(svm);
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("Semester_Name", "Semester Name Already Exist");
                }
            }

            if (ModelState.IsValid)
            {
                semDB.Update(svm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }
        public HttpResponseMessage Delete(int ID)
        {
            var semester = semDB.ListAll().Find(s => s.Id.Equals(ID));
            string message = "Semester Deleted Successfully";
            string messagenodata = "Data Not found/ Might be Deleted or Removed";
            HttpResponseMessage response;
            if (semester == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messagenodata);
            }
            else
            {
                semDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.OK, message);
            }
            return response;
        }

    }
}

