using Application.Common;
using DAL.db;
using DAL.Helper;
using InvogueApp.Areas.Inventory.Models;
using InvogueApp.Areas.Reports.Models;
using InvogueApp.Areas.Settings.Controllers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Reports.Controllers
{
    public class TechnoReportsController : Controller
    {
        // GET: Reports/Reports
         CommonFunctions commonFunctions = new CommonFunctions();
             DateTime ToDate = DateTime.Now;
        //===============================================================  Inventory Reports Controller ========================================================

        public ActionResult SRandIssueReport()
             {
                 ViewBag.CallingForm = "Inventory";
                 ViewBag.CallingForm1 = "Store Requisition Report";
                 ViewBag.CallingViewPage = "#";

            return View();
        }
        [HttpPost]
        public JsonResult LoadSRNOForDropdown(string name)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", name);
            var trialList = db.Database.SqlQuery<VM_SearchSRNO>("sp_SRNOSearch @p1", p1).ToList();
            return Json(trialList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SRandIssueReportView(VM_Perameter parameters)
        {
            DevourInvEntities db = new DevourInvEntities();
 
            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";
           
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            
            int ? loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));



            SqlParameter p1 = new SqlParameter("@P1", parameters.SRID);
            var dataList = db.Database.SqlQuery<VM_SRandIssueDtls>("sp_SRandIssueNoteReportList @p1", p1).ToList();

          
            if(dataList.Count> 0){
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "StoreRequisitation.rdlc");
                reportDataSetName = "SRandIssueDataSet";

            }
              else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
            }
       
                List<ReportParameter> reportParameterss = new List<ReportParameter>();

                fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
                return Json(fileString);
            
        }
    }


    public class VM_Perameter
    {
        public int SRID { get; set; }
    }




    
}