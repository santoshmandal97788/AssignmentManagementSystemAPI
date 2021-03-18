using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http;

namespace AssignmentSystem.Models
{
    public class AssignmentRoutineDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        public List<AssignmentRoutineViewModel> ListAll()
        {
            List<AssignmentRoutineViewModel> lstassrt = new List<AssignmentRoutineViewModel>();
            var assroutine = _db.tblAssignmentRoutines.ToList();
            foreach (var item in assroutine)
            {
                lstassrt.Add(new AssignmentRoutineViewModel() { Id = item.Routine_Id, Assignment_Name = item.Assignment_Name, Teacher_Id = item.tblTeacher.Teacher_Id, Teacher_Name = item.tblTeacher.Name, Assignment_Release_Date = item.Assignment_Release_Date, Deadline=item.Deadline, Section_Id=item.tblSection.Section_Id,Section_Name=item.tblSection.Sec_Name, Faculty_Id=item.tblFaculty.Faculty_Id,Faculty_Name=item.tblFaculty.Faculty_Name, Semester_Id=item.tblSemester.Semester_Id,Semester_Name=item.tblSemester.Semester_Name, Year_Batch_Id=item.YearBatchId, Year_Batch=item.tblYearBatch.Year_Batch,Name=item.Name });
            }
            return lstassrt;
        }
        public List<AssignmentRoutineViewModel> RoutineListByStudentId(int? studentid)
        {
            List<AssignmentRoutineViewModel> lstassrt = new List<AssignmentRoutineViewModel>();
            var assroutine = _db.tblStudentRoutineRelations.Where(a => a.StudentId == studentid).ToList();
            foreach (var item in assroutine)
            {
                lstassrt.Add(new AssignmentRoutineViewModel() { Id = item.tblAssignmentRoutine.Routine_Id, Assignment_Name = item.tblAssignmentRoutine. Assignment_Name, Teacher_Id = item.tblAssignmentRoutine. tblTeacher.Teacher_Id, Teacher_Name = item.tblAssignmentRoutine. tblTeacher.Name, Assignment_Release_Date = item.tblAssignmentRoutine. Assignment_Release_Date, Deadline = item.tblAssignmentRoutine.Deadline, Section_Id = item.tblAssignmentRoutine. tblSection.Section_Id, Section_Name = item.tblAssignmentRoutine. tblSection.Sec_Name, Faculty_Id = item.tblAssignmentRoutine. tblFaculty.Faculty_Id, Faculty_Name = item.tblAssignmentRoutine.tblFaculty.Faculty_Name, Semester_Id = item.tblAssignmentRoutine.tblSemester.Semester_Id, Semester_Name = item.tblAssignmentRoutine.tblSemester.Semester_Name, Year_Batch_Id = item.tblAssignmentRoutine.YearBatchId, Year_Batch = item.tblAssignmentRoutine.tblYearBatch.Year_Batch });
            }
            return lstassrt;
        }
        public int Add(AssignmentRoutineViewModel arvm)
        {
            tblAssignmentRoutine tb = new tblAssignmentRoutine();
            tb.Assignment_Name = arvm.Assignment_Name;
            tb.Teacher_Id = arvm.Teacher_Id;
            tb.Assignment_Release_Date = arvm.Assignment_Release_Date;
            tb.Deadline = arvm.Deadline;
            tb.Section_Id = arvm.Section_Id;
            tb.Faculty_Id = arvm.Faculty_Id;
            tb.Semester_Id = arvm.Semester_Id;
            tb.YearBatchId = arvm.Year_Batch_Id;


            _db.tblAssignmentRoutines.Add(tb);
            _db.SaveChanges();


            tblStudentRoutineRelation tsr = new tblStudentRoutineRelation();
            var studnet = _db.tblStudents.Where(s => s.Faculty_Id == tb.Faculty_Id && s.Section_Id == tb.Section_Id && s.Semester_Id == tb.Semester_Id && s.YearBatchId == tb.YearBatchId).ToList();
            foreach (var item in studnet)
            {
                tsr.RoutineId = tb.Routine_Id;
                tsr.StudentId = item.Student_Id;
                _db.tblStudentRoutineRelations.Add(tsr);
                _db.SaveChanges();
            }
            return 0;
        }
        public int Update(AssignmentRoutineViewModel arvm)
        {
            bool fieldChange = false;
            tblAssignmentRoutine tb = _db.tblAssignmentRoutines.Where(a => a.Routine_Id == arvm.Id).FirstOrDefault();
            if (tb.Section_Id==arvm.Section_Id && tb.Faculty_Id==arvm.Faculty_Id &&tb.Semester_Id==arvm.Semester_Id &&tb.YearBatchId==arvm.Year_Batch_Id)
            {
                fieldChange = false;
            }
            else
            {
                fieldChange = true;
            }
            tb.Assignment_Name = arvm.Assignment_Name;
            tb.Teacher_Id = arvm.Teacher_Id;
            tb.Assignment_Release_Date = arvm.Assignment_Release_Date;
            tb.Deadline = arvm.Deadline;
            tb.Section_Id = arvm.Section_Id;
            tb.Faculty_Id = arvm.Faculty_Id;
            tb.Semester_Id = arvm.Semester_Id;
            tb.YearBatchId = arvm.Year_Batch_Id;
             _db.SaveChanges();
            //tblStudentRoutineRelation tsr = new tblStudentRoutineRelation();
            //var studnet = _db.tblStudents.Where(s => s.Faculty_Id == tb.Faculty_Id && s.Section_Id == tb.Section_Id && s.Semester_Id == tb.Semester_Id && s.YearBatchId == tb.YearBatchId).ToList();
            if (fieldChange)
            {
                var studentlist = _db.tblStudentRoutineRelations.Where(s => s.RoutineId == tb.Routine_Id).ToList();
                foreach (var item in studentlist)
                {
                    _db.tblStudentRoutineRelations.Remove(item);
                }
                tblStudentRoutineRelation tsr = new tblStudentRoutineRelation();
                var studnet = _db.tblStudents.Where(s => s.Faculty_Id == tb.Faculty_Id && s.Section_Id == tb.Section_Id && s.Semester_Id == tb.Semester_Id && s.YearBatchId == tb.YearBatchId).ToList();
                foreach (var item in studnet)
                {
                    tsr.RoutineId = tb.Routine_Id;
                    tsr.StudentId = item.Student_Id;
                    _db.tblStudentRoutineRelations.Add(tsr);
                    _db.SaveChanges();
                }
            }
           
            return 0;
        }
        public int Delete(int ID)
        {
            tblAssignmentRoutine tb = _db.tblAssignmentRoutines.Where(a => a.Routine_Id == ID).FirstOrDefault();
            _db.tblAssignmentRoutines.Remove(tb);
            return _db.SaveChanges();
        }
        public string SendEMail(int sectionid,int semesterid,int facultyid,AssignmentRoutineViewModel arvm)
        {

            using (AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities())
            {
                List<tblStudent> tb = db.tblStudents.Where(x => x.Section_Id == sectionid && x.Semester_Id == semesterid && x.Faculty_Id==facultyid).ToList();
                //var tb1 = db.tblStudents.ToList();
                
              

                try
                {
                    foreach (var item in tb)
                    {
                        if (tb != null)
                        {
                            var fromAddress = new MailAddress("santoshmandal97788@gmail.com", "santoshmandal97788");
                            var toAddress = new MailAddress(item.Email, "To Name");
                            const string fromPassword = "changeme@12345";
                            const string subject = "Assignment Management System";
                            var studentname = item.Name;


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
                                Body = "Hi!" + " " + studentname + " " + "Your "+ arvm.Assignment_Name + " Assignment is Released and Deadline is "+ arvm.Deadline
                            })
                            {
                                smtp.Send(message);
                            }

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
                return "Email Sent";

            }
        }
        public List<AssignmentRoutineViewModel> SearchRoutine(string search)
        {
            List<AssignmentRoutineViewModel> lstrout = new List<AssignmentRoutineViewModel>();
            if (search != null)
            {

                var routine = _db.tblAssignmentRoutines.Where(r => r.Assignment_Name.Contains(search) || r.tblTeacher.Name.Contains(search) || r.Assignment_Release_Date.ToString().Contains(search) || r.Deadline.ToString().Contains(search)||r.tblSection.Sec_Name.StartsWith(search)||r.tblFaculty.Faculty_Name.Contains(search)||r.tblSemester.Semester_Name.Contains(search)||r.tblYearBatch.Year_Batch.Contains(search));
                foreach (var item in routine)
                {
                    lstrout.Add(new AssignmentRoutineViewModel() { Id = item.Routine_Id, Assignment_Name = item.Assignment_Name, Teacher_Id = item.tblTeacher.Teacher_Id, Teacher_Name = item.tblTeacher.Name, Assignment_Release_Date = item.Assignment_Release_Date, Deadline = item.Deadline, Section_Id = item.tblSection.Section_Id, Section_Name = item.tblSection.Sec_Name, Faculty_Id = item.tblFaculty.Faculty_Id, Faculty_Name = item.tblFaculty.Faculty_Name, Semester_Id = item.tblSemester.Semester_Id, Semester_Name = item.tblSemester.Semester_Name, Year_Batch_Id = item.YearBatchId, Year_Batch = item.tblYearBatch.Year_Batch });
                }
                return lstrout;
            }
            else
            {
                //List<TeacherViewModel> lstteach1 = new List<TeacherViewModel>();
                var tb = _db.tblAssignmentRoutines.ToList();
                foreach (var item in tb)
                {
                    lstrout.Add(new AssignmentRoutineViewModel() { Id = item.Routine_Id, Assignment_Name = item.Assignment_Name, Teacher_Id = item.tblTeacher.Teacher_Id, Teacher_Name = item.tblTeacher.Name, Assignment_Release_Date = item.Assignment_Release_Date, Deadline = item.Deadline, Section_Id = item.tblSection.Section_Id, Section_Name = item.tblSection.Sec_Name, Faculty_Id = item.tblFaculty.Faculty_Id, Faculty_Name = item.tblFaculty.Faculty_Name, Semester_Id = item.tblSemester.Semester_Id, Semester_Name = item.tblSemester.Semester_Name, Year_Batch_Id = item.YearBatchId, Year_Batch = item.tblYearBatch.Year_Batch });
                }
                return lstrout;
            }
        }
    }
}