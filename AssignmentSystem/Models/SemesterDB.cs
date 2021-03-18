using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AssignmentSystem.Models
{
    public class SemesterDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        string str;
        public List<SemesterViewModel> ListAll()
        {
            List<SemesterViewModel> lstsem = new List<SemesterViewModel>();
            var semester = _db.tblSemesters.ToList();
            foreach (var item in semester)
            {
                lstsem.Add(new SemesterViewModel() { Id = item.Semester_Id, Semester_Name = item.Semester_Name });
            }
            return lstsem;
        }
        public int Add(SemesterViewModel svm)
        {
            tblSemester tb = new tblSemester();
            tb.Semester_Name = svm.Semester_Name;
            _db.tblSemesters.Add(tb);
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "New Semester '" + svm.Semester_Name + "' " + "Added by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 7;
            tml.ItemId = tb.Semester_Id;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        public int Update(SemesterViewModel svm)
        {
            tblSemester tb = _db.tblSemesters.Where(s => s.Semester_Id == svm.Id).FirstOrDefault();
            var semesterName = tb.Semester_Name;
            tb.Semester_Name = svm.Semester_Name;
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "Semester Name Updated from '" + semesterName + "' " + "to '" + svm.Semester_Name + "' " + "by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 7;
            tml.ItemId = tb.Semester_Id;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        public int Delete(int ID)
        {
            try
            {
                tblSemester tb = _db.tblSemesters.Where(s => s.Semester_Id == ID).FirstOrDefault();
                _db.tblSemesters.Remove(tb);
                return _db.SaveChanges();
            }
            catch (Exception Error)
            {

                throw Error ;
            }
           
        }
        public bool CheckSemesterExist(string SemesterName)
        {
            var isExist = _db.tblSemesters.Where(s => s.Semester_Name == SemesterName).FirstOrDefault();
            if (isExist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}