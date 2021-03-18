using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace AssignmentSystem.Controllers
{
    public class RoleController : ApiController
    {
        public JsonResult GetRole()
        {
            int userRole;
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            var id = principal.Claims.ToList();
            var Role = id[0];
            if (Role.Value=="admin")
            {
                userRole = 0;
            }
            else if (Role.Value=="user")
            {
                userRole = 2;
            }
            else
            {
                userRole = 1;
            }
            return new JsonResult
            {
                Data = new { userRole = userRole },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
