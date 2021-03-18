using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace AssignmentSystem.Models
{
    public  class TotalCount
    {
        public  AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        public  int GetTotalStudent()
        {
            return _db.tblStudents.Count();
        }
        public  int GetTotalTeacher()
        {
            return _db.tblTeachers.Count();
        }
        public  int GetTotalSubmittedAssignment()
        {
            return _db.tblSubmittedAssignments.Count();
        }
        public  int GetTotalRoutine()
        {
            return _db.tblAssignmentRoutines.Count();
        }
        //Total Pass Students
        public  int GetTotalPassStudent(int? a, int? b, int? c, int? d)
        {
            List<tblSubmittedAssignment> totalAssignment = GetTotalSubmittedAssignmentFilter(a, b, c, d);
            return totalAssignment.Where(p => p.Marking == 2).Count();
        }
        //Total Merit Holder Students
        public  int GetTotalMeritStudent(int? a, int? b, int? c, int? d)
        {
            List<tblSubmittedAssignment> totalAssignment = GetTotalSubmittedAssignmentFilter(a, b, c, d);
            return totalAssignment.Where(p => p.Marking == 3).Count();
        }
        //Total Distinction holder Students
        public  int GetTotalDistictionStudent(int? a, int? b, int? c, int? d)
        {
            List<tblSubmittedAssignment> totalAssignment = GetTotalSubmittedAssignmentFilter(a, b, c, d);
            return totalAssignment.Where(p => p.Marking == 4).Count();
        }
        //Total Fail Students
        public  int GetTotalFailStudent(int? a, int? b, int? c, int? d)
        {
            List<tblSubmittedAssignment> totalAssignment = GetTotalSubmittedAssignmentFilter(a, b, c, d);
            return totalAssignment.Where(p => p.Marking == 1).Count();
        }
        public  List<tblSubmittedAssignment> GetTotalSubmittedAssignmentFilter(int? yearid, int? facultyid, int? semesterid,int? sectionid)
        {
            var filterByYear = _db.tblSubmittedAssignments.ToList();
            var filterByfaculty = filterByYear;
            var filterBysemester = filterByYear;
            var filterBysection = filterByYear;
            if (yearid!=null)
            {
                filterByYear=_db.tblSubmittedAssignments.ToList().Where(e => e.tblStudent.YearBatchId == yearid).ToList();
            }
            else
            {
                filterByYear = _db.tblSubmittedAssignments.ToList();
            }
            if (facultyid!=null)
            {
                filterByfaculty= filterByYear.Where(e => e.tblStudent.Faculty_Id == facultyid).ToList();
            }
            else
            {
                filterByfaculty = filterByYear;
            }
            if (semesterid != null)
            {
                filterBysemester = filterByfaculty.Where(e => e.tblStudent.Semester_Id == semesterid).ToList();
            }
            else
            {
                filterBysemester = filterByfaculty;
            }
            if (sectionid != null)
            {
                filterBysection = filterBysemester.Where(e => e.tblStudent.Section_Id == sectionid).ToList();
            }
            else
            {
                filterBysection  = filterBysemester;
            }
            return filterBysection;/*_db.tblSubmittedAssignments.ToList().Where(e=>e.tblStudent.YearBatchId==yearid && e.tblStudent.Faculty_Id==facultyid && e.tblStudent.Semester_Id==semesterid && e.tblStudent.Section_Id==sectionid).ToList();*/
        }
        

    }
}