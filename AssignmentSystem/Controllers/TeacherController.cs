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
using System.Web.Http.Results;

namespace AssignmentSystem.Controllers
{
    [AuthorizeAttribute]
    [Authorize(Roles = "admin")]
    public class TeacherController : ApiController
    {
        TeacherDB teaDB = new TeacherDB();
        [System.Web.Http.Route("api/Teacher/List")]
        public List<TeacherViewModel> Get()
        {
            return teaDB.ListAll();
        }

        public JsonResult GetTeacherList(string sortColumnName = "Name", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<TeacherViewModel> List = new List<TeacherViewModel>();
            int totalPage = 0;
            int totalRecord = 0;


            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                var emp = teaDB.ListAll().Select(a => a);
                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(x => x.Name.Contains(searchText) || x.Email.Contains(searchText) || x.Phone.Contains(searchText) || x.Address.Contains(searchText));
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

        public TeacherViewModel Get(int ID)
        {
            var teacher = teaDB.ListAll().Find(t => t.Id.Equals(ID));
            return teacher;
        }
        //List Student Submitted Assignment By StudentId
        //public List<TeacherViewModel> GetTeacherByTeacherName(string search)
        //{
        //    return teaDB.SearchTeacherByName(search);
        //}
        //public HttpResponseMessage GetTeacherByTeacherName(string name)
        //{
        //    AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        //    var teacher = _db.tblTeachers.Where(t => t.Name == name).ToList();
        //    string message = "No Data Found";
        //    if (teacher == null)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, teacher);
        //    }

        //}
        public HttpResponseMessage Post([Bind(Include = "Email,Model")]TeacherViewModel tvm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            string message = "Teacher Added Successfully";
            bool isexists = teaDB.EmailExists(tvm.Email);
            if (isexists)
            {
                ModelState.AddModelError("Email", "Email Already Exist");
            }
            if (ModelState.IsValid)
            {
                teaDB.Add(tvm);


                //tblTeacher tb = db.tblTeachers.Where(x => x.Email == tvm.Email).FirstOrDefault();
                //if (tb != null)
                //{
                //    teaDB.SendEmail(tb.Teacher_Id);
                //}
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public HttpResponseMessage Put([Bind(Include = "Email,Model")]TeacherViewModel tvm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            var student = teaDB.ListAll().Find(t => t.Id.Equals(tvm.Id));
            string message = "Teacher Updated Successfully";
            tblTeacher tb = db.tblTeachers.Where(t => t.Teacher_Id == tvm.Id).FirstOrDefault();
            bool isexists = teaDB.EmailExists(tvm.Email);
            if (isexists)
            {
                if (tb.Email == tvm.Email)
                {
                    if (ModelState.IsValid)
                    {
                        teaDB.Update(tvm);
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email Already Exist");
                }
            }

            if (ModelState.IsValid)
            {
                teaDB.Update(tvm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


        }
        public HttpResponseMessage Delete(int ID)
        {
            var teacher = teaDB.ListAll().Find(t => t.Id.Equals(ID));
            string message = "Teacher Deleted Successfully";
            string messagenodata = "Data Not found/ Might be Deleted or Removed";
            HttpResponseMessage response;
            if (teacher == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messagenodata);
            }
            else
            {
                teaDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.OK, message);
            }
            return response;
        }

    }
}

