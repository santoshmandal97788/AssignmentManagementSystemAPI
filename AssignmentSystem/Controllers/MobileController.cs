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
    [AuthorizeAttribute]
    [Authorize(Roles = "user")]
    public class MobileController : ApiController
    {
        StudentRoutineDB srDB = new StudentRoutineDB();
        StudentDB stuDB = new StudentDB();
        public List<AssignmentRoutineViewModel> GetRoutineListByLoggedInStudentId()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = principal.Claims.ToList();           
            var studentId = id[1];
            return srDB.RoutineListByLoggedInStudentId(Convert.ToInt32(studentId.Value));
        }
        [System.Web.Http.Route("api/Mobile/StudentDetails")]
        public StudentViewModel GetStudent()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = principal.Claims.ToList();
            var studentId = id[1];
            var student = stuDB.ListAll().Find(t => t.Id.Equals(Convert.ToInt32(studentId.Value)));
            return student;
        }
        [System.Web.Http.Route("api/Mobile/ChangePassword")]
        public HttpResponseMessage Post(ChangePasswordViewModel cvm)
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = principal.Claims.ToList();
            var studentId = id[1];
            string message = "Password Changed Successfully";
            bool isexists = srDB.CheckUser(Convert.ToInt32(studentId.Value), cvm.OldPassword);
            if (!isexists)
            {
                ModelState.AddModelError("OldPassword", "Incorrect Password");
            }

            if (ModelState.IsValid)
            {
                    srDB.ChangePassword(Convert.ToInt32(studentId.Value), cvm);
                    return Request.CreateErrorResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

    }
}
