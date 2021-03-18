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
    public class FacultyController : ApiController
    {
        FacultyDB facDB = new FacultyDB();
        [System.Web.Http.Route("api/Faculty/List")]
        public List<FacultyViewModel> Get()
        {
            return facDB.ListAll();
        }
        public JsonResult GetFacultyList(string sortColumnName = "Faculty_Name", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<FacultyViewModel> List = new List<FacultyViewModel>();
            int totalPage = 0;
            int totalRecord = 0;


            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                var emp = facDB.ListAll().Select(a => a);
                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(x => x.Faculty_Name.Contains(searchText));
                    var result = dc.tblFaculties.Where(x => x.Faculty_Name.Contains(searchText));
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


        public FacultyViewModel Get(int ID)
        {
            var faculty = facDB.ListAll().Find(f => f.Id.Equals(ID));
            return faculty;
        }

        public HttpResponseMessage Post([Bind(Include = "Faculty_Name,Model")]FacultyViewModel fvm)
        {
            //AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            string message = "New Faculty Added Successfully";
            bool isexists = facDB.CheckFacultyNameExist(fvm.Faculty_Name);
            if (isexists)
            {
                ModelState.AddModelError("Faculty_Name", "Faculty Name Already Exist");
            }
            if (ModelState.IsValid)
            {
                facDB.Add(fvm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        public HttpResponseMessage Put([Bind(Include = "Faculty_Name,Model")]FacultyViewModel fvm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            var faculty = facDB.ListAll().Find(f => f.Id.Equals(fvm.Id));
            string message = "Faculty Updated Successfully";
            tblFaculty tb = db.tblFaculties.Where(s => s.Faculty_Id == fvm.Id).FirstOrDefault();
            bool isexists = facDB.CheckFacultyNameExist(fvm.Faculty_Name);
            if (isexists)
            {
                if (tb.Faculty_Name == fvm.Faculty_Name)
                {
                    if (ModelState.IsValid)
                    {
                        facDB.Update(fvm);
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("Faculty_Name", "Faculty Name Already Exist");
                }
            }

            if (ModelState.IsValid)
            {
                facDB.Update(fvm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }
        public HttpResponseMessage Delete(int ID)
        {
            var faculty = facDB.ListAll().Find(f => f.Id.Equals(ID));
            string message = "Faculty Deleted Successfully";
            string messagenodata = "Data Not found/ Might be Deleted or Removed";
            HttpResponseMessage response;
            if (faculty == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messagenodata);
            }
            else
            {
                facDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.OK, message);
            }
            return response;
        }

    }
}

