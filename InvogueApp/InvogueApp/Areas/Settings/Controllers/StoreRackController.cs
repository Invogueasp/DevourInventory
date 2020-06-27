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
    public class StoreRackController : Controller
    {
        // GET: Settings/StoreRack
        private IStoreRackFactory storeRackFactory;
        private Result result;
        public ActionResult StoreRackList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Store Rack";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult StoreRackCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Store Rack";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/StoreRackList";
        }
        [HttpPost]
        public JsonResult SaveStoreRack(INV_StoreRack storeRack)
        {
            result = new Result();
            storeRackFactory = new StoreRackFactorys();

            result = storeRackFactory.SaveStoreRack(storeRack);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadStoreRack(int? storeRackID)
        {
            storeRackFactory = new StoreRackFactorys();

            List<INV_StoreRack> list = storeRackFactory.SearchStoreRack(storeRackID);
            var storeRackList = list.Select(x => new
            {
                x.StoreRackID,
                x.BranchID,
                x.Name,
                StoreName= x.SET_CompanyBranch.Name,                
                x.Status
                
            });
            return Json(storeRackList, JsonRequestBehavior.AllowGet);
        }
    }
}