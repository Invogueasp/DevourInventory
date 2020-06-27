using BLL.Common;
using BLL.Factory;
using BLL.Factory.Security;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class CommonController : Controller
    {
        Result result;
        ISQLFactory sqlFactory;
        ICommonFactory commonFactory;

        public JsonResult GenerateCode(string tableName, string fieldName, string prefix)
        {
            result = new Result();
            sqlFactory = new SQLFactory();
            result.message = sqlFactory.ExecuteSP_GnCode(DateTime.Now.Date, tableName, fieldName, prefix);
            return Json(result);
        }
        public JsonResult LoadCountry()
        {
            commonFactory = new CountryFactorys();
            List<SET_Country> list = commonFactory.SearchCountry(0);
            var countryList = list.Select(x => new
            {
                x.CountryID,
                x.Name
            });

            return Json(countryList);

        }

        public ActionResult SelectPermission(string moduleName, string pageName)
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "Dashboard");
                if (tblUserActionMapping.Select)
                {
                    ViewBag.CallingForm = moduleName;
                    ViewBag.CallingForm1 = pageName;
                    ViewBag.CallingViewPage = "#";
                    return View();
                }
            }
            Session["logInSession"] = "false";
            return Redirect("/#!/");
        }
    }
}