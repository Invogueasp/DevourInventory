using Application.Common;
using BLL.Common;
using BLL.Factory.Inventory;
using BLL.Interfaces.Inventory;
using BLL.Models;
using DAL.db;
using DAL.Helper;
using InvogueApp.Areas.Inventory.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class ItemReturnController : Controller
    {
        // GET: Inventory/ItemReturn
        private ITransferFactory transferFactory;

        private IItemReturnFactory itemReturnFactory;

        private Result result;
        int userID = 1;
        DateTime todayDate = DateTime.Now;
        public ActionResult ItemReturnList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Item Return";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult ItemReturnCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Item Return";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/ItemReturnList";
        }

        [HttpPost]
        public JsonResult SaveIetmReturn(INV_Return ietmReturn, List<VM_TempItemReturnDtls> ietmReturnDtls)
        {
            result = new Result();
            itemReturnFactory = new ItemReturnFactorys();

            ietmReturn.Status = "P";
            result = itemReturnFactory.SaveIetmReturn(ietmReturn, ietmReturnDtls);            
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadIssueID(int? srID)
        {
            itemReturnFactory = new ItemReturnFactorys();
            List<INV_Issue> list = itemReturnFactory.SearchIssue(srID);
            var storeReqList = list.Select(x => new
            {
                x.SRID,
                x.IssueID,
                x.Status
            });
            return Json(storeReqList.OrderByDescending(x => x.SRID), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadIssueDtls(int? issueID)
        {
            //itemReturnFactory = new ItemReturnFactorys();
            //List<INV_IssueDtls> list = itemReturnFactory.SearchIssueDtls(issueID);
            //var issueDtlsList = list.Select(x => new
            //{
            //    x.CategoryID,
            //    x.ProductID,
            //    x.IssueQty,
            //    x.UnitID,
            //    x.IssueDtlsID,
            //    CategoryName = x.INV_Category.Name,
            //    ProductName = x.INV_Product.Name,
            //    UnitName = x.INV_Unit.Name
            //});
            //return Json(issueDtlsList, JsonRequestBehavior.AllowGet);
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int empID = Convert.ToInt32(dictionary[1].Id);
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", issueID);
            var data = db.Database.SqlQuery<VM_ItemReturnDtlsForSTW>("sp_ItemReturnDtlsForSTW @P1", p1).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult LoadTransferByStoreID(int? fromStoreID , int? toStoreID)
        //{
        //    DevourInvEntities db = new DevourInvEntities();
        //    transferFactory = new TransferFactorys();

        //    var transferList = from t in db.INV_Transfer
        //                       join s in db.SET_CompanyBranch on t. equals s.StoreID
        //                       join s2 in db.INV_Store on t.FromStoreID equals s2.StoreID

        //                       where t.FromStoreID == fromStoreID && t.ToStoreID == toStoreID
        //                       select new
        //                       {
        //                           t.TransferDate,
        //                           t.TransferID,
        //                           t.TransferNO,
        //                           t.Status,
        //                           t.CreatedDate,
        //                           t.CreatedBy,
        //                           t.ToStoreID,
        //                           t.FromStoreID,
        //                           ToStore = s.Name,
        //                           FromStore = s2.Name


        //                       };

        //    return Json(transferList.OrderByDescending(x => x.TransferID).OrderByDescending(x => x.TransferID), JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult LoadTransferDtls(int? transferID)
        {
            transferFactory = new TransferFactorys();
            List<INV_TransferDtls> list = transferFactory.SearchTransferDtls(transferID);
            var transferDtlsList = list.Select(x => new
            {
                x.CategoryID,
                x.ProductID,
                x.TransferID,
                x.ApprovalRemarks,
                x.TransferDtlsID,
                x.UnitID,
                x.Remarks,
                x.TransferQty,
                CategoryName = x.INV_Category.Name,
                ProductName = x.INV_Product.Name,
                UnitName = x.INV_Unit.Name,


            }).Where(x => x.TransferID == transferID);

            return Json(transferDtlsList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadRtnTransferDtls(int? transferID)
        {

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int empID = Convert.ToInt32(dictionary[1].Id);
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", transferID);
            var data = db.Database.SqlQuery<VM_ItemReturnDtls>("sp_ItemReturnDtls @P1", p1).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadReturn(int? returnID)
        {
            itemReturnFactory = new ItemReturnFactorys();
            List<INV_Return> list = itemReturnFactory.SearchReturn(returnID);
            var returnList = list.Select(x => new
            {
                x.ReturnID,
                x.ReturnNO,
                x.ReturnType,
                x.ReturnTypeID,
                x.SRID,
                x.Status,
                x.ToStoreID,
                x.ReturnDate,
                x.ToSupplierID,
                x.ToWarehouseID,
                x.TransferID,
                x.FromStoreID,
                x.FromWarehouseID,
                x.PurchaseOrderID
            });
            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadReturnDtls(int? returnID)
        {
            itemReturnFactory = new ItemReturnFactorys();
            DevourInvEntities db = new DevourInvEntities();

            var returnList = from x in db.INV_ReturnDtls
                                  join r in db.INV_Return on x.ReturnID equals r.ReturnID
                                  join p in db.INV_Product on x.ProductID equals p.ProductID
                                  join c in db.INV_Category on x.CategoryID equals c.CategoryID
                                  join u in db.INV_Unit on x.UnitID equals u.UnitID
                                  

                                  where x.ReturnID == returnID 
                                  select new
                                  {
                                      x.CategoryID,
                                      x.ProductID,
                                      x.ReturnDtlsID,
                                      x.ApprovalRemarks,
                                      x.ReturnID,
                                      x.UnitID,
                                      x.Remarks,
                                      x.ReturnQty,
                                      x.ProductType,
                                      r.FromWarehouseID,
                                      CategoryName = c.Name,
                                      ProductName = p.Name,
                                      UnitName = u.Name
                                   
                                  };

            //List<INV_ReturnDtls> list = itemReturnFactory.SearchReturnDtls(returnID);
            //var returnList = list.Select(x => new
            //{
            //    x.CategoryID,
            //    x.ProductID,
            //    x.ReturnDtlsID,
            //    x.ApprovalRemarks,
            //    x.ReturnID,
            //    x.UnitID,
            //    x.Remarks,
            //    x.ReturnQty,
            //    x.ProductType,
            //    x.INV_Return.FromWarehouseID,
            //    CategoryName = x.INV_Category.Name,
            //    ProductName = x.INV_Product.Name,
            //    UnitName = x.INV_Unit.Name
            //}).Where(x => x.ReturnID == returnID);
            return Json(returnList, JsonRequestBehavior.AllowGet);
        }
        //======================================== Report ==============================
        CommonFunctions commonFunctions = new CommonFunctions();
        DateTime ToDate = DateTime.Now;
        public JsonResult ReturnMemoReport(VM_Perameters parameters)
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            DevourInvEntities db = new DevourInvEntities();

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";


            SqlParameter p1 = new SqlParameter("@P1", parameters.ReturnID);
            var dataList = db.Database.SqlQuery<VM_ReturnDtlsLists>("sp_StoreReturnMemoDtls @p1", p1).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "StoreReturnMemo.rdlc");
                reportDataSetName = "SReturnMemoDataset";

            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
            }

            List<ReportParameter> reportParameterss = new List<ReportParameter>();


            int? loginUserID = 0;
            fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
            return Json(fileString);

        }
    }
    public class VM_Perameters
    {
        public int ReturnID { get; set; }

    }
    public class VM_ReturnDtlsLists
    {

        public int ReturnQty { get; set; }
        public string Remarks { get; set; }
        public string Code { get; set; }
        public string ReturnNO { get; set; }
        //public DateTime MRRDate { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
    }
}