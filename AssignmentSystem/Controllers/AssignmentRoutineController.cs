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
using System.Web;
using System.IO;
using System.Net.Http.Headers;

namespace AssignmentSystem.Controllers
{
    [AuthorizeAttribute]
  

    public class AssignmentRoutineController : ApiController
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();

        AssignmentRoutineDB asrDB = new AssignmentRoutineDB();
        [System.Web.Http.Route("api/Routine/List")]
        public List<AssignmentRoutineViewModel> Get()
        {
            return asrDB.ListAll();
        }
        [System.Web.Http.Route("api/Routine/byStudent")]
        [Authorize(Roles = "admin")]
        public List<AssignmentRoutineViewModel> GetRoutineListByStudentId(int? studentid)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    Page.Title = "Home page for " + User.Identity.na;
            //}
            //else
            //{
            //    Page.Title = "Home page for guest user.";
            //}
            return asrDB.RoutineListByStudentId(studentid);
        }
        [Authorize(Roles = "admin")]
        public JsonResult GetAssignmentRoutineList(string sortColumnName = "Assignment_Name", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<AssignmentRoutineViewModel> List = new List<AssignmentRoutineViewModel>();
            int totalPage = 0;
            int totalRecord = 0;


            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                var emp = asrDB.ListAll().Select(a => a);


                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(a => a.Assignment_Name.Contains(searchText) || a.Teacher_Name.Contains(searchText) || a.Assignment_Release_Date.ToString().Contains(searchText) || a.Deadline.ToString().Contains(searchText) || a.Faculty_Name.Contains(searchText) || a.Semester_Name.Contains(searchText) || a.Year_Batch.Contains(searchText));
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
        [Authorize(Roles = "admin")]
        public List<AssignmentRoutineViewModel> Get(int year, int faculty, int semester, int section)
        {
            return asrDB.ListAll().Where(x => x.Year_Batch_Id == year && x.Faculty_Id == faculty && x.Semester_Id == semester && x.Section_Id == section).ToList();
        }
        public AssignmentRoutineViewModel Get(int ID)
        {
            var assign = asrDB.ListAll().Find(a => a.Id.Equals(ID));
            return assign;
        }
        //List Student Submitted Assignment By StudentId
        [Authorize(Roles = "admin")]
        public List<AssignmentRoutineViewModel> GetSearchRoutine(string search)
        {
            return asrDB.SearchRoutine(search);
        }
        //[System.Web.Http.Route("api/AssignmentRoutine/DownloadFile")]
        //public JsonResult GetAssignmentRoutineFile(int routineid)
        //{

        //    string filename = "";

        //    //Create HTTP Response.
        //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

        //    //Fetch the File data from Database.
        //    //FilesEntities entities = new FilesEntities();
        //    AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();

        //    tblAssignmentRoutine file = _db.tblAssignmentRoutines.Where(r => r.Routine_Id == routineid).FirstOrDefault();

        //    //Set the Response Content.
        //    response.Content = new ByteArrayContent(file.Data);
        //    String filedata = new ByteArrayContent(file.Data).ToString();
        //    //Set the Response Content Length.
        //    response.Content.Headers.ContentLength = file.Data.LongLength;

        //    //Set the Content Disposition Header Value and FileName.
        //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //    response.Content.Headers.ContentDisposition.FileName = file.Name;
        //    filename = file.Name;
        //    //Set the File Content Type.
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue(file.Content_Type);
        //    // return response;


        //    return new JsonResult
        //    {
        //        //Data = new { List = List, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage},
        //        Data = new { filename = filename, filedata = filedata},
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}


        [System.Web.Http.Route("api/AssignmentRoutine/DownloadFile")]
        [Authorize(Roles = "admin,user,teacher")]
        public HttpResponseMessage GetFile(int routineid)
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Fetch the File data from Database.
            //FilesEntities entities = new FilesEntities();
            AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();

            tblAssignmentRoutine file = _db.tblAssignmentRoutines.Where(r => r.Routine_Id == routineid).FirstOrDefault();

            //Set the Response Content.
            response.Content = new ByteArrayContent(file.Data);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = file.Data.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = file.Name;
            //var filename = file.Name;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(file.Content_Type);
            // return Request.CreateResponse(HttpStatusCode.OK, filename);
            return response;
        }
      
        //[System.Web.Http.Route("api/Routine/Upload")]
        [Authorize(Roles = "admin")]
        public HttpResponseMessage Post()
        {
             var message = "Routine Added Successfully";
             var  Assignment_Name = HttpContext.Current.Request.Params["Assignment_Name"];
             var Teacher_Id = HttpContext.Current.Request.Params["Teacher_Id"];
             var Assignment_Release_Date = HttpContext.Current.Request.Params["Assignment_Release_Date"];
             var Deadline = HttpContext.Current.Request.Params["Deadline"];
             var Section_Id = HttpContext.Current.Request.Params["Section_Id"];
             var Faculty_Id = HttpContext.Current.Request.Params["Faculty_Id"];
             var Semester_Id = HttpContext.Current.Request.Params["Semester_Id"];
             var Year_Batch_Id = HttpContext.Current.Request.Params["Year_Batch_Id"];
            //var file= HttpContext.Current.Request.Files[0];
            if (Assignment_Name == "")
            {
                ModelState.AddModelError("Assignment_Name", "Enter Assignment Name");
            }
            if (Teacher_Id == "")
            {
                ModelState.AddModelError("Teacher_Name", "Select Subject Teacher");
            }
            if (Assignment_Release_Date == "")
            {
                ModelState.AddModelError("Assignment_Release_Date", "Select Release Date");
            }
            if (Deadline == "")
            {
                ModelState.AddModelError("Deadline", "Select Assignmnet Deadline");
            }
            if (Section_Id == "")
            {
                ModelState.AddModelError("Section_Name", "Select Section");
            }
            if (Faculty_Id == "")
            {
                ModelState.AddModelError("Faculty_Name", "Select Faculty");
            }
            if (Semester_Id == "")
            {
                ModelState.AddModelError("Semester_Name", "Select Semester");
            }
            if (Year_Batch_Id == "")
            {
                ModelState.AddModelError("Year_Batch", "Select Year Batch");
            }
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                ModelState.AddModelError("PDF", "Choose File");
            }
            if (ModelState.IsValid)
            {
                //Read the File data from Request.Form collection.
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
                //Convert the File data to Byte Array.
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }

                //Insert the File to Database Table.
                AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();

                tblAssignmentRoutine tb = new tblAssignmentRoutine();
                tb.Assignment_Name = Assignment_Name;
                tb.Teacher_Id = Convert.ToInt32(Teacher_Id);
                tb.Assignment_Release_Date = Convert.ToDateTime(Assignment_Release_Date);
                tb.Deadline = Convert.ToDateTime(Deadline);
                tb.Section_Id = Convert.ToInt32(Section_Id);
                tb.Faculty_Id = Convert.ToInt32(Faculty_Id);
                tb.Semester_Id = Convert.ToInt32(Semester_Id);
                tb.YearBatchId = Convert.ToInt32(Year_Batch_Id);
                tb.Name = Path.GetFileName(postedFile.FileName);
                tb.Content_Type = postedFile.ContentType;
                tb.Data = bytes;

                _db.tblAssignmentRoutines.Add(tb);
                _db.SaveChanges();

                tblStudentRoutineRelation tsr = new tblStudentRoutineRelation();
                var studnet = _db.tblStudents.Where(s => s.Faculty_Id == tb.Faculty_Id && s.Section_Id == tb.Section_Id && s.Semester_Id == tb.Semester_Id && s.YearBatchId == tb.YearBatchId).ToList();
                foreach (var item in studnet)
                {
                    tsr.RoutineId = tb.Routine_Id;
                    tsr.StudentId = item.Student_Id;
                    _db.tblStudentRoutineRelations.Add(tsr);
                    _db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


        }

        //var message = "Routine Added Successfully";
        //    if (ModelState.IsValid)
        //    {
        //        asrDB.Add(arvm);
        //        //asrDB.SendEMail(arvm.Section_Id ,arvm.Semester_Id ,arvm.Faculty_Id, arvm);
        //        return Request.CreateResponse(HttpStatusCode.OK, message);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }
       // [Authorize(Roles = "admin")]
   
        public HttpResponseMessage Put()
        {
            var Id = Convert.ToInt32(HttpContext.Current.Request.Params["Id"]);
            var Assignment_Name = HttpContext.Current.Request.Params["Assignment_Name"];
            var Teacher_Id = HttpContext.Current.Request.Params["Teacher_Id"];
            var Assignment_Release_Date = HttpContext.Current.Request.Params["Assignment_Release_Date"];
            var Deadline = HttpContext.Current.Request.Params["Deadline"];
            var Section_Id = HttpContext.Current.Request.Params["Section_Id"];
            var Faculty_Id = HttpContext.Current.Request.Params["Faculty_Id"];
            var Semester_Id = HttpContext.Current.Request.Params["Semester_Id"];
            var Year_Batch_Id =HttpContext.Current.Request.Params["Year_Batch_Id"];
            var message = "Routine Added Successfully";

            if (Assignment_Name == "")
            {
                ModelState.AddModelError("Assignment_Name", "Enter Assignment Name");
            }
            if (Teacher_Id == "")
            {
                ModelState.AddModelError("Teacher_Name", "Select Subject Teacher");
            }
            if (Assignment_Release_Date == "")
            {
                ModelState.AddModelError("Assignment_Release_Date", "Select Release Date");
            }
            if (Deadline == "")
            {
                ModelState.AddModelError("Deadline", "Select Assignmnet Deadline");
            }
            if (Section_Id == "")
            {
                ModelState.AddModelError("Section_Name", "Select Section");
            }
            if (Faculty_Id == "")
            {
                ModelState.AddModelError("Faculty_Name", "Select Faculty");
            }
            if (Semester_Id == "")
            {
                ModelState.AddModelError("Semester_Name", "Select Semester");
            }
            if (Year_Batch_Id == "")
            {
                ModelState.AddModelError("Year_Batch", "Select Year Batch");
            }
            //if (HttpContext.Current.Request.Files.Count == 0)
            //{
            //    ModelState.AddModelError("PDF", "Choose File");
            //}
            bool fieldChange = false;
            tblAssignmentRoutine tb = _db.tblAssignmentRoutines.Where(a => a.Routine_Id == Id).FirstOrDefault();
           // var assign = asrDB.ListAll().Find(a => a.Id.Equals(Id));
            if (tb.Section_Id == Convert.ToInt32(Section_Id) && tb.Faculty_Id == Convert.ToInt32(Faculty_Id) && tb.Semester_Id == Convert.ToInt32(Semester_Id) && tb.YearBatchId == Convert.ToInt32(Year_Batch_Id))
            {
                fieldChange = false;
            }
            else
            {
                fieldChange = true;
            }

           
            if (ModelState.IsValid)
            {
                //Read the File data from Request.Form collection.
               
                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    tb.Assignment_Name = Assignment_Name;
                    tb.Teacher_Id = Convert.ToInt32(Teacher_Id);
                    tb.Assignment_Release_Date = Convert.ToDateTime(Assignment_Release_Date);
                    tb.Deadline = Convert.ToDateTime(Deadline);
                    tb.Section_Id = Convert.ToInt32(Section_Id);
                    tb.Faculty_Id = Convert.ToInt32(Faculty_Id);
                    tb.Semester_Id = Convert.ToInt32(Semester_Id);
                    tb.YearBatchId = Convert.ToInt32(Year_Batch_Id);           
                    _db.SaveChanges();
                }
                else
                {
                    HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                    {
                        bytes = br.ReadBytes(postedFile.ContentLength);
                    }
                    //byte[] bytes;
                    //using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                    //{
                    //    bytes = br.ReadBytes(postedFile.ContentLength);
                    //}

                    tb.Assignment_Name = Assignment_Name;
                    tb.Teacher_Id = Convert.ToInt32(Teacher_Id);
                    tb.Assignment_Release_Date = Convert.ToDateTime(Assignment_Release_Date);
                    tb.Deadline = Convert.ToDateTime(Deadline);
                    tb.Section_Id = Convert.ToInt32(Section_Id);
                    tb.Faculty_Id = Convert.ToInt32(Faculty_Id);
                    tb.Semester_Id = Convert.ToInt32(Semester_Id);
                    tb.YearBatchId = Convert.ToInt32(Year_Batch_Id);
                    tb.Name = Path.GetFileName(postedFile.FileName);
                    tb.Content_Type = postedFile.ContentType;
                    tb.Data = bytes;
                    _db.SaveChanges();

                }
                //Convert the File data to Byte Array.

                //tblStudentRoutineRelation tsr = new tblStudentRoutineRelation();
                //var studnet = _db.tblStudents.Where(s => s.Faculty_Id == tb.Faculty_Id && s.Section_Id == tb.Section_Id && s.Semester_Id == tb.Semester_Id && s.YearBatchId == tb.YearBatchId).ToList();
                if (fieldChange)
                {
                    var studentlist = _db.tblStudentRoutineRelations.Where(s => s.RoutineId == tb.Routine_Id).ToList();
                    foreach (var item in studentlist)
                    {
                        _db.tblStudentRoutineRelations.Remove(item);
                    }
                    tblStudentRoutineRelation tsr = new tblStudentRoutineRelation();
                    var studnet = _db.tblStudents.Where(s => s.Faculty_Id == tb.Faculty_Id && s.Section_Id == tb.Section_Id && s.Semester_Id == tb.Semester_Id && s.YearBatchId == tb.YearBatchId).ToList();
                    foreach (var item in studnet)
                    {
                        tsr.RoutineId = tb.Routine_Id;
                        tsr.StudentId = item.Student_Id;
                        _db.tblStudentRoutineRelations.Add(tsr);
                        _db.SaveChanges();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }



            //var assign = asrDB.ListAll().Find(a => a.Id.Equals(arvm.Id));
            //string message = "Assignment Routine Updated Successfully";
            //if (ModelState.IsValid)
            //{
            //    asrDB.Update(arvm);
            //    return Request.CreateResponse(HttpStatusCode.OK, message);
            //}
            //else
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            //}

        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Delete(int ID)
        {
            var assign = asrDB.ListAll().Find(a => a.Id.Equals(ID));
            string message = "Assignment Routine Deleted Successfully";
            string messagenodata = "Data Not found/ Might be Deleted or Removed";
            HttpResponseMessage response;
            if (assign == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messagenodata);
            }
            else
            {
                asrDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.OK, message);
            }
            return response;
        }
    }
}
