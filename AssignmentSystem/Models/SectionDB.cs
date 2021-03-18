using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace AssignmentSystem.Models
{
    public class SectionDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        string str;        
        public List<SectionViewModel> ListAll()
        {
            List<SectionViewModel> lstsec = new List<SectionViewModel>();
            var section = _db.tblSections.ToList();
            foreach (var item in section)
            {
                lstsec.Add(new SectionViewModel() {Id = item.Section_Id, Sec_Name = item.Sec_Name});
            }
            return lstsec;
        }
        public int Add(SectionViewModel svm)
        {
            tblSection tb = new tblSection();
            tb.Sec_Name = svm.Sec_Name;
            _db.tblSections.Add(tb);
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "New Section '" + svm.Sec_Name + "' " + "Added by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 8;
            tml.ItemId = tb.Section_Id;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        public int Update(SectionViewModel svm)
        {
            tblSection tb = _db.tblSections.Where(s => s.Section_Id == svm.Id).FirstOrDefault();
            var secName = tb.Sec_Name;
            tb.Sec_Name = svm.Sec_Name;
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "Section Name Updated from '" +secName +"' " + "to '" + svm.Sec_Name + "' " + "by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 8;
            tml.ItemId = tb.Section_Id;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        public int Delete(int ID)
        {
            tblSection tb = _db.tblSections.Where(s => s.Section_Id == ID).FirstOrDefault();
            _db.tblSections.Remove(tb);
            return _db.SaveChanges();
        }
        public bool CheckSectionNameExist(string SectionName)
        {
            var isExist = _db.tblSections.Where(s => s.Sec_Name == SectionName).FirstOrDefault();
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