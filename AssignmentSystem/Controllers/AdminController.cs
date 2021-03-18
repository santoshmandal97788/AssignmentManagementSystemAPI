//using AssignmentSystem.Filters;
using AssignmentSystem.Models;
using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AssignmentSystem.Controllers
{
    //[APIAuthorizeAttribute]
    public class AdminController : ApiController
    {
        AdminDB admDB = new AdminDB();

        public List<Admin> Get()
        {
            return admDB.ListAll();
        }
        public Admin Get(int ID)
        {
            var admin = admDB.ListAll().Find(a => a.Admin_Id.Equals(ID));
            return admin;
        }

        public HttpResponseMessage Post([FromBody]Admin adm)
        {
            int i = admDB.Add(adm);
            HttpResponseMessage response;
            if (i > 0)
            {
                response = Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }

        public HttpResponseMessage Put([FromBody]Admin adm)
        {
            var admin = admDB.ListAll().Find(a => a.Admin_Id.Equals(adm.Admin_Id));
            HttpResponseMessage response;
            if (admin == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                admDB.Update(adm);
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return response;
        }
        public HttpResponseMessage Delete(int ID)
        {
            var admin = admDB.ListAll().Find(a => a.Admin_Id.Equals(ID));
            HttpResponseMessage response;
            if (admin == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                admDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            return response;
        }
       
    }
}

