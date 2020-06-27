using Application.Controllers;
using BLL.Common;
using BLL.Factory.Inventory;
using BLL.Factory.Security;
using BLL.Interfaces.Inventory;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;
using InvogueApp.Areas.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class IssueController : Controller
    {
        // GET: Inventory/Issue
        DevourInvEntities db = new DevourInvEntities();
        CommonController common = new CommonController();
        private IIssueItemFactory issueFactory;
        private IItemReturnFactory itemReturnFactory;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();

        private Result result;
        private IStoreReqFactory storeReqFactory;

        DateTime todayDate = DateTime.Now;
        public ActionResult IssueList()
        {
            return common.SelectPermission("Inventory", "Issue");

            //ViewBag.CallingForm = "Inventory";
            //ViewBag.CallingForm1 = "Issue";
            //ViewBag.CallingViewPage = "#";
            //return View();
        }
        
        public ActionResult IssueCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Issue";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/IssueList";
        }
        [HttpPost]
        public JsonResult LoadStoreReq(int? storeID, int? departmentID, DateTime? fromDate , DateTime? toDate)
        {
            //issueFactory = new IssueItemFactorys();
            //int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            //var userID = db.SEC_UserInformation.Where(x => x.ID == loginUserID).FirstOrDefault();
            //var deptName = db.INV_Department.Where(x => x.DepartmentID == userID.DepartmentID).FirstOrDefault();

            //List<INV_SR> list = issueFactory.SearchStoreReq(storeID);
            //var storeReqList = list.Select(x => new
            //{
            //    x.SRID,
            //    x.BranchID,
            //    x.SRNO,
            //    x.SET_CompanyBranch.Name,
            //    x.RequisitionDate,
            //    x.RequiredDate,
            //    x.CreatedDate,
            //    x.Status,
            //    x.SEC_UserInformation.DepartmentID,
            //    DepartmentName = deptName.Name,
            //    x.CreatedBy

            //}).Where(x => x.Status == "2A" || x.Status == "IP").ToList();

            DevourInvEntities db = new DevourInvEntities();
            var storeReqList = (from x in db.INV_SR
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on x.BranchID equals cb.BranchID
                where x.Status == "2A" || x.Status == "IP"
                select new
                {
                    x.SRID,
                    x.BranchID,
                    x.SRNO,
                    cb.Name,
                    x.RequisitionDate,
                    x.RequiredDate,
                    x.CreatedDate,
                    x.Status,
                    u.DepartmentID,
                    DepartmentName = d.Name,
                    x.CreatedBy
                }).ToList();

            if (storeID > 0)
            {
                storeReqList = storeReqList.Where(x => x.BranchID == storeID).ToList();
            }
            if(fromDate != null && toDate != null){
                storeReqList = storeReqList.Where(x => x.RequisitionDate >= fromDate && x.RequisitionDate <= toDate).ToList();
            }
            if (departmentID > 0)
            {
                storeReqList = storeReqList.Where(x => x.DepartmentID == departmentID).ToList();
            }
            return Json(storeReqList.OrderByDescending(x => x.SRID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadIssue(int? srID)
        {
            itemReturnFactory = new ItemReturnFactorys();
            List<INV_Issue> list = itemReturnFactory.SearchIssue(srID);
            var storeReqList = list.Select(x => new
            {
                x.SRID,
                x.IssueDate,
                x.IssueNO,
                x.IssueID,
                x.Status,
                x.CreatedDate
               
            }).FirstOrDefault();
            return Json(storeReqList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadStoreDtls(int? sRID)
        {
            storeReqFactory = new StoreReqFactorys();
            SqlParameter p1 = new SqlParameter("@P1", sRID);
            var storeReqList = db.Database.SqlQuery<VM_IssueDtls>("sp_IssueList @p1", p1).ToList();

            //List<INV_SRDtls> list = storeReqFactory.SearchStoreReqDtls(sRID);
            //var storeReqList = list.Select(x => new
            //{
            //  x.ProductID,
            //  x.ReqQty,
            //  x.SRID,
            //  x.SRDtlsID,
            //  x.UnitID,
            //  x.IssueQty,
            //  x.ApprovedQty,
            //  x.CategoryID,
            //CategoryName= x.INV_Category.Name,
            // ProductName=x.INV_Product.Name,
            // UnitNAme = x.INV_Unit.Name,
              
            //});
            return Json(storeReqList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveIssue(INV_Issue issue, List<VM_TempIssueDetails> issueDtls)
        {
            result = new Result();
            issueFactory = new IssueItemFactorys();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (issue.IssueID > 0)
            {

                issue.UpdatedBy = userID;
                issue.UpdatedDate = todayDate;
            }
            else
            {
                
                issue.CreatedBy = userID;
                issue.CreatedDate = todayDate;
            }

            result = issueFactory.SaveIssueItem(issue, issueDtls);
            return Json(result);
        }
    }
}