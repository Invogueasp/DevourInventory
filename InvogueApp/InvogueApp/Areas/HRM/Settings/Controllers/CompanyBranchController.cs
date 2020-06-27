using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Common;
using BLL.Factory.Security;
using BLL.Factory.Settings;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Interfaces.Settings;
using BLL.Models;
using DAL.db;
using DAL.Helper;

namespace Application.Areas.HRM.Settings.Controllers
{
    public class CompanyBranchController : Controller
    {
        // GET: HRM/CompanyBranch
        private ICompanyBranchFactory companyBranchFactory;
        private Result result;
        private DevourInvEntities db;
        public ActionResult CompanyBranchList()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "CompanyBranch");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = "HR";
                    ViewBag.CallingForm1 = "Company Branch";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }

        [HttpPost]
        public JsonResult LoadCompanyBranch(int? id)
        {
            companyBranchFactory = new CompanyBranchFactorys();
            List<SET_CompanyBranch> list = companyBranchFactory.SearchCompanyBranch(id);
            var branchList = list.Select(x => new
            {
                x.BranchID, x.CompanyID, x.Name, CompanyName = x.SET_Company.Name, x.ContactNO, x.Code, x.Address, x.Status,
                x.IsHeadOffice, x.Description, x.Email, x.CreatedBy, x.CreatedDate
            });
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateCompanyBranch()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "CompanyBranch");
                if (tblUserActionMapping.Create)
                {
                    DefaultLoad();
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }
        
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "HR";
            ViewBag.CallingForm1 = "Company Branch";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/CompanyBranchList";
        }
    }
}