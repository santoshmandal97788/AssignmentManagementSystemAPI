using AssignmentSystem.Models;
using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace AssignmentSystem.Controllers
{
    public class TeacherMobileController : ApiController
    {
        TeacherRoutineDB trDB = new TeacherRoutineDB();
        TeacherDB tDB = new TeacherDB();
      
        public List<AssignmentRoutineViewModel> GetRoutineListByLoggedInTeacherId()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = principal.Claims.ToList();
            var teacherId = id[1];
            return trDB.RoutineListByLoggedInteacherId(Convert.ToInt32(teacherId.Value));
        }
        [System.Web.Http.Route("api/TeacherMobile/teacherDetails")]
        public TeacherViewModel GetTeacher()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = principal.Claims.ToList();
            var teacherId = id[1];
            var teacher = tDB.ListAll().Find(t => t.Id.Equals(Convert.ToInt32(teacherId.Value)));
            return teacher;
        }
        [System.Web.Http.Route("api/TeacherMobile/StudentList")]
        public List<StudentViewModel> GetStudentListByRoutineId(int routineid)
        {
            return trDB.StudentListByRoutineId(routineid);
        }
        [System.Web.Http.Route("api/TeacherMobile/ChangePassword")]
        public HttpResponseMessage Post(ChangePasswordViewModel cvm)
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = principal.Claims.ToList();
            var teacherId = id[1];
            string message = "Password Changed Successfully";
            bool isexists = trDB.CheckUser(Convert.ToInt32(teacherId.Value), cvm.OldPassword);
            if (!isexists)
            {
                ModelState.AddModelError("OldPassword", "Incorrect Password");
            }

            if (ModelState.IsValid)
            {
                trDB.ChangePassword(Convert.ToInt32(teacherId.Value), cvm);
                return Request.CreateErrorResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

    }
}
