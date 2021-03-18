using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AssignmentSystem.Models
{
    public class FacultyDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        string str;
        public List<FacultyViewModel> ListAll()
        {
            List<FacultyViewModel> lstfac = new List<FacultyViewModel>();
            var faculty = _db.tblFaculties.ToList();
            foreach (var item in faculty)
            {
                lstfac.Add(new FacultyViewModel() { Id = item.Faculty_Id, Faculty_Name = item.Faculty_Name });
            }
            return lstfac;
        }
        public int Add(FacultyViewModel fvm)
        {
            tblFaculty tb = new tblFaculty();
            tb.Faculty_Name = fvm.Faculty_Name;
            _db.tblFaculties.Add(tb);
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "New Faculty '" + fvm.Faculty_Name + "' " + "Added by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 6;
            tml.ItemId = tb.Faculty_Id;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        public int Update(FacultyViewModel fvm)
        {
            tblFaculty tb = _db.tblFaculties.Where(f => f.Faculty_Id == fvm.Id).FirstOrDefault();
            var facName = tb.Faculty_Name;
            tb.Faculty_Name = fvm.Faculty_Name;
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "Faculty Name Updated from '" + facName + "' " + "to '" + fvm.Faculty_Name + "' " + "by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 6;
            tml.ItemId = tb.Faculty_Id;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        public int Delete(int ID)
        {
            tblFaculty tb = _db.tblFaculties.Where(f => f.Faculty_Id == ID).FirstOrDefault();
            _db.tblFaculties.Remove(tb);
            return _db.SaveChanges();
        }
        public bool CheckFacultyNameExist(string FacultyName)
        {
            var isExist = _db.tblFaculties.Where(f => f.Faculty_Name == FacultyName).FirstOrDefault();
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