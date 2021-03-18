using AssignmentSystem.Models;
using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Mvc;

namespace AssignmentSystem.Controllers
{
    public class ForgetPasswordController : ApiController
    {
        ForgetPasswordDB fpDB = new ForgetPasswordDB();
        public HttpResponseMessage Post([Bind(Include = "Email,Model")] ForgetPasswordViewModel fpvm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            string message = "Your Password is Sent to Email";
            bool isexists = fpDB.EmailExists(fpvm.Email);
            if (!isexists)
            {
                ModelState.AddModelError("Email", "Not Registered Email");
            }
            if (ModelState.IsValid)
            {
                fpDB.SendRecoveryPasswordEMail(fpvm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


        }
    }
}
