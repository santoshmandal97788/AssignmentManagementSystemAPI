//using AssignmentSystem.Filters;
using AssignmentSystem.Models;
using AssignmentSystem.Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace AssignmentSystem.Controllers
{
    //[AuthorizeAttribute]
    //[Authorize(Roles = "admin")]
    public class StudentController : ApiController
    {
        StudentDB stuDB = new StudentDB();

        public List<StudentViewModel> Get()
        {
            return stuDB.ListAll();
        }

        public JsonResult GetStudentList(string sortColumnName = "Name", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
             List<StudentViewModel> List = new List<StudentViewModel>();
            int totalPage = 0;
            int totalRecord = 0;

           
            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                //var emp = dc.tblStudents.Select(a => a);
                var emp = stuDB.ListAll().Select(a=>a);
             
                //var emp = stuDB.ListAll().Select(x => new { Id = x.Student_Id, Name = x.Name, Email = x.Email, Gender = x.Gender, Phone = x.Phone ,Address=x.Address, YearBatchId=x.YearBatchId, YearBatch=x.tblYearBatch.YearBatchId, FacultyId=x.Faculty_Id, FacultyName=x.tblFaculty.Faculty_Name, SectionId=x.Section_Id, SectionName=x.tblSection.Sec_Name, SemesterId=x.Semester_Id, SemesterName=x.tblSemester.Semester_Name});

                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(x => x.Name.Contains(searchText) || x.Email.Contains(searchText) || x.Gender.Contains(searchText) || x.Phone.Contains(searchText) || x.Address.Contains(searchText) || x.Year_Batch.Contains(searchText) || x.Faculty_Name.Contains(searchText) || x.Sec_Name.Contains(searchText) || x.Semester_Name.Contains(searchText));
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
                Data = new { List = List,totalRecord=totalRecord, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage, pageSize = pageSize },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet 
            };
        }

        public List<StudentViewModel> Get(int year, int faculty, int semester, int section)
        {
            return stuDB.ListAll().Where(x => x.Year_Batch_Id == year && x.Faculty_Id ==faculty && x.Semester_Id == semester && x.Section_Id == section).ToList();
        }
      
        public StudentViewModel Get(int ID)
        {
            var student = stuDB.ListAll().Find(t => t.Id.Equals(ID));
            return student;
        }
        //List Student Submitted Assignment By StudentId
        public List<StudentViewModel> GetSearchStudent(string search)
        {
            return stuDB.SearchStudent(search);
        }

        public HttpResponseMessage Post([Bind(Include = "Email,Model")]StudentViewModel svm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            string message = "Student Added Successfully";
            bool isexists = stuDB.EmailExists(svm.Email);
            if (isexists)
            {
                ModelState.AddModelError("Email", "Email Already Exist");
            }
            if (ModelState.IsValid)
            {
                //    stuDB.Add(svm);
                //stuDB.SendSMS();

                tblStudent tb = db.tblStudents.Where(x => x.Email == svm.Email).FirstOrDefault();
                if (tb != null)
                {
                    stuDB.SendEMail(tb.Student_Id);
                }

                //tblStudent tb1 = db.tblStudents.Where(x => x.Phone == svm.Phone).FirstOrDefault();
                //if (tb1 != null)
                //{
                //    stuDB.SendSMS(tb1.Phone);
                //}

                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }


        public HttpResponseMessage Put([Bind(Include = "Email,Model")]StudentViewModel svm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            var student = stuDB.ListAll().Find(t => t.Id.Equals(svm.Id));
            string message = "Student Updated Successfully";
            tblStudent tb = db.tblStudents.Where(s => s.Student_Id == svm.Id).FirstOrDefault();
            bool isexists = stuDB.EmailExists(svm.Email);
            if (isexists)
            {
                if (tb.Email == svm.Email)
                {
                    if (ModelState.IsValid)
                    {
                        stuDB.Update(svm);
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
                stuDB.Update(svm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }
        [System.Web.Http.Route("api/Student/Shift")]
        public HttpResponseMessage PutShiftStudentSemesterSection( ShiftStudentViewModel svm)
        {
            string message = "Student Shifted Successfully";
            if (ModelState.IsValid)
            {
                stuDB.ShiftStudentSemesterOrSection(svm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }
        public HttpResponseMessage Delete(int ID)
        {
            var student  = stuDB.ListAll().Find(t => t.Id.Equals(ID));
            string message = "Student Deleted Successfully";
            string messagenodata = "Data Not found/ Might be Deleted or Removed";
            HttpResponseMessage response;
            if (student == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NoContent, messagenodata);
            }
            else
            {
                stuDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.OK, message);
            }
            return response;
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}

