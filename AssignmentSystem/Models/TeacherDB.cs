using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;

namespace AssignmentSystem.Models
{
    public class TeacherDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        string str;
        public List<TeacherViewModel> ListAll()
        {
            try
            {
                List<TeacherViewModel> lsttch = new List<TeacherViewModel>();
                var teacher = _db.tblTeachers.ToList();
                foreach (var item in teacher)
                {
                    lsttch.Add(new TeacherViewModel() { Id = item.Teacher_Id, Name = item.Name, Email = item.Email,  Phone = item.Phone, Address = item.Address });
                }
                return lsttch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public int Add(TeacherViewModel tvm)
        {
            try
            {
                tblTeacher tb = new tblTeacher();
                tb.Name = tvm.Name;
                tb.Email = tvm.Email;
                tb.Password = CreateRandomPassword(5);
                tb.Phone = tvm.Phone;
                tb.Address = tvm.Address;
                _db.tblTeachers.Add(tb);
                _db.SaveChanges();
                tblMainLog tml = new tblMainLog();
                var principal = System.Security.Claims.ClaimsPrincipal.Current;
                string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
                str = "New Teacher Added With Name- '" + tvm.Name + "' " +", Email- '"+ tvm.Email + "' " + ", Phone- '" + tvm.Phone + "' " + "and Address- '" + tvm.Address + "Added by Admin";
                tml.Description = str;
                tml.AdminName = Name;
                tml.Date = System.DateTime.Now;
                tml.EntityId = 4;
                tml.ItemId = tb.Teacher_Id;
                _db.tblMainLogs.Add(tml);
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public int Update(TeacherViewModel tvm)
        {
            try
            {
                tblTeacher tb = _db.tblTeachers.Where(t => t.Teacher_Id == tvm.Id).FirstOrDefault();
                
                //var oldName = tb.Name;
                //var oldEmail = tb.Email;
                //var oldPhone = tb.Phone;
                //var oldAddress = tb.Address;
                List<string> lstchange = new List<string>();
                if (tb.Name!=tvm.Name)
                {
                    lstchange.Add("name from " + tb.Name + " to " + tvm.Name);
                }
                if (tb.Email != tvm.Email)
                {
                    lstchange.Add("email from " + tb.Email + " to " + tvm.Email);
                }
                if (tb.Phone != tvm.Phone)
                {
                    lstchange.Add("phone from " + tb.Phone + " to " + tvm.Phone);
                }
                if (tb.Address != tvm.Address)
                {
                    lstchange.Add("address from " + tb.Address + " to " + tvm.Address);
                }
                tb.Name = tvm.Name;
                tb.Email = tvm.Email;
                tb.Password = tb.Password;
                tb.Phone = tvm.Phone;
                tb.Address = tvm.Address;
                //_db.SaveChanges();
                if (lstchange.Count==0)
                {
                    return _db.SaveChanges();
                }
                else
                {
                    tblMainLog tml = new tblMainLog();
                    var principal = System.Security.Claims.ClaimsPrincipal.Current;
                    string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
                    foreach (var item in lstchange)
                    {
                        str += item +","; 
                    }
                    string str2 = str;
                    tml.Description = str;
                    tml.AdminName = Name;
                    tml.Date = System.DateTime.Now;
                    tml.EntityId = 4;
                    tml.ItemId = tb.Teacher_Id;
                    _db.tblMainLogs.Add(tml);
                    return _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public int Delete(int ID)
        {
            try
            {
                tblTeacher tb = _db.tblTeachers.Where(t => t.Teacher_Id == ID).FirstOrDefault();
                _db.tblTeachers.Remove(tb);
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        public int SendEmail(int id)
        {

            using (AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities())
            {
                tblTeacher tb = db.tblTeachers.Where(x => x.Teacher_Id == id).FirstOrDefault();

                try
                {

                    if (tb != null)
                    {
                        var fromAddress = new MailAddress("santoshmandal97788@gmail.com", "santoshmandal97788");
                        var toAddress = new MailAddress(tb.Email, "To Name");
                        const string fromPassword = "changeme@12345";
                        const string subject = " ISMT COLLEGE Assignment Management System";
                        var teachername = tb.Name;
                        var Email = tb.Email;
                        var Password = tb.Password;
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
                            Body = "Hi!" + " " + teachername + " " + "Your Assignment Management System  For Teacher Account is Created. And Your Login Credential is Email: " + Email + " " + "and Password: " + Password
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
                return id;

            }
        }
        public bool EmailExists(string Email)
        {

            var isExist = _db.tblTeachers.Where(t => t.Email == Email).FirstOrDefault();
            if (isExist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<TeacherViewModel> SearchTeacherByName(string search)
        {
            List<TeacherViewModel> lstteach = new List<TeacherViewModel>();
            if (search != null)
            {
               
                var teacher = _db.tblTeachers.Where(t => t.Name.Contains(search) ||t.Email.Contains(search) ||t.Phone.Contains(search) ||t.Address.Contains(search));
                foreach (var item in teacher)
                {
                    lstteach.Add(new TeacherViewModel() { Id = item.Teacher_Id, Name = item.Name, Email = item.Email, Phone = item.Phone, Address = item.Address });
                }
                return lstteach;
            }
            else
            {
                //List<TeacherViewModel> lstteach1 = new List<TeacherViewModel>();
                var tb= _db.tblTeachers.ToList();
                foreach (var item in tb)
                {
                    lstteach.Add(new TeacherViewModel() { Id = item.Teacher_Id, Name = item.Name, Email = item.Email, Phone = item.Phone, Address = item.Address });
                }
                return lstteach;
            }
        }
    }
}