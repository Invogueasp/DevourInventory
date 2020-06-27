using BLL.Common;
using BLL.Factory.Inventory;
using BLL.Interfaces.Inventory;
using DAL.db;
using DAL.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class StoreRequisitionController : Controller
    {
        // GET: Inventory/StoreRequisition
        private IStoreReqFactory storeReqFactory;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        DevourInvEntities db = new DevourInvEntities();
        private Result result;
    
        DateTime todayDate = DateTime.Now;
        public ActionResult StoreRequisitionList()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Store Requisition";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult StoreRequisitionCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Store Requisition";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/StoreRequisitionList";
        }


        [HttpPost]
        public JsonResult SaveStoreReq(INV_SR storeReq, List<INV_SRDtls> storeReqDtls, List<int> deleteStoreReqDtlsID)
        {
            result = new Result();
            storeReqFactory = new StoreReqFactorys();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (storeReq.SRID > 0)
            {
               
                storeReq.UpdatedBy = userID;
                storeReq.UpdatedDate = todayDate;
            }
            else
            {
                storeReq.Status = "P";
                storeReq.CreatedBy = userID;
                storeReq.CreatedDate = todayDate;
            }

            result = storeReqFactory.SaveStoreReq(storeReq, storeReqDtls, deleteStoreReqDtlsID);
            return Json(result);
        }


        [HttpPost]
        public JsonResult LoadStoreReq(int? srID, string status)
        {   
            //storeReqFactory = new StoreReqFactorys();
            //List<INV_SR> list = storeReqFactory.SearchStoreReq(srID);
            //int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            //string userName = db.SEC_UserInformation.Where(x=> x.ID== loginUserID).Select(x => x.UserFullName).ToList().FirstOrDefault();
            //var userID = db.SEC_UserInformation.Where(x => x.ID == loginUserID).FirstOrDefault();
            //var deptName = db.INV_Department.Where(x => x.DepartmentID == userID.DepartmentID).FirstOrDefault();
            //var productList = list.Select(x => new
            //{
            //    x.SRID,
            //    x.BranchID,
            //    x.SRNO,
            //    x.SET_CompanyBranch.Name,
            //    x.RequisitionDate,
            //    x.RequiredDate,
            //    x.CreatedDate,
            //    x.CreatedBy,
            //    x.UpdatedBy,
            //    x.UpdatedDate,
            //    UserFullName = userName,
            //    Department = deptName.Name,
            //    x.Status
            //});

            var productList = (from x in db.INV_SR
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on x.BranchID equals cb.BranchID
                select new
                {
                    x.SRID,
                    x.BranchID,
                    x.SRNO,
                    cb.Name,
                    x.RequisitionDate,
                    x.RequiredDate,
                    x.CreatedDate,
                    x.CreatedBy,
                    x.UpdatedBy,
                    x.UpdatedDate,
                    UserFullName = u.UserFullName,
                    Department = d.Name,
                    x.Status
                }).ToList();

            if (srID > 0)
            {
                productList = productList.Where(x => x.SRID == srID).ToList();
            }
            if (status != "")
            {
                productList = productList.Where(x => x.Status == status).ToList();
            }
            return Json(productList.OrderByDescending(x => x.SRID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadStore2ndAppReq(int? srID)
        {
            //DevourInvEntities db = new DevourInvEntities();
            //storeReqFactory = new StoreReqFactorys();
            //List<INV_SR> list = storeReqFactory.SearchStoreReq(srID);

            //Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            //int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            //string userName = db.SEC_UserInformation.Where(x => x.ID == loginUserID).Select(x => x.UserFullName).ToList().FirstOrDefault();
            //var productList = list.Select(x => new
            //{
            //    x.SRID,
            //    x.BranchID,
            //    x.SRNO,
            //    x.SET_CompanyBranch.Name,
            //    x.RequisitionDate,
            //    x.RequiredDate,
            //    x.CreatedDate,
            //    UserFullName = userName,
            //    x.Status
            //});
            var productList = (from x in db.INV_SR
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on x.BranchID equals cb.BranchID
                where x.Status == "A" || x.Status == "2A"
                select new
                {
                    x.SRID,
                    x.BranchID,
                    x.SRNO,
                    cb.Name,
                    x.RequisitionDate,
                    x.RequiredDate,
                    x.CreatedDate,
                    x.CreatedBy,
                    x.UpdatedBy,
                    x.UpdatedDate,
                    UserFullName = u.UserFullName,
                    Department = d.Name,
                    x.Status
                }).ToList();

            if (srID > 0)
            {
                productList = productList.Where(x => x.SRID == srID).ToList();
            }
            return Json(productList.OrderByDescending(x => x.SRID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadStoreDtls(int? sRID)
        {
            storeReqFactory = new StoreReqFactorys();
            List<INV_SRDtls> list = storeReqFactory.SearchStoreReqDtls(sRID);
            var storeDtlsList = list.Select(x => new
            {
                x.CategoryID,
                x.ProductID,
                x.SRDtlsID,
                x.SRID,
                x.UnitID,
                x.ApprovedQty,
                x.Remarks,
                x.ReqQty,
                StockQty = 0,
               CategoryName= x.INV_Category.Name,
               ProductName = x.INV_Product.Name,
               UnitName = x.INV_Unit.Name

            });
            return Json(storeDtlsList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadLoginBranchID()
        {
            DevourInvEntities db = new DevourInvEntities();

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchsID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));


            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            string userName = db.SEC_UserInformation.Where(x => x.ID == loginUserID).Select(x => x.UserFullName).ToList().FirstOrDefault();
            var userID = db.SEC_UserInformation.Where(x => x.ID == loginUserID).FirstOrDefault();
            var deptName = db.INV_Department.Where(x => x.DepartmentID == userID.DepartmentID).FirstOrDefault();
            var department = deptName.Name;
            return Json(new { branchsID, userName, department }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadStockQty(int BranchID,int CategoryID, int ProductID,int UnitID  )
        {
            DevourInvEntities db = new DevourInvEntities();
            //DateTime today = DateTime.Now.Date;
            SqlParameter p1 = new SqlParameter("@P1", BranchID);
            SqlParameter p2 = new SqlParameter("@P2", CategoryID);
            SqlParameter p3 = new SqlParameter("@P3", ProductID);
            SqlParameter p4 = new SqlParameter("@P4", UnitID);
            var trialList = db.Database.SqlQuery<VM_StockQty>("sp_StockCheck @p1,@P2,@P3,@P4", p1, p2, p3, p4).FirstOrDefault();
            return Json(trialList, JsonRequestBehavior.AllowGet);
        }

    }
    public class VM_StockQty
    {
        public decimal Quantity { get; set; }
        public int ProductID { get; set; }
    }
}