//using System;
//using System.Linq;
//using System.Web.Http;
//using System.Web.Http.Controllers;
//using System.Net.Http;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Security.Principal;
//using AssignmentSystem.Models;

//namespace AssignmentSystem.Filters
//{
//    public class APIAuthorizeAttribute : AuthorizeAttribute
//    {
//        private AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
//        public override void OnAuthorization(HttpActionContext filterContext)
//        {
//            if (filterContext.Request.Headers.Authorization == null)
//            {
//                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
//            }
//            else
//            {
//                string authenticationToken = filterContext.Request.Headers.Authorization.Parameter;
//                string decodedauthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
//                string[] emailandpassword = decodedauthenticationToken.Split(':');
//                string email = emailandpassword[0];
//                string password = emailandpassword[1];
//                if (CheckUser(email, password))
//                {
//                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(email), null);

//                }
//                else
//                {
//                    filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

//                }

//            }

//        }
//        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
//        {
//            base.HandleUnauthorizedRequest(filterContext);
//        }

//        private bool CheckUser(string email, string password)
//        {
//            var users = db.tblAdmins.Any(u => u.Email == email && u.Password == password);
//            if (users != null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//            bool i;
//            bool j;
//            bool k;

//            if (i = db.tblAdmins.Any(admin => admin.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && admin.Password == password))
//            {
//                return i;
//            }
//            else if (j = db.tblStudents.Any(student => student.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && student.Password == password))
//            {
//                return j;
//            }
//            else if (k = db.tblTeachers.Any(teacher => teacher.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && teacher.Password == password))
//            {
//                return j;
//            }
//            else
//            {
//                return false;
//            }




//            bool j = db.tblStudents.Any(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && user.Password == password);
//            return j;


//        }
//    }
//}