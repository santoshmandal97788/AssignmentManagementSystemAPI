using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models
{
    public class SubmittedAssignmentDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();

        public  List<SubmittedAssignmentViewModel> ListAll()
        {
            List<SubmittedAssignmentViewModel> lstsub = new List<SubmittedAssignmentViewModel>();
            List<SubmittedAssignmentViewModel> lstsub1 = new List<SubmittedAssignmentViewModel>();
            List<tblSubmittedAssignment> asssubmits = _db.tblSubmittedAssignments.ToList();
            //List<tblSubmittedAssignment> asssubmit = _db.tblSubmittedAssignments.Where(a=>a.Submitted_Date<=a.tblAssignmentRoutine.Deadline).ToList();
            //List<tblSubmittedAssignment> asssubmit1 = _db.tblSubmittedAssignments.Where(a => a.Submitted_Date > a.tblAssignmentRoutine.Deadline).ToList();

            //if (asssubmit!=null && asssubmit1 != null)
            //{
                foreach (var item in asssubmits)
                {
                    int submitStatus;
                    if (item.Submitted_Date <= item.tblAssignmentRoutine.Deadline)
                    {
                        submitStatus = 1;
                    }
                    else
                    {
                        submitStatus = 0;
                    }

                    lstsub.Add(new SubmittedAssignmentViewModel() { Id = item.ID, Routine_Id = item.tblAssignmentRoutine.Routine_Id, Assignment_Name = item.tblAssignmentRoutine.Assignment_Name, TeacherName = item.tblAssignmentRoutine.tblTeacher.Name, Section_Id = item.tblAssignmentRoutine.tblSection.Section_Id, SectionName = item.tblAssignmentRoutine.tblSection.Sec_Name, Faculty_Id = item.tblAssignmentRoutine.tblFaculty.Faculty_Id, FacultyName = item.tblAssignmentRoutine.tblFaculty.Faculty_Name, Semester_Id = item.tblAssignmentRoutine.tblSemester.Semester_Id, SemesterName = item.tblAssignmentRoutine.tblSemester.Semester_Name, Year_Batch_Id = item.tblAssignmentRoutine.tblYearBatch.YearBatchId, YearBatch = item.tblAssignmentRoutine.tblYearBatch.Year_Batch, Student_Id = item.tblStudent.Student_Id, StudentName = item.tblStudent.Name, Submitted_Date = item.Submitted_Date, Checked_Status = item.Checked_Status, Feedback_Status = item.Feedback_Status, Marking = item.Marking, Assignmnet_Location = item.Assignmnet_Location, onTime= submitStatus});
                }
                //List<SubmittedAssignmentViewModel> lstupdate=  new List<SubmittedAssignmentViewModel>();
                //lstupdate = lstsub;
                //foreach (var item in asssubmit1)
                //{
                //    lstupdate.Add(new SubmittedAssignmentViewModel() { Id = item.ID, Routine_Id = item.tblAssignmentRoutine.Routine_Id, Assignment_Name = item.tblAssignmentRoutine.Assignment_Name, TeacherName = item.tblAssignmentRoutine.tblTeacher.Name, Section_Id = item.tblAssignmentRoutine.tblSection.Section_Id, SectionName = item.tblAssignmentRoutine.tblSection.Sec_Name, Faculty_Id = item.tblAssignmentRoutine.tblFaculty.Faculty_Id, FacultyName = item.tblAssignmentRoutine.tblFaculty.Faculty_Name, Semester_Id = item.tblAssignmentRoutine.tblSemester.Semester_Id, SemesterName = item.tblAssignmentRoutine.tblSemester.Semester_Name, Year_Batch_Id = item.tblAssignmentRoutine.tblYearBatch.YearBatchId, YearBatch = item.tblAssignmentRoutine.tblYearBatch.Year_Batch, Student_Id = item.tblStudent.Student_Id, StudentName = item.tblStudent.Name, Submitted_Date = item.Submitted_Date, Checked_Status = item.Checked_Status, Feedback_Status = item.Feedback_Status, Marking = item.Marking, Assignmnet_Location = item.Assignmnet_Location, onTime = 1 });
                //}
                //lstsub1= lstupdate;
           
            //}
            return lstsub;







            //else
            //{
            //    foreach (var item in asssubmit1)
            //    {
            //        lstsub.Add(new SubmittedAssignmentViewModel() { Id = item.ID, Routine_Id = item.tblAssignmentRoutine.Routine_Id, Assignment_Name = item.tblAssignmentRoutine.Assignment_Name, TeacherName = item.tblAssignmentRoutine.tblTeacher.Name, Section_Id = item.tblAssignmentRoutine.tblSection.Section_Id, SectionName = item.tblAssignmentRoutine.tblSection.Sec_Name, Faculty_Id = item.tblAssignmentRoutine.tblFaculty.Faculty_Id, FacultyName = item.tblAssignmentRoutine.tblFaculty.Faculty_Name, Semester_Id = item.tblAssignmentRoutine.tblSemester.Semester_Id, SemesterName = item.tblAssignmentRoutine.tblSemester.Semester_Name, Year_Batch_Id = item.tblAssignmentRoutine.tblYearBatch.YearBatchId, YearBatch = item.tblAssignmentRoutine.tblYearBatch.Year_Batch, Student_Id = item.tblStudent.Student_Id, StudentName = item.tblStudent.Name, Submitted_Date = item.Submitted_Date, Checked_Status = item.Checked_Status, Feedback_Status = item.Feedback_Status, Marking = item.Marking, Assignmnet_Location = item.Assignmnet_Location, onTime = 1 });
            //    }
            //    return lstsub;
            //}

        }
        public int Add(SubmittedAssignmentViewModel sub)
        {
            tblSubmittedAssignment tb = new tblSubmittedAssignment();
            tb.Routine_Id = sub.Routine_Id;
            tb.Student_Id = sub.Student_Id;
            tb.Submitted_Date = System.DateTime.Now;
            tb.Checked_Status = 0;
            tb.Feedback_Status = 0;
            tb.Marking = 0;
            tb.Assignmnet_Location = 0;
            _db.tblSubmittedAssignments.Add(tb);
            _db.SaveChanges();
            tblLog tl = new tblLog();
            tl.Date = DateTime.Now;
            tl.ActivityId = 1;
            tl.SubmittedAssignmentId = tb.ID;
            _db.tblLogs.Add(tl);
            return _db.SaveChanges();
        }
        public int Update(SubmittedAssignmentViewModel sub)
        {
            ////tblSubmittedAssignment tb = _db.tblSubmittedAssignments.Where(a => a.ID == sub.Id).FirstOrDefault();
            ////tb.Routine_Id = sub.Routine_Id;
            ////tb.Student_Id = sub.Student_Id;
            ////tb.Submitted_Date = tb.Submitted_Date;
            ////tb.Checked_Status = sub.Checked_Status;
            ////tb.Feedback_Status = sub.Feedback_Status;
            ////tb.Assignmnet_Location = sub.Assignmnet_Location;
            ////tb.Marking = sub.Marking;
            ////_db.SaveChanges();
            ////tblLog tl = new tblLog();
            ////if (tb.Assignmnet_Location == 1)
            ////{
            ////    tl.Date = DateTime.Now;
            ////    tl.ActivityId = 2;
            ////    tl.SubmittedAssignmentId = tb.ID;
            ////    _db.tblLogs.Add(tl);
            ////    return _db.SaveChanges();
            ////}
            ////else if (tb.Assignmnet_Location == 0)
            ////{
            ////    tl.Date = DateTime.Now;
            ////    tl.ActivityId = 3;
            ////    tl.SubmittedAssignmentId = tb.ID;
            ////    _db.tblLogs.Add(tl);
            ////    return _db.SaveChanges();
            ////}
            ////else if (tb.Assignmnet_Location == 2)
            ////{
            ////    tl.Date = DateTime.Now;
            ////    tl.ActivityId = 4;
            ////    tl.SubmittedAssignmentId = tb.ID;
            ////    _db.tblLogs.Add(tl);
            ////    return _db.SaveChanges();
            ////}
            ////else
            ////{
            ////    tl.Date = DateTime.Now;
            ////    tl.ActivityId = 4;
            ////    tl.SubmittedAssignmentId = tb.ID;
            ////    _db.tblLogs.Add(tl);
            ////    return _db.SaveChanges();
            ////}
            tblSubmittedAssignment tb = _db.tblSubmittedAssignments.Where(a => a.ID == sub.Id).FirstOrDefault();
            tblLog tl = new tblLog();
            if (tb.Assignmnet_Location == 0 && sub.Assignmnet_Location == 1)
            {
                tl.Date = DateTime.Now;
                tl.ActivityId = 2;
                tl.SubmittedAssignmentId = tb.ID;
                _db.tblLogs.Add(tl);

            }
            if (tb.Assignmnet_Location == 1 && sub.Assignmnet_Location == 0)
            {
                tl.Date = DateTime.Now;
                tl.ActivityId = 3;
                tl.SubmittedAssignmentId = tb.ID;
                _db.tblLogs.Add(tl);

            }
            if (tb.Assignmnet_Location == 0 && sub.Assignmnet_Location == 2)
            {
                tl.Date = DateTime.Now;
                tl.ActivityId = 4;
                tl.SubmittedAssignmentId = tb.ID;
                _db.tblLogs.Add(tl);

            }
            if (tb.Assignmnet_Location == 2 && sub.Assignmnet_Location == 0)
            {
                tl.Date = DateTime.Now;
                tl.ActivityId = 5;
                tl.SubmittedAssignmentId = tb.ID;
                _db.tblLogs.Add(tl);

            }
            tb.Routine_Id = sub.Routine_Id;
            tb.Student_Id = sub.Student_Id;
            tb.Submitted_Date = tb.Submitted_Date;
            tb.Checked_Status = sub.Checked_Status;
            tb.Feedback_Status = sub.Feedback_Status;
            tb.Marking = sub.Marking;
            tb.Assignmnet_Location = sub.Assignmnet_Location;
            return _db.SaveChanges();


        }
        public int Delete(int ID)
        {
            tblSubmittedAssignment tb = _db.tblSubmittedAssignments.Where(a => a.ID == ID).FirstOrDefault();
            _db.tblSubmittedAssignments.Remove(tb);
            return _db.SaveChanges();
        }
        public bool StudentAssignmentMatch(int? StudentId, int? RoutineId)
        {
            tblStudent ts = _db.tblStudents.Where(s => s.Student_Id == StudentId).FirstOrDefault();
            tblAssignmentRoutine tr = _db.tblAssignmentRoutines.Where(r => r.Routine_Id == RoutineId).FirstOrDefault();
            if (ts.Faculty_Id == tr.Faculty_Id && ts.YearBatchId == tr.YearBatchId /*&& ts.Section_Id == tr.Section_Id && ts.Semester_Id == tr.Semester_Id*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public bool AssignmentLocationCheck(SubmittedAssignmentViewModel sub)
        //{
        //    tblSubmittedAssignment tb = _db.tblSubmittedAssignments.Where(a => a.ID == sub.Id).FirstOrDefault();
        //    if (tb.Assignmnet_Location==1 && sub.Assignmnet_Location==2)
        //    {
        //        return true;
        //    }
        //    else if (tb.Assignmnet_Location == 2 && sub.Assignmnet_Location == 1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public List<SubmittedAssignmentViewModel> ListAllByStudent(int studentid)
        {
            List<SubmittedAssignmentViewModel> lstsub = new List<SubmittedAssignmentViewModel>();
            var asssubmit = _db.tblSubmittedAssignments.Where(a => a.Student_Id == studentid).ToList();
            foreach (var item in asssubmit)
            {
                lstsub.Add(new SubmittedAssignmentViewModel() { Id = item.ID, Routine_Id = item.tblAssignmentRoutine.Routine_Id, Assignment_Name = item.tblAssignmentRoutine.Assignment_Name, TeacherName = item.tblAssignmentRoutine.tblTeacher.Name, Section_Id=item.tblAssignmentRoutine.tblSection.Section_Id, SectionName = item.tblAssignmentRoutine.tblSection.Sec_Name, Faculty_Id = item.tblAssignmentRoutine.tblFaculty.Faculty_Id, FacultyName = item.tblAssignmentRoutine.tblFaculty.Faculty_Name, Semester_Id = item.tblAssignmentRoutine.tblSemester.Semester_Id, SemesterName = item.tblAssignmentRoutine.tblSemester.Semester_Name, Year_Batch_Id = item.tblAssignmentRoutine.tblYearBatch.YearBatchId, YearBatch = item.tblAssignmentRoutine.tblYearBatch.Year_Batch, Student_Id = item.tblStudent.Student_Id, StudentName = item.tblStudent.Name, Submitted_Date = item.Submitted_Date, Checked_Status = item.Checked_Status, Feedback_Status = item.Feedback_Status, Marking = item.Marking, Assignmnet_Location = item.Assignmnet_Location });
            }
            return lstsub;
        }
        public List<LogViewModel> LogListByAssignmentId(int assignmentid)
        {
            List<LogViewModel> lstlog = new List<LogViewModel>();
            var log = _db.tblLogs.Where(a => a.SubmittedAssignmentId == assignmentid).ToList();
            foreach (var item in log)
            {
                lstlog.Add(new LogViewModel() { Date =  item.Date, Description=item.tblActivity.Description });
            }
            return lstlog;
        }

    }
}