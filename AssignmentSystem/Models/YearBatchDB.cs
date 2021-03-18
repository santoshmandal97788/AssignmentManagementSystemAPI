using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AssignmentSystem.Models
{
    public class YearBatchDB
    {
        AssignmentManagementSystemEntities _db = new AssignmentManagementSystemEntities();
        string str;
        public List<YearBatchViewModel> ListAll()
        {
            List<YearBatchViewModel> lstyear = new List<YearBatchViewModel>();
            var yearbatch = _db.tblYearBatches.ToList();
            foreach (var item in yearbatch)
            {
                lstyear.Add(new YearBatchViewModel() { Id = item.YearBatchId, Year_Batch = item.Year_Batch });
            }
            return lstyear;
        }
        public int Add(YearBatchViewModel yvm)
        {
            tblYearBatch tb = new tblYearBatch();
            tb.Year_Batch = yvm.Year_Batch;
            _db.tblYearBatches.Add(tb);
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "New Yearbatch '" + yvm.Year_Batch + "' " + "Added by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 5;
            tml.ItemId = tb.YearBatchId;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        public int Update(YearBatchViewModel yvm)
        {
            tblYearBatch tb = _db.tblYearBatches.Where(y => y.YearBatchId == yvm.Id).FirstOrDefault();
            var yearName = tb.Year_Batch;
            tb.Year_Batch = yvm.Year_Batch;
            _db.SaveChanges();
            tblMainLog tml = new tblMainLog();
            var principal = System.Security.Claims.ClaimsPrincipal.Current;
            string Name = principal.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value;
            str = "YearBatch Updated from '" + yearName + "' " + "to '" + yvm.Year_Batch + "' " + "by Admin";
            tml.Description = str;
            tml.AdminName = Name;
            tml.Date = System.DateTime.Now;
            tml.EntityId = 5;
            tml.ItemId = tb.YearBatchId;
            _db.tblMainLogs.Add(tml);
            return _db.SaveChanges();
        }
        //public int Delete(int ID)
        //{
        //    tblYearBatch tb = _db.tblYearBatches.Where(y => y.YearBatchId == ID).FirstOrDefault();
        //    _db.tblYearBatches.Remove(tb);
        //    return _db.SaveChanges();
        //}
        public bool CheckYearbatch(string yearbatch)
        {
            var isExist = _db.tblYearBatches.Where(y => y.Year_Batch == yearbatch).FirstOrDefault();
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