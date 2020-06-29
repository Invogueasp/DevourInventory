using Application.Common;
using BLL.Common;
using BLL.Factory.Commercial;
using BLL.Interfaces.Commercial;
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
    public class MRRController : Controller
    {
        // GET: Inventory/MRR
        private IPurchaseOrderFactory sprFactory;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        DevourInvEntities db = new DevourInvEntities();
        private IMRRFactory mrrFactory;
        private Result result;
        DateTime todayDate = DateTime.Now;
        public ActionResult MRRList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Material Receive Report";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult MRRCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Material Receive Report";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/MRRList";
        }
        [HttpPost]
        public JsonResult LoadPurchaseOrder(int? pOID)
        {
            sprFactory = new POrderFactorys();
            //Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            //int empID = Convert.ToInt32(dictionary[1].Id);
            DevourInvEntities db = new DevourInvEntities();
            var data = db.Database.SqlQuery<VM_PurchaseOrderAppList>("sp_PurchaseOrderAppList").ToList();
            var appDataList = data.Where(x => x.SecondApproveStatus == "2A").Select(x => new { x.PONO, x.POID, x.FirstApproveStatus, x.SupplierID, x.SupplierName, x.TotalAmount }).ToList();
            return Json(appDataList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult LoadPurchaseOrderDtls(int? pOID)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", pOID);
            var mrRDtlsList = db.Database.SqlQuery<VM_MrrDtlsList>("sp_MrrDtlsList @p1", p1).ToList();
            return Json(mrRDtlsList.OrderByDescending(x => x.MRRID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadMrrDtls(int? mrrID)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", mrrID);
            var dataList = db.Database.SqlQuery<VM_MrrDtlsList>("sp_MrrDtlsList_ForEdit @p1", p1).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveMRR(INV_MRR mRr, List<VM_TempMrrDtlsList> mRrDtls)
        {
            result = new Result();
            mrrFactory = new MRRFactorys();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            var data = db.INV_MRR.Where(x => x.MRRNO == mRr.MRRNO).Count();
            if (data > 0)
            {
                result.isSucess = false;
                result.message = "All Ready Exsit!!";
            }
            else
            {
                if (mRr.MRRID > 0)
                {
                    mRr.UpdatedBy = userID;
                    mRr.UpdatedDate = todayDate;
                }
                else
                {
                    mRr.Status = "A";
                    mRr.CreatedBy = userID;
                    mRr.CreatedDate = todayDate;
                }

                result = mrrFactory.SaveMRR(mRr, mRrDtls);
                result.lastInsertedID = mRr.MRRID;
            }
            
            return Json(result);
        }
        [HttpPost]
        public JsonResult LoadMRR(int? mRRID, VM_Parameters param)
        {

            var mRRList = (from x in db.INV_MRR
                join spr in db.INV_SPR on x.SPRID equals spr.SPRID
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join cb in db.SET_CompanyBranch on spr.BranchID equals cb.BranchID
                select new
                {
                    x.POID,
                    x.SPRID,
                    x.MRRID,
                    x.Status,
                    x.SupplierID,
                    x.SupplierInv,
                    x.MRRNO,
                    spr.SPRNO,
                    x.CreatedDate,
                    x.MRRDate,
                    x.CreatedBy,
                    x.UpdatedBy,
                    x.UpdatedDate,
                    x.QCID,
                    u.DepartmentID,
                    spr.BranchID,
                    BranchName = cb.Name
                }).ToList();

            if (mRRID > 0)
            {
                mRRList = mRRList.Where(x => x.MRRID == mRRID).ToList();
            }
            if (param.FormDate != null && param.ToDate != null)
            {
                mRRList = mRRList.Where(x => x.MRRDate >= param.FormDate && x.MRRDate <= param.ToDate).ToList();
            }
            if (param.BranchID > 0)
            {
                mRRList = mRRList.Where(x => x.BranchID == param.BranchID).ToList();
            }
            return Json(mRRList.OrderByDescending(x => x.MRRID), JsonRequestBehavior.AllowGet);
        }
        //======================================== Report ==============================
        CommonFunctions commonFunctions = new CommonFunctions();
        DateTime ToDate = DateTime.Now;
        public JsonResult MaterialReceiveReport(VM_ReturnPerameters parameters)
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            DevourInvEntities db = new DevourInvEntities();

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";


            SqlParameter p1 = new SqlParameter("@P1", parameters.MRRID);
            var dataList = db.Database.SqlQuery<VM_MrrDtlsLists>("sp_MrrDtls_Report @p1", p1).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "MrrReport.rdlc");
                reportDataSetName = "MRRDataDtlsSet";

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
    public class VM_ReturnPerameters
    {
        public int MRRID { get; set; }
       
    }
    public class VM_MrrDtlsLists
    {

        public int Quantity { get; set; }
        public string PONO { get; set; }
        public string Code { get; set; }
        public string SPRNO { get; set; }
        public string SupplierInv { get; set; }
        public string Address { get; set; }
        public string SupplierName { get; set; }
        public DateTime MRRDate { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public Decimal UnitRate { get; set; }
        public Decimal LineTotal { get; set; }                           
    }
}