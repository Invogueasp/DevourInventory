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
    public class QCController : Controller
    {
        // GET: Inventory/QC
     
        private IQCFactory mrrFactory;
        private Result result;
        DateTime todayDate = DateTime.Now;
        public ActionResult QCList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Quality Certificats";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult QCCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Quality Certificats";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/QCList";
        }
        [HttpPost]
        public JsonResult SaveQC(INV_QC mRr, List<INV_QCDtls> mRrDtls, List<int> deletepDtlsID)
        {
            DevourInvEntities db = new DevourInvEntities();
           result = new Result();
            mrrFactory = new QCFactorys();
            result = mrrFactory.SaveQC(mRr, mRrDtls, deletepDtlsID);
            result.lastInsertedID = mRr.QCID;

            if (result.isSucess == true)
            {
                var appData = db.INV_MaterialReceive.Where(x => x.MaterialReceiveID == mRr.MaterialReceiveID).FirstOrDefault();
                appData.Status = "F";
                db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(result);
        }
        [HttpPost]
        public JsonResult LoadQC(int? qcID, VM_Parameters param)
        {
            DevourInvEntities  db = new DevourInvEntities();
            //mrrFactory = new QCFactorys();
            //List<INV_QC> list = mrrFactory.SearchQC(qcID);
            var mRRList = (from qc in db.INV_QC
                          join mrr in db.INV_MaterialReceive on qc.MaterialReceiveID equals mrr.MaterialReceiveID
                          join po in db.INV_PO on mrr.POID equals po.POID
                          join spr in db.INV_SPR on po.SPRID equals spr.SPRID
                          join u in db.SEC_UserInformation on mrr.CreatedBy equals u.ID
                          join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                          join cb in db.SET_CompanyBranch on spr.BranchID equals cb.BranchID
                          select new
                          {
                              qc.MaterialReceiveID,
                              qc.QCDate,
                              qc.QCID,
                              qc.QCNO,
                              mrr.POID,
                              spr.BranchID,
                              u.DepartmentID,
                              Department = d.Name,
                              BranchName = cb.Name

                          }).ToList();


            if (qcID > 0)
            {
                mRRList = mRRList.Where(x => x.QCID == qcID).ToList();
            }
            if (param.FormDate != null && param.ToDate != null)
            {
                mRRList = mRRList.Where(x => x.QCDate >= param.FormDate && x.QCDate <= param.ToDate).ToList();
            }
            if (param.DepartmentID > 0)
            {
                mRRList = mRRList.Where(x => x.DepartmentID == param.DepartmentID).ToList();
            }
            if (param.BranchID > 0)
            {
                mRRList = mRRList.Where(x => x.BranchID == param.BranchID).ToList();
            }
            return Json(mRRList.OrderByDescending(x => x.QCID), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult LoadQCDtls(int? qcID)
        //{
        //    DevourInvEntities db = new DevourInvEntities();
        //    SqlParameter p1 = new SqlParameter("@P1", qcID);
        //    var mrRDtlsList = db.Database.SqlQuery<VM_MRSDtlsList>("sp_MRRSDtlsList @p1", p1).ToList();
        //    return Json(mrRDtlsList.OrderByDescending(x => x.MaterialReceiveID), JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult LoadMRSDtls(int? qcid)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", qcid);
            var mrRDtlsList = db.Database.SqlQuery<VM_QCDtlsList>("sp_MRS_For_QCDtlsList @p1", p1).ToList();
            return Json(mrRDtlsList.OrderByDescending(x => x.MaterialReceiveID), JsonRequestBehavior.AllowGet);
        }
        //============================================================= Report =============================================================
        CommonFunctions commonFunctions = new CommonFunctions();

        public JsonResult QCReportView(VM_Perameters parameters)
        {
            DevourInvEntities db = new DevourInvEntities();

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";
            

            SqlParameter p1 = new SqlParameter("@P1", parameters.QCID);
            var dataList = db.Database.SqlQuery<VM_QCDtls>("sp_QCReport_DtlsList @p1", p1).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "QCReport.rdlc");
                reportDataSetName = "QCDataSet";

            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
            }

            List<ReportParameter> reportParameterss = new List<ReportParameter>();

            int? loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
            return Json(fileString);

        }

        public class VM_QCDtls
        {

            public DateTime QCDate { get; set; }
            public string SPRNO { get; set; }
            public string ProductName { get; set; }
            public string UnitName { get; set; }
            public string QCNO { get; set; }
            public decimal QCPassQty { get; set; }

        }
        public class VM_Perameters
        {
            public int QCID { get; set; }
        }


    }
}