using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Common;
using BLL.Factory.Security;
using BLL.Factory.Settings;
using BLL.Interfaces.Security;
using BLL.Interfaces.Settings;
using BLL.Models;
using DAL.db;
using DAL.Helper;

namespace InvogueApp.Areas.Settings.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Settings/Company
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        ICompanyFactory companyFactory;
        Result result;

        public ActionResult CreateCompany()
        {

            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "Company");
                if (tblUserActionMapping.Create)
                {
                    ViewBag.CallingForm = "HRM";
                    ViewBag.CallingForm1 = "Company";
                    ViewBag.CallingForm2 = "Add New";
                    ViewBag.CallingViewPage = "#!/CompanyList";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }

        public ActionResult CompanyList()
        {

            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "Company");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = "HRM";
                    ViewBag.CallingForm1 = "Company";
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = null;
            return Redirect("/#!/");
        }

        [HttpPost]
        public JsonResult SaveCompany(SET_Company company)
        {
            companyFactory = new SettingsFactorys();
            result = companyFactory.SaveCompany(company);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadCompany(int? companyID)
        {
            companyFactory = new SettingsFactorys();
            List<SET_Company> list = companyFactory.SearchCompany(companyID);
            var companyList = list.Select(x => new { x.CompanyID, x.ContactNo, x.Name, x.RegistrationNo, x.Code, x.EmailID, x.Address, x.TaxNo, x.Status });
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }
    }
}