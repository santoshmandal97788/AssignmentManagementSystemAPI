//using AssignmentSystem.Filters;
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
    public class SectionController : ApiController
    {
        SectionDB secDB = new SectionDB();
        [System.Web.Http.Route("api/Section/List")]
        public List<SectionViewModel> Get()
        {
            return secDB.ListAll();
        }
        public JsonResult GetSectionList(string sortColumnName = "Sec_Name", string sortOrder = "asc", int pageSize = 3, int currentPage = 1, string searchText = "")
        {
            List<SectionViewModel> List = new List<SectionViewModel>();
            int totalPage = 0;
            int totalRecord = 0;


            using (AssignmentManagementSystemEntities dc = new AssignmentManagementSystemEntities())
            {
                dc.Configuration.ProxyCreationEnabled = false;
                var emp = secDB.ListAll().Select(a => a);
                //Search
                if (!string.IsNullOrEmpty(searchText))
                {
                    emp = emp.Where(x => x.Sec_Name.Contains(searchText));
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


        public SectionViewModel Get(int ID)
        {
            var section = secDB.ListAll().Find(s => s.Id.Equals(ID));
            return section;
        }
        public HttpResponseMessage Post([Bind(Include = "Sec_Name,Model")]SectionViewModel svm)
        {
            //AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            string message = "New Section Added Successfully";
            bool isexists = secDB.CheckSectionNameExist(svm.Sec_Name);
            if (isexists)
            {
                ModelState.AddModelError("Sec_Name", "Section Name Already Exist");
            }
            if (ModelState.IsValid)
            {
                secDB.Add(svm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        public HttpResponseMessage Put([Bind(Include = "Sec_Name,Model")]SectionViewModel svm)
        {
            AssignmentManagementSystemEntities db = new AssignmentManagementSystemEntities();
            var section = secDB.ListAll().Find(s => s.Id.Equals(svm.Id));
            string message = "Section Updated Successfully";
            tblSection tb = db.tblSections.Where(s => s.Section_Id == svm.Id).FirstOrDefault();
            bool isexists = secDB.CheckSectionNameExist(svm.Sec_Name);
            if (isexists)
            {
                if (tb.Sec_Name == svm.Sec_Name)
                {
                    if (ModelState.IsValid)
                    {
                        secDB.Update(svm);
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("Sec_Name", "Section Name Already Exist");
                }
            }

            if (ModelState.IsValid)
            {
                secDB.Update(svm);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }
        public HttpResponseMessage Delete(int ID)
        {
            var section = secDB.ListAll().Find(s => s.Id.Equals(ID));
            string message = "Section Deleted Successfully";
            string messagenodata = "Data Not found/ Might be Deleted or Removed";
            HttpResponseMessage response;
            if (section == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messagenodata);
            }
            else
            {
                secDB.Delete(ID);
                response = Request.CreateResponse(HttpStatusCode.OK, message);
            }
            return response;
        }

    }
}

