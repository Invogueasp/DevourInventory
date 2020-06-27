using Application.Common;
using BLL.Common;
using BLL.Factory.Commercial;
using BLL.Interfaces.Commercial;
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
    public class PurchaseOrderController : Controller
    {
        // GET: Inventory/PurchaseOrder
        private IPurchaseOrderFactory sprFactory;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        private Result result;
        DateTime todayDate = DateTime.Now;
        public ActionResult PurchaseOrderList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Purchase Order";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult PendingPOrderCreate()
        {
            DefaultLoad();
            return View();
        }
        public ActionResult PurchaseOrderCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Purchase Order";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/PurchaseOrderList";
        }
        [HttpPost]
        public JsonResult SavePurchaseOrder(INV_PO pOrder, List<INV_PODtls> pOrderDtls, List<int> deletepOrderDtlsID,int ? check)
        {
            result = new Result();
            sprFactory = new POrderFactorys();
            DevourInvEntities db = new DevourInvEntities();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (pOrder.POID > 0)
            {
                pOrder.UpdatedBy = userID;
                pOrder.UpdatedDate = todayDate;
            }
            else
            {
               
                pOrder.CreatedBy = userID;
                pOrder.CreatedDate = todayDate;
            }
          
           
            result = sprFactory.SavePurchaseOrder(pOrder, pOrderDtls, deletepOrderDtlsID, check);
            if (result.isSucess == true)
            {
                var appData = db.INV_SPR.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
                appData.ThirdApproveStatus = "F";
             
                db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


            }
            result.lastInsertedID = pOrder.POID;
            return Json(result);
        }



        [HttpPost]
        public JsonResult SavePendingPurchaseOrder(INV_PO pOrder, List<INV_PODtls> pOrderDtls, List<int> deletepOrderDtlsID)
        {
            result = new Result();
            sprFactory = new POrderFactorys();
            DevourInvEntities db = new DevourInvEntities();
            string tableName = "Save Pending Purchase Order";

            var appData = db.INV_PO.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
              pOrder.POID = appData.POID;
           
            foreach (var dtls in pOrderDtls)
            {
                var appDataDt = db.INV_PODtls.Where(x => x.POID == pOrder.POID).FirstOrDefault();

                //dtls.PODtlsID = appDataDt.PODtlsID;
                appDataDt.POID = pOrder.POID;
                appDataDt.ApprovedQty = (dtls.ApprovedQty - dtls.OrderQty) + dtls.OrderQty;
                db.Entry(appDataDt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                result.message = result.UpdateSuccessfull(tableName);
            }
          
            result.lastInsertedID = pOrder.POID;
            return Json(result);
        }


        [HttpPost]
        public JsonResult LoadPurchaseOrder(int? pOID, VM_Parameters param)
        {
            DevourInvEntities db = new DevourInvEntities();
            var pOList = (from x in db.INV_PO
                join spr in db.INV_SPR on x.SPRID equals spr.SPRID
                join s in db.INV_Supplier on x.SupplierID equals s.SupplierID
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on spr.BranchID equals cb.BranchID
                select new
                {
                    x.POID,
                    x.PONO,
                    x.SPRID,
                    x.SupplierID,
                    x.PODate,
                    x.DueDate,
                    x.CreatedBy,
                    x.UpdatedBy,
                    x.UpdatedDate,
                    x.CreatedDate,
                    s.Name,
                    x.SecondApproveStatus,
                    x.FirstApproveStatus,
                    u.DepartmentID,
                    spr.BranchID,
                    Department = d.Name,
                    BranchName = cb.Name
                }).ToList();

            if (pOID > 0)
            {
                pOList = pOList.Where(x => x.POID == pOID).ToList();
            }

            if (param.FormDate != null && param.ToDate != null)
            {
                pOList = pOList.Where(x => x.PODate >= param.FormDate && x.PODate <= param.ToDate).ToList();
            }
            if (param.DepartmentID > 0)
            {
                pOList = pOList.Where(x => x.DepartmentID == param.DepartmentID).ToList();
            }
            if (param.BranchID > 0)
            {
                pOList = pOList.Where(x => x.BranchID == param.BranchID).ToList();
            }
            return Json(pOList.OrderByDescending(x => x.POID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadPurchaseOrderDtls(int? pOID)
        {
            //Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            //int empID = Convert.ToInt32(dictionary[1].Id);
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", pOID);
            var data = db.Database.SqlQuery<VM_ItemReturnDtlsWTS>("sp_ItemReturnDtlsForWTS @P1", p1).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        public JsonResult POReport(VM_PerameterFrPO parameters)
        {
            DevourInvEntities db = new DevourInvEntities();
            CommonFunctions commonFunctions = new CommonFunctions();

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";


            SqlParameter p1 = new SqlParameter("@P1", parameters.POID);
            var dataList = db.Database.SqlQuery<VM_PoReportDtls>("sp_PurchaseOdrDtlsListForReport @p1", p1).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "POReports.rdlc");
                reportDataSetName = "PoDatasets";

            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
            }

            List<ReportParameter> reportParameterss = new List<ReportParameter>();
            //ReportParameter parameterr = new ReportParameter("ToDate");
            //parameterr.Values.Add(todayDate.ToString());
            //reportParameterss.Add(parameterr);

            int? loginUserID = 0;
            fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
            return Json(fileString);

        }
    }
    public class VM_PerameterFrPO
    {
        public int POID { get; set; }
        public int BranchID { get; set; }
    }
    public class VM_PoReportDtls
    {
        public string SPRNO { get; set; }
        public DateTime SPRDate { get; set; }
        public string CategoryName { get; set; }
        public string PartNumber { get; set; }
        public string UnitName { get; set; }
        public string PONO { get; set; }
        public int ReqQty { get; set; }
        public int ApprovedQty { get; set; }
        public int OrderQty { get; set; }
        public int OrderAppQty { get; set; }
        public decimal UnitRate { get; set; }
        public decimal LineTotal { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string SPRRemarks { get; set; }
        public DateTime PODate { get; set; }
        public DateTime DueDate { get; set; }

    }
}