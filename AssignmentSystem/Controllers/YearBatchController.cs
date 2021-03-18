using AssignmentSystem.Models;
using AssignmentSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace AssignmentSystem.Controllers
{
    [AuthorizeAttribute]
    [Authorize(Roles = "admin")]
    public class YearBatchController : ApiController
    {
        YearBatchDB yearDB = new YearBatchDB();
        [System.Web.Http.Route("api/YearBatch/List")]
        public List<YearBatchViewModel> Get()
        {
            return yearDB.ListAll();
        }
        public JsonResult GetYearBatchList(string sortColumnName = "Year_Batch", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<YearBatchViewModel> List = new List<YearBatchViewModel>();
            int totalPage = 0;
            int totalRecord = 0;


            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                var emp = yearDB.ListAll().Select(a => a);
                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(x => x.Year_Batch.Contains(searchText));
                }
                totalRecord = emp.Count();
                if (pageSize > 0)
                {
                    totalPage = totalRecord / pageSize + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    List = emp.OrderBy(sortColumnName + " " + sortOrder).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                }
                else
                {
                    List = emp.ToList();
                }
            }

            return new JsonResult
            {
                //Data = new { List = List, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage},
                Data = new { List = List, totalRecord = totalRecord, totalPage = totalPage, sortColumnName = sortColumnName, sortOrder = sortOrder, currentPage = currentPage, pageSize = pageSize },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public YearBatchViewModel Get(int ID)
        {
            var yearbatch = yearDB.ListAll().Find(s => s.Id.Equals(ID));
            return yearbatch;
        }

        public HttpResponseMessage Post([Bind(Include = "Year_Batch,Model")]YearBatchViewModel yvm)
        {
            //AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            string message = "New Year/Batch Added Successfully";
            bool isexists = yearDB.CheckYearbatch(yvm.Year_Batch);
            if (isexists)
            {
                ModelState.AddModelError("Year_Batch", "Year/Batch Already Exist");
            }
            if (ModelState.IsValid)
            {
                yearDB.Add(yvm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public HttpResponseMessage Put([Bind(Include = "Year_Batch,Model")]YearBatchViewModel yvm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            var year = yearDB.ListAll().Find(y => y.Id.Equals(yvm.Id));
            string message = "Year/Batch Updated Successfully";
            tblYearBatch tb = db.tblYearBatches.Where(y => y.YearBatchId == yvm.Id).FirstOrDefault();
            bool isexists = yearDB.CheckYearbatch(yvm.Year_Batch);
            if (isexists)
            {
                if (tb.Year_Batch == yvm.Year_Batch)
                {
                    if (ModelState.IsValid)
                    {
                        yearDB.Update(yvm);
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("Year_Batch", "Year/Batch Already Exist");
                }
            }

            if (ModelState.IsValid)
            {
                yearDB.Update(yvm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }
        //public HttpResponseMessage Delete(int ID)
        //{
        //    var year = yearDB.ListAll().Find(s => s.Id.Equals(ID));
        //    string message = "Year/Batch Deleted Successfully";
        //    string messagenodata = "Data Not found/ Might be Deleted or Removed";
        //    HttpResponseMessage response;
        //    if (year == null)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest, messagenodata);
        //    }
        //    else
        //    {
        //        yearDB.Delete(ID);
        //        response = Request.CreateResponse(HttpStatusCode.OK, message);
        //    }
        //    return response;
        //}

    }
}

