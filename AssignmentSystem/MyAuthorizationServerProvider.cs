using AssignmentSystem.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AssignmentSystem
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var adminUser = db.tblAdmins.Where(a => a.Email == context.UserName && a.Password == context.Password).FirstOrDefault();
            var studentUser = db.tblStudents.Where(a => a.Email == context.UserName && a.Password == context.Password).FirstOrDefault();
            var teacherUser = db.tblTeachers.Where(a => a.Email == context.UserName && a.Password == context.Password).FirstOrDefault();

            if (adminUser != null)
            {
                
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                //identity.AddClaim(new Claim("Email", adminUser.Email));
                identity.AddClaim(new Claim(ClaimTypes.Name, adminUser.Name));
                context.Validated(identity);
            }
            else if (studentUser != null)
            {
          
               // var loggedInStudentId = studentUser.Student_Id;
               
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("Id", studentUser.Student_Id.ToString()));
                identity.AddClaim(new Claim("Email", studentUser.Email));
              //  identity.AddClaim(new Claim(ClaimTypes.Name, studentUser.Name));
                context.Validated(identity);
            }
            else if (teacherUser != null)
            {
               // var loggedInTeacherId = studentUser.Student_Id;
                identity.AddClaim(new Claim(ClaimTypes.Role, "teacher"));
                identity.AddClaim(new Claim("Id", teacherUser.Teacher_Id.ToString()));
                identity.AddClaim(new Claim("Email", teacherUser.Email));
                identity.AddClaim(new Claim(ClaimTypes.Name, teacherUser.Name));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_Grant", "Incorrect Username and Password");
                return;
            }
            ////bool i;
            ////bool j;
            ////bool k;
            ////var users = db.tblAdmins.Where(a => a.Email == context.UserName && a.Password == context.Password).FirstOrDefault();
            ////if (users != null)
            ////{
            ////    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            ////    identity.AddClaim(new Claim("username", context.UserName));
            ////    identity.AddClaim(new Claim(ClaimTypes.Name, "Santosh Mandal"));

            ////    context.Validated(identity);
            ////}

            ////if (i = db.tblAdmins.Any(a => a.Email == context.UserName && a.Password == context.Password))
            ////{
            ////    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            ////    identity.AddClaim(new Claim("username", context.UserName));
            ////    identity.AddClaim(new Claim(ClaimTypes.Name, "Santosh Mandal"));

            ////    context.Validated(identity);
            ////}
            ////else if (j = db.tblStudents.Any(a => a.Email == context.UserName && a.Password == context.Password))
            ////{

            ////    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            ////    identity.AddClaim(new Claim("username", context.UserName));
            ////    identity.AddClaim(new Claim("Name", "Santosh Mandal"));

            ////    context.Validated(identity);
            ////}
            ////else if (k = db.tblTeachers.Any(a => a.Email == context.UserName && a.Password == context.Password))
            ////{
            ////    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            ////    identity.AddClaim(new Claim("username", "user"));
            ////    //identity.AddClaim(new Claim(ClaimTypes.Name, "David Warner"));
            ////    context.Validated(identity);
            ////}
            ////else
            ////{
            ////    context.SetError("invalid_Grant", "Incorrect Username and Password");
            ////    return;
            ////}
        }
        

    }
}

  