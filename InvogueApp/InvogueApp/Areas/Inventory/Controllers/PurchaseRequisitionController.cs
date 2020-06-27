using Application.Common;
using BLL.Common;
using BLL.Factory.Inventory;
using BLL.Interfaces.Inventory;
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
    public class PurchaseRequisitionController : Controller
    {
        // GET: Inventory/PurchaseRequisition
        private ISPRFactory sprFactory;
        DevourInvEntities db = new DevourInvEntities();
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        private Result result;
        DateTime todayDate = DateTime.Now;
        public ActionResult PurchaseRequisitionList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Store Purchase Requisition";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult PurchaseRequisitionCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Store Purchase Requisition";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/PurchaseRequisitionList";
        }
        [HttpPost]
        public JsonResult SaveSPR(INV_SPR storePR, List<INV_SPRDtls> storePRDtls, List<int> deleteSPRDtlsID)
        {
            result = new Result();
            sprFactory = new SPRFactorys();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (storePR.SPRID > 0)
            {

                storePR.UpdatedBy = userID;
                storePR.UpdatedDate = todayDate;
            }
            else
            {
                
                storePR.CreatedBy = userID;
                storePR.CreatedDate = todayDate;
            }

            result = sprFactory.SaveSPR(storePR, storePRDtls, deleteSPRDtlsID);
            result.lastInsertedID = storePR.SPRID;
            return Json(result);
        }
        [HttpPost]
        public JsonResult LoadSPR3rdApp(int? srID)
        {
            var srIDList = (from x in db.INV_SPR
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on x.BranchID equals cb.BranchID
                where x.ThirdApproveStatus == "2A"
                select new
                {
                    x.SPRID,
                    x.SPRNO,
                    x.SPRDate,
                    x.RequiredDate,
                    x.CreatedDate,
                    x.CreatedBy,

                    x.FirstApproveStatus,
                    x.FirstApproveBy,
                    x.FirstApproveDate,


                    x.SecondApproveStatus,
                    x.SecondApproveDate,
                    x.SecondApproveBy,

                    x.ThirdApproveStatus,
                    x.ThirdApproveDate,
                    x.ThirdApproveBy,
                    u.DepartmentID,

                    UserFullName = u.UserFullName,
                    x.BranchID,
                    Department = d.Name,
                    BranchName = cb.Name
                }).ToList();

            if (srID > 0)
            {
                srIDList = srIDList.Where(x => x.SPRID == srID).ToList();
            }
            return Json(srIDList.OrderByDescending(x => x.SPRID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadSPR2ndApp(int? srID)
        {
            var srIDList = (from x in db.INV_SPR
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on x.BranchID equals cb.BranchID
                select new
                {
                    x.SPRID,
                    x.SPRNO,
                    x.SPRDate,
                    x.RequiredDate,
                    x.CreatedDate,
                    x.CreatedBy,
                    x.FirstApproveStatus,
                    x.FirstApproveDate,
                    x.FirstApproveBy,
                    u.DepartmentID,

                    x.SecondApproveStatus,
                    x.SecondApproveDate,
                    x.SecondApproveBy,


                    x.ThirdApproveBy,
                    x.ThirdApproveDate,
                    x.ThirdApproveStatus,

                    UserFullName = u.UserFullName,
                    x.BranchID,
                    Department = d.Name,
                    BranchName = cb.Name
                }).ToList();

            if (srID > 0)
            {
                srIDList = srIDList.Where(x => x.SPRID == srID).ToList();
            }
            return Json(srIDList.OrderByDescending(x => x.SPRID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadSPR(int? srID, VM_Parameters param)
        {
            //int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            //int branchsID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));
            //string userName = db.SEC_UserInformation.Where(x => x.ID == loginUserID).Select(x => x.UserFullName).ToList().FirstOrDefault();
            //var userID = db.SEC_UserInformation.Where(x => x.ID == loginUserID).FirstOrDefault();
            //var deptName = db.INV_Department.Where(x => x.DepartmentID == userID.DepartmentID).FirstOrDefault();
            //var branchName = db.SET_CompanyBranch.Where(x => x.BranchID == branchsID).FirstOrDefault();

            //sprFactory = new SPRFactorys();
            //List<INV_SPR> list = sprFactory.SearchSPR(srID);
            //var srIDList = list.Select(x => new
            //{
            //  x.SPRID,
            //  x.SPRNO,
            //  x.SPRDate,
            //  x.RequiredDate,
            //  x.CreatedDate,
            //  x.CreatedBy,
            //  x.FirstApproveStatus,
            //  x.FirstApproveDate,
            //  x.FirstApproveBy,
            //  DepartmentID = x.SEC_UserInformation.DepartmentID,
              
            //  x.SecondApproveStatus,
            //  x.SecondApproveDate,
            //  x.SecondApproveBy,


            //  x.ThirdApproveBy,
            //  x.ThirdApproveDate,
            //  x.ThirdApproveStatus,

            //  UserFullName = userName,
            //  x.BranchID,
            //  Department = deptName.Name,
            //  BranchName = x.SET_CompanyBranch.Name
            //}).ToList();


            var srIDList = (from x in db.INV_SPR
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on x.BranchID equals cb.BranchID
                select new
                {
                    x.SPRID,
                    x.SPRNO,
                    x.SPRDate,
                    x.RequiredDate,
                    x.CreatedDate,
                    x.CreatedBy,
                    x.FirstApproveStatus,
                    x.FirstApproveDate,
                    x.FirstApproveBy,
                    u.DepartmentID,

                    x.SecondApproveStatus,
                    x.SecondApproveDate,
                    x.SecondApproveBy,


                    x.ThirdApproveBy,
                    x.ThirdApproveDate,
                    x.ThirdApproveStatus,

                    UserFullName = u.UserFullName,
                    x.BranchID,
                    Department = d.Name,
                    BranchName = cb.Name
                }).ToList();

            if (srID > 0)
            {
                srIDList = srIDList.Where(x => x.SPRID == srID).ToList();
            }

            if (param.FormDate != null && param.ToDate != null)
            {
                srIDList = srIDList.Where(x => x.SPRDate >= param.FormDate && x.SPRDate <= param.ToDate).ToList();
            }
            if (param.DepartmentID > 0)
            {
                srIDList = srIDList.Where(x => x.DepartmentID == param.DepartmentID).ToList();
            }
            if (param.BranchID > 0)
            {
                srIDList = srIDList.Where(x => x.BranchID == param.BranchID).ToList();
            }
            return Json(srIDList.OrderByDescending(x => x.SPRID), JsonRequestBehavior.AllowGet);
        }
        

          [HttpPost]
        public JsonResult LoadSprBy3rdApp(int? srID)
        {
            var srIDList = (from x in db.INV_SPR
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on x.BranchID equals cb.BranchID
                where x.ThirdApproveStatus == "3A" || x.ThirdApproveStatus == "F"
                select new
                {
                    x.SPRID,
                    x.SPRNO,
                    x.SPRDate,
                    x.RequiredDate,
                    x.CreatedDate,
                    x.CreatedBy,

                    x.FirstApproveStatus,
                    x.FirstApproveBy,
                    x.FirstApproveDate,


                    x.SecondApproveStatus,
                    x.SecondApproveDate,
                    x.SecondApproveBy,

                    x.ThirdApproveStatus,
                    x.ThirdApproveDate,
                    x.ThirdApproveBy,
                    u.DepartmentID,
                    
                    UserFullName = u.UserFullName,
                    x.BranchID,
                    Department = d.Name,
                    BranchName = cb.Name
                }).ToList();

            if (srID > 0)
            {
                srIDList = srIDList.Where(x => x.SPRID == srID).ToList();
            }
            return Json(srIDList.OrderByDescending(x => x.SPRID), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadSPRByDate(int? srID, DateTime fromDate, DateTime toDate)
        {
            sprFactory = new SPRFactorys();
            List<INV_SPR> list = sprFactory.SearchSPR(srID);
            var srIDList = list.Select(x => new
            {
                x.SPRID,
                x.SPRNO,
                x.SPRDate,
                x.RequiredDate,
                x.CreatedDate,
                x.CreatedBy,
                x.BranchID
            }).Where(x => x.RequiredDate >= fromDate && x.RequiredDate <= toDate);
            return Json(srIDList.OrderByDescending(x => x.SPRID), JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
        public JsonResult LoadSPRDtls(int? sPRID)
        {
            sprFactory = new SPRFactorys();
            List<INV_SPRDtls> list = sprFactory.SearchSprDtls(sPRID);
            var SPRDtlsList = list.Select(x => new
            {
                x.SPRID,
                x.SPRDtlsID,
                x.ReqQty,
                x.Remarks,
                x.UnitID,
                x.ProductID,
                x.CategoryID,
                CategoryName = x.INV_Category.Name,
                ProductName = x.INV_Product.Name,
                Code = x.INV_Product.Code,
                PartNumber = x.INV_Product.PartNumber,
                UnitName = x.INV_Unit.Name,
                x.ApprovedQty
            });
            return Json(SPRDtlsList.OrderByDescending(x => x.SPRDtlsID), JsonRequestBehavior.AllowGet);
        }


         [HttpPost]
         public JsonResult LoadSPRDtlss(int? sPRID)
         {
             Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
             //int empID = Convert.ToInt32(dictionary[1].Id);
             DevourInvEntities db = new DevourInvEntities();
             SqlParameter p1 = new SqlParameter("@P1", sPRID);
             var data = db.Database.SqlQuery<VM_PurchaseOdrList>("sp_PurchaseOdrList @P1", p1).ToList();
             return Json(data, JsonRequestBehavior.AllowGet);

         }

         [HttpPost]
         public JsonResult LoadSPRDtlsListFrPendingPo(int? sPRID)
         {
             Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
             //int empID = Convert.ToInt32(dictionary[1].Id);
             DevourInvEntities db = new DevourInvEntities();
             SqlParameter p1 = new SqlParameter("@P1", sPRID);
             var data = db.Database.SqlQuery<VM_PendingPurchaseOdrList>("sp_PendingPurchaseOdrList @P1", p1).ToList();
             return Json(data, JsonRequestBehavior.AllowGet);

         }

           [HttpPost]
         public JsonResult LoadSprFrPendingPO(int? sPRID)
         {
             Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
             DevourInvEntities db = new DevourInvEntities();
             //SqlParameter p1 = new SqlParameter("@P1", sPRID);
             var data = db.Database.SqlQuery<VM_PendingSprList>("sp_SprFRPendingPO").ToList();
             return Json(data, JsonRequestBehavior.AllowGet);

         }
        
         [HttpPost]
         public JsonResult LoadPODtlsList(int? pOID)
         {
             Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
             //int empID = Convert.ToInt32(dictionary[1].Id);
             DevourInvEntities db = new DevourInvEntities();
             SqlParameter p1 = new SqlParameter("@P1", pOID);
             var data = db.Database.SqlQuery<VM_PurchaseOdrList>("sp_PurchaseOdrDtlsList @P1", p1).ToList();
             return Json(data, JsonRequestBehavior.AllowGet);

         }

         public JsonResult IndentPrReport(VM_PerameterFrSPR parameters)
          {
             DevourInvEntities db = new DevourInvEntities();
             CommonFunctions commonFunctions = new CommonFunctions();

             Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
             int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

             string path = string.Empty;
             string reportDataSetName = string.Empty;
             string fileString = string.Empty;
             string docType = "pdf";


             SqlParameter p1 = new SqlParameter("@P1", parameters.SPRID);
             var dataList = db.Database.SqlQuery<VM_IndentPrReportDtls>("sp_IndentPR_Report @p1", p1).ToList();


             if (dataList.Count > 0)
             {
                 path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "IndentPrReport.rdlc");
                 reportDataSetName = "IndentPRDataSet2";

             }
             else
             {
                 path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
             }

             List<ReportParameter> reportParameterss = new List<ReportParameter>();
             ReportParameter parameterr = new ReportParameter("ToDate");
             parameterr.Values.Add(todayDate.ToString());
             reportParameterss.Add(parameterr);


             int? loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
             fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
             return Json(fileString);

         }
    }
    public class VM_PerameterFrSPR
    {
        public int SPRID { get; set; }
        public int BranchID { get; set; }
    }
    public class VM_IndentPrReportDtls
    {
       
        public int ApprovedQty { get; set; }
        public int ReceiveQty { get; set; }
        public string PONO { get; set; }
        public string DepartmentName { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string UnitName { get; set; }
        public Decimal Inhand { get; set; }
        public int OnOrder { get; set; }
        public int InTransit { get; set; }
        public int OrderQty { get; set; }
        public string Remarks { get; set; }
       
    }
}