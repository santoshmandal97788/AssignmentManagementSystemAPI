using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models
{
    public class StudentRoutineDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        public List<AssignmentRoutineViewModel> RoutineListByLoggedInStudentId(int? studentid)
        {
            List<AssignmentRoutineViewModel> lstassrt = new List<AssignmentRoutineViewModel>();
            var assroutine = _db.tblStudentRoutineRelations.Where(a => a.StudentId == studentid).ToList();


            foreach (var item in assroutine)
            {
                int? Submittedstaus=0, checkedstatus=0, marking=0, feedbackstatus=0, location=0; 
                var assignment = _db.tblSubmittedAssignments.Where(a => a.Student_Id == studentid && a.Routine_Id == item.RoutineId).ToList();
                bool submittedassignment = _db.tblSubmittedAssignments.Any(a => a.Student_Id == studentid && a.Routine_Id == item.RoutineId);
              
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
                lstassrt.Add(new AssignmentRoutineViewModel() { Id = item.tblAssignmentRoutine.Routine_Id, Assignment_Name = item.tblAssignmentRoutine.Assignment_Name, Teacher_Id = item.tblAssignmentRoutine.tblTeacher.Teacher_Id, Teacher_Name = item.tblAssignmentRoutine.tblTeacher.Name, Assignment_Release_Date = item.tblAssignmentRoutine.Assignment_Release_Date, Deadline = item.tblAssignmentRoutine.Deadline, Section_Id = item.tblAssignmentRoutine.tblSection.Section_Id, Section_Name = item.tblAssignmentRoutine.tblSection.Sec_Name, Faculty_Id = item.tblAssignmentRoutine.tblFaculty.Faculty_Id, Faculty_Name = item.tblAssignmentRoutine.tblFaculty.Faculty_Name, Semester_Id = item.tblAssignmentRoutine.tblSemester.Semester_Id, Semester_Name = item.tblAssignmentRoutine.tblSemester.Semester_Name, Year_Batch_Id = item.tblAssignmentRoutine.YearBatchId, Year_Batch = item.tblAssignmentRoutine.tblYearBatch.Year_Batch, SubmittedStatus = Submittedstaus,CheckedStatus= checkedstatus, FeedbackStatus=feedbackstatus, Marking=marking, AssignmentLoaction=location,Name=item.tblAssignmentRoutine.Name });
            }
            return lstassrt;
        }

        public int ChangePassword(int? studentid, ChangePasswordViewModel cvm )
        {
            try
            {
                tblStudent tb = _db.tblStudents.Where(s => s.Student_Id == studentid &&s.Password==cvm.OldPassword).FirstOrDefault();
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
        public bool CheckUser(int? studentid, string  password)
        {

            var isExist = _db.tblStudents.Where(s => s.Student_Id == studentid && s.Password==password).FirstOrDefault();
            if (isExist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
