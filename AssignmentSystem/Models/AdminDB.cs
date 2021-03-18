using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentSystem.Models
{
    public class AdminDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        public List<Admin> ListAll()
        {
            try
            {
                List<Admin> lstadmin = new List<Admin>();
                var admin = _db.tblAdmins.ToList();
                foreach (var item in admin)
                {
                    lstadmin.Add(new Admin() { Admin_Id = item.Admin_Id, Name = item.Name, Email = item.Email, Password = item.Password, Role = item.Role });
                }
                return lstadmin;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int Add(Admin adm)
        {
            try
            {
                tblAdmin tb = new tblAdmin();
                tb.Name = adm.Name;
                tb.Email = adm.Email;
                tb.Password = adm.Password;
                tb.Role = adm.Role;
                _db.tblAdmins.Add(tb);
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        public int Update(Admin adm)
        {
            try
            {
                tblAdmin tb = _db.tblAdmins.Where(a => a.Admin_Id == adm.Admin_Id).FirstOrDefault();
                tb.Name = adm.Name;
                tb.Email = adm.Email;
                tb.Password = adm.Password;
                tb.Role = adm.Role;
                return _db.SaveChanges();
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
                tblAdmin tb = _db.tblAdmins.Where(a => a.Admin_Id == ID).FirstOrDefault();
                _db.tblAdmins.Remove(tb);
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
    }
}