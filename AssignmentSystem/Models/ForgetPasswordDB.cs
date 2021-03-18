using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AssignmentSystem.Models
{
    public class ForgetPasswordDB
    {
        AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();

        public bool EmailExists(string Email)
        {

            var isExist = db.tblStudents.Where(s => s.Email == Email).FirstOrDefault();
            var isExist1 = db.tblTeachers.Where(t => t.Email == Email).FirstOrDefault();
            var isExist2 = db.tblAdmins.Where(t => t.Email == Email).FirstOrDefault();
            if (isExist!= null|| isExist1!=null || isExist2!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int SendRecoveryPasswordEMail(ForgetPasswordViewModel fpvm)
        {

            using (AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities())
            {
                tblStudent tb = db.tblStudents.Where(s => s.Email == fpvm.Email).FirstOrDefault();
                tblTeacher tb1 = db.tblTeachers.Where(t => t.Email == fpvm.Email).FirstOrDefault();
                tblAdmin tb2 = db.tblAdmins.Where(a => a.Email == fpvm.Email).FirstOrDefault();
                try
                {
                    string password = "";
                    if (tb != null)
                    {
                        password = tb.Password;
                    }
                    else if (tb1 != null)
                    {
                        password = tb1.Password;
                    }
                    else
                    {
                        password = tb2.Password;
                    }

                    if (tb != null || tb1 != null || tb2 != null)
                    {
                        var fromAddress = new MailAddress("santoshmandal97788@gmail.com", "santoshmandal97788");
                        var toAddress = new MailAddress(fpvm.Email, "To Name");
                        const string fromPassword = "changeme@12345";
                        const string subject = "Assignment Management System Password Recovery:";

                        var sendpwd = password;
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                        };
                        using (var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Body = "Hi!" + "Your Assignment Management System Password is: " + sendpwd
                        })
                        {
                            smtp.Send(message);
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {

                }
                return 0;

            }
        }
    }
}