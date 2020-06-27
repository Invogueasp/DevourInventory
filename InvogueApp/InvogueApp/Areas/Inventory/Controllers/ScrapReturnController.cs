using Application.Common;
using BLL.Common;
using BLL.Factory.Inventory;
using BLL.Interfaces.Inventory;
using DAL.db;
using DAL.Helper;
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
    public class ScrapReturnController : Controller
    {
        // GET: Inventory/ScrapReturn
        private IScrapReturnFactory scrapReturnFactory;

        private Result result;

        // int userID = 1;
        CommonFunctions commonFunctions = new CommonFunctions();
        DateTime todayDate = DateTime.Now;
        public ActionResult ScrapReturnCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Scrap Return";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/ScrapReturnList";
        }
        public ActionResult ScrapReturnList()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Scrap Return";
            ViewBag.CallingViewPage = "#";
            return View();
        }


        [HttpPost]
        public JsonResult SaveScrapReturn(INV_ScrapReturn scrapReturn, List<INV_ScrapReturnDtls> scrapReturnDtls, List<int> deletescrapReturnDtls)
        {

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));

            result = new Result();
            scrapReturnFactory = new ScrapReturnFactorys();

            if (scrapReturn.ScrapReturnID > 0)
            {
                scrapReturn.ReturnBy = Convert.ToString(userID);
                scrapReturn.UpdatedBy = userID;
                scrapReturn.UpdatedDate = todayDate;
            }
            else
            {

                scrapReturn.CreatedBy = userID;
                scrapReturn.ReturnBy = Convert.ToString(userID);
                scrapReturn.CreatedDate = todayDate;
            }

            result = scrapReturnFactory.SaveScrapReturn(scrapReturn, scrapReturnDtls, deletescrapReturnDtls);
            result.lastInsertedID = scrapReturn.ScrapReturnID;
            return Json(result);
        }


        [HttpPost]
        public JsonResult LoadScrapReturn(int? id)
        {
            DevourInvEntities db = new DevourInvEntities();
            scrapReturnFactory = new ScrapReturnFactorys();
            List<INV_ScrapReturn> list = scrapReturnFactory.SearchScrapReturn(id);

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            string userName = db.SEC_UserInformation.Where(x => x.ID == loginUserID).Select(x => x.UserFullName).ToList().FirstOrDefault();
            var productList = list.Select(x => new
            {
                x.ScrapReturnID,
                x.ReturnNO,
                x.ReturnDate,
                x.Remarks,
                x.CreatedDate,
                x.CreatedBy,
                UserFullName = userName,

            });
            return Json(productList.OrderByDescending(x => x.ScrapReturnID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadScrapReturnDtls(int? id)
        {
            scrapReturnFactory = new ScrapReturnFactorys();
            List<INV_ScrapReturnDtls> list = scrapReturnFactory.SearchScrapReturnDtls(id);
            var storeDtlsList = list.Select(x => new
            {
                x.CategoryID,
                x.ProductID,
                x.UnitID,
                x.Quantity,
                x.Remarks,
                StockQty = 0,
                x.ScrapReturnDtlsID,
                x.ScrapReturnID,
                x.Location,
                CategoryName = x.INV_Category.Name,
                ProductName = x.INV_Product.Name,
                UnitName = x.INV_Unit.Name

            });
            return Json(storeDtlsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ScrapReturnView(VM_PerameterSR parameters)
        {
            DevourInvEntities db = new DevourInvEntities();

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            SqlParameter p1 = new SqlParameter("@P1", parameters.ScrapReturnID);
            var dataList = db.Database.SqlQuery<VM_ScrapDtls>("sp_ScrapReturn_Report @p1", p1).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "ScrapReturnMemo.rdlc");
                reportDataSetName = "scrapReturnDataSet";

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


    public class VM_PerameterSR
    {
        public int ScrapReturnID { get; set; }
    }

    public class VM_ScrapDtls
    {
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Location { get; set; }
        public decimal Quantity { get; set; }
        public string Remarks { get; set; }
    }
}