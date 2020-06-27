using BLL.Factory.Security;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;
using DAL.db;
using IASystemWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Settings.Controllers
{
    public class BranchController : Controller
    {
        // GET: Settings/Branch
        private ICompanyBranchFactory companyBranchFactory;
        private Result result;
        public ActionResult StoreList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Branch";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult StoreCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Branch";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/BranchList";
        }
       
        [HttpPost]
        public JsonResult LoadBranch(int? branchID, string branchCode)
        {
            companyBranchFactory = new CompanyBranchFactorys();
            List<SET_CompanyBranch> list = companyBranchFactory.SearchCompanyBranch(branchID);
            var branchList = list.Select(x => new
            {
                x.BranchID,
                x.Name,
                x.Code,
                x.Address,
                x.Status
            });

            if (branchCode !=null)
            {
                branchList = branchList.Where(x => x.Code == branchCode);
            }

            return Json(branchList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchCompanyWiseBranch(int? companyID)
        {            
            companyBranchFactory = new CompanyBranchFactorys();
            List<SET_CompanyBranch> list = companyBranchFactory.SearchCompanyBranch(companyID);
            var companyList = list.Select(x => new { x.BranchID, x.Name });
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }

    }
}