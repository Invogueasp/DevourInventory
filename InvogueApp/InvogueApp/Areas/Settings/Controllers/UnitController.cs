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
    public class UnitController : Controller
    {
        // GET: Settings/Unit
        private IUnitFactory unitFactory;
        private Result result;
        public ActionResult UnitList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Unit";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult UnitCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Unit";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/UnitList";
        }
        [HttpPost]
        public JsonResult SaveUnit(INV_Unit unit)
        {
            result = new Result();
            unitFactory = new UnitFactorys();


            result = unitFactory.SaveUnit(unit);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadUnit(int? unitID)
        {
            unitFactory = new UnitFactorys();
            List<INV_Unit> list = unitFactory.SearchUnit(unitID);
            var unitList = list.Select(x => new
            {
              x.Name,
              x.ShortName,
              x.UnitID,
              x.Status

            });
            return Json(unitList, JsonRequestBehavior.AllowGet);
        }
    }
}