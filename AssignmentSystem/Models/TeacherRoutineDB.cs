using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models
{
    public class TeacherRoutineDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        public List<AssignmentRoutineViewModel> RoutineListByLoggedInteacherId(int? teacherid)
        {
            List<AssignmentRoutineViewModel> lstassrt = new List<AssignmentRoutineViewModel>();
            var teacherroutine = _db.tblAssignmentRoutines.Where(a => a.Teacher_Id == teacherid).ToList();

            foreach (var item in teacherroutine)
            {
                lstassrt.Add(new AssignmentRoutineViewModel() { Id = item.Routine_Id, Assignment_Name = item.Assignment_Name, Teacher_Id = item.tblTeacher.Teacher_Id, Teacher_Name = item.tblTeacher.Name, Assignment_Release_Date = item.Assignment_Release_Date, Deadline = item.Deadline, Section_Id = item.tblSection.Section_Id, Section_Name = item.tblSection.Sec_Name, Faculty_Id = item.tblFaculty.Faculty_Id, Faculty_Name = item.tblFaculty.Faculty_Name, Semester_Id = item.tblSemester.Semester_Id, Semester_Name = item.tblSemester.Semester_Name, Year_Batch_Id = item.YearBatchId, Year_Batch = item.tblYearBatch.Year_Batch, Name=item.Name});
            }
            return lstassrt;
        }

        public int ChangePassword(int? teacherid, ChangePasswordViewModel cvm)
        {
            try
            {
                tblTeacher tb = _db.tblTeachers.Where(s => s.Teacher_Id == teacherid && s.Password == cvm.OldPassword).FirstOrDefault();
                if (tb != null)
                {
                    tb.Password = cvm.ConfirmNewPassword;

                }
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckUser(int? teacherid, string password)
        {

            var isExist = _db.tblTeachers.Where(s => s.Teacher_Id == teacherid && s.Password == password).FirstOrDefault();
            if (isExist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<StudentViewModel> StudentListByRoutineId(int? routineid)
        {
            List<StudentViewModel> lstst = new List<StudentViewModel>();
            var student= _db.tblStudentRoutineRelations.Where(a => a.RoutineId == routineid).ToList();


            foreach (var item in student)
            {
                int? Submittedstaus = 0, checkedstatus = 0, marking = 0, feedbackstatus = 0, location = 0;
                var assignment = _db.tblSubmittedAssignments.Where(a => a.Routine_Id == routineid && a.Student_Id==item.StudentId).ToList();
                
                bool submittedassignment = _db.tblSubmittedAssignments.Any(a => a.Student_Id == item.StudentId && a.Routine_Id == item.RoutineId);

                if (submittedassignment)
                {
                    Submittedstaus = 1;
                    foreach (var data in assignment)
                    {
                        checkedstatus = data.Checked_Status;
                        marking = data.Marking;
                        feedbackstatus = data.Feedback_Status;
                        location = data.Assignmnet_Location;
                    }
                }
                else
                {
                    Submittedstaus = 0;
                    checkedstatus = 0;
                    marking = 0;
                    feedbackstatus = 0;
                    location = 0;
                }
                lstst.Add(new StudentViewModel() { Id = item.tblStudent.Student_Id, Name = item.tblStudent.Name,Email=item.tblStudent.Email,Phone=item.tblStudent.Phone,Gender=item.tblStudent.Gender,Address=item.tblStudent.Address, Faculty_Name = item.tblAssignmentRoutine.tblFaculty.Faculty_Name, Semester_Name = item.tblAssignmentRoutine.tblSemester.Semester_Name,  Year_Batch = item.tblAssignmentRoutine.tblYearBatch.Year_Batch, Sec_Name=item.tblAssignmentRoutine.tblSection.Sec_Name, SubmittedStatus = Submittedstaus,  FeedbackStatus = feedbackstatus, Marking = marking });
            }
            return lstst;
        }
    }
}
