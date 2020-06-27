using BLL.Common;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Settings.Controllers
{
    public class TypeController : Controller
    {
        // GET: Settings/Type
        private ITypeFactory typeFactory;
        private Result result;
        public ActionResult TypeList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Type";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult TypeCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Type";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/TypeList";
        }
        [HttpPost]
        public JsonResult SaveType(INV_Type type)
        {
            result = new Result();
            typeFactory = new TypeFactorys();


            result = typeFactory.SaveType(type);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadType(int? typeID)
        {
            typeFactory = new TypeFactorys();
            List<INV_Type> list = typeFactory.SearchType(typeID);
            var typeList = list.Select(x => new
            {
                x.Name,
                x.Status,
                x.TypeID

            });
            return Json(typeList, JsonRequestBehavior.AllowGet);
        }
    }
}