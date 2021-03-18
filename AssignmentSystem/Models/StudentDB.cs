using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AssignmentSystem.Models
{
    public class StudentDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();

        public List<StudentViewModel> ListAll()
        {
          
            try
            {
                List<StudentViewModel> lststu = new List<StudentViewModel>();
                var student = _db.tblStudents.ToList();
                foreach (var item in student)
                {
                    lststu.Add(new StudentViewModel() { Id = item.Student_Id, Name = item.Name, Email = item.Email, Gender = item.Gender, Phone = item.Phone, Address = item.Address, Year_Batch_Id = item.YearBatchId, Year_Batch = item.tblYearBatch.Year_Batch, Faculty_Id=item.Faculty_Id, Faculty_Name = item.tblFaculty.Faculty_Name, Section_Id=item.Section_Id, Sec_Name = item.tblSection.Sec_Name, Semester_Id=item.Semester_Id, Semester_Name = item.tblSemester.Semester_Name });
                }
                return lststu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Add(StudentViewModel svm)
        {
            try
            {
                tblStudent tb = new tblStudent();
                //var keyNew = Helper.GeneratePassword(10);
                //var password = Helper.EncodePassword(keyNew, keyNew);

                //objNewUser.VCode = keyNew;



                tb.Name = svm.Name;
                tb.Email = svm.Email;
                tb.Password = CreateRandomPassword(5);/*stu.Password;*/
                //tb.VCode = keyNew;
                tb.Gender = svm.Gender;
                tb.Phone = svm.Phone;
                tb.Address = svm.Address;
                tb.YearBatchId = svm.Year_Batch_Id;
                tb.Faculty_Id = svm.Faculty_Id;
                tb.Section_Id = svm.Section_Id;
                tb.Semester_Id = svm.Semester_Id;
                _db.tblStudents.Add(tb);
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(StudentViewModel svm)
        {
            try
            {
                tblStudent tb = _db.tblStudents.Where(s => s.Student_Id == svm.Id).FirstOrDefault();
                tb.Name = svm.Name;
                tb.Email = svm.Email;
                tb.Password = tb.Password;
                tb.Gender = svm.Gender;
                tb.Phone = svm.Phone;
                tb.Address = svm.Address;
                tb.YearBatchId = svm.Year_Batch_Id;
                tb.Faculty_Id = svm.Faculty_Id;
                tb.Section_Id = svm.Section_Id;
                tb.Semester_Id = svm.Semester_Id;
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public int ShiftStudentSemesterOrSection( ShiftStudentViewModel svm )
        {
            try
            {
                var shiftStudentList = _db.tblStudents.Where(s => s.YearBatchId == svm.Year_Batch_Id && s.Faculty_Id== svm.Faculty_Id && s.Semester_Id==svm.Semester_Id && s.Section_Id==svm.Section_Id).ToList();
                //tblStudent tb = new tblStudent();

               // tblStudent tb = _db.tblStudents.Where(s => s.Student_Id == svm.Id).FirstOrDefault();
                foreach (var item in shiftStudentList)
                {
                    tblStudent tb = _db.tblStudents.Where(s => s.Student_Id == item.Student_Id).FirstOrDefault();
                    tb.Section_Id = svm.ToSectionId;
                    tb.Semester_Id = svm.ToSemesterId;
                    _db.SaveChanges();
                }                
                return 0;

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
                tblStudent tb = _db.tblStudents.Where(s => s.Student_Id == ID).FirstOrDefault();
                _db.tblStudents.Remove(tb);
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

        public int SendEMail(int id)
        {

            using (AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities())
            {
                tblStudent tb = db.tblStudents.Where(x => x.Student_Id == id).FirstOrDefault();
                

                try
                {

                    if (tb != null)
                    {
                        var fromAddress = new MailAddress("santoshmandal97788@gmail.com", "santoshmandal97788");
                        var toAddress = new MailAddress(tb.Email, "To Name");
                        const string fromPassword = "changeme@12345";
                        const string subject = "Assignment Management System";
                        var studentname = tb.Name;
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
                            Body = "Hi!" + " " + studentname + " " + "Your Assignment Management System Account is Created. And Your Login Credential is Email: " + Email + " " + "and Password: " + Password
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
           
            var isExist = _db.tblStudents.Where(s => s.Email == Email).FirstOrDefault();
            if (isExist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<StudentViewModel> SearchStudent(string search)
        {
            List<StudentViewModel> lststu = new List<StudentViewModel>();
            if (search != null)
            {

                var student = _db.tblStudents.Where(s => s.tblSection.Sec_Name.StartsWith(search) || s.Name.Contains(search) || s.Email.Contains(search) || s.Gender.Contains(search) || s.Phone.Contains(search) ||s.Address.Contains(search)|| s.tblYearBatch.Year_Batch.Contains(search)||s.tblFaculty.Faculty_Name.Contains(search)||s.tblSemester.Semester_Name.Contains(search));
                foreach (var item in student)
                {
                    lststu.Add(new StudentViewModel() { Id = item.Student_Id, Name = item.Name, Email = item.Email, Gender = item.Gender, Phone = item.Phone, Address = item.Address, Year_Batch_Id = item.YearBatchId, Year_Batch = item.tblYearBatch.Year_Batch, Faculty_Id = item.Faculty_Id, Faculty_Name = item.tblFaculty.Faculty_Name, Section_Id = item.Section_Id, Sec_Name = item.tblSection.Sec_Name, Semester_Id = item.Semester_Id, Semester_Name = item.tblSemester.Semester_Name });
                }
                return lststu;
            }
            else
            {
                //List<TeacherViewModel> lstteach1 = new List<TeacherViewModel>();
                var tb = _db.tblStudents.ToList();
                foreach (var item in tb)
                {
                    lststu.Add(new StudentViewModel() { Id = item.Student_Id, Name = item.Name, Email = item.Email, Gender = item.Gender, Phone = item.Phone, Address = item.Address, Year_Batch_Id = item.YearBatchId, Year_Batch = item.tblYearBatch.Year_Batch, Faculty_Id = item.Faculty_Id, Faculty_Name = item.tblFaculty.Faculty_Name, Section_Id = item.Section_Id, Sec_Name = item.tblSection.Sec_Name, Semester_Id = item.Semester_Id, Semester_Name = item.tblSemester.Semester_Name });
                }
                return lststu;
            }
        }
        //public string SendSMS()
        //{
        //    using (AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities())
        //    {
        //        tblStudent tb = db.tblStudents.Where(x => x.Phone == Phone).FirstOrDefault();

        //        if (tb != null)
        //        {
        //            var Email = tb.Email;
        //            var Password = tb.Password;
        //            var Phoneno = tb.Phone;
        //            var studentname = tb.Name;
        //            String message = HttpUtility.UrlEncode("Hi!" + " " + studentname + " " + "Your Assignment Management System Account is Created. And Your Login Credential is Email: " + Email + " " + "and Password: " + Password);
        //            String message = HttpUtility.UrlEncode("This is your message");
        //            using (var wb = new WebClient())
        //            {
        //                byte[] response = wb.UploadValues("https://api.txtlocal.com/send/", new NameValueCollection()
        //                {
        //                {"apikey" , "aqKeunxeo+I-QlD7juMSdiPLjDtYSmjdt25bILbmSz"},
        //                {"numbers" , "+9779816815884"},
        //                {"message" , message},
        //                {"sender" , "ISMT AMS"}
        //                });
        //                string result = System.Text.Encoding.UTF8.GetString(response);
        //                return result;
        //            }

        //        }

        //    }
        //}

    }
}