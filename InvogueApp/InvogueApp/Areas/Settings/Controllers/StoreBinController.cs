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
    public class StoreBinController : Controller
    {
        // GET: Settings/StoreBin
        private IStoreBinFactory storeBinFactory;
        private Result result;
        public ActionResult StoreBinList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Store Bin";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult StoreBinCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Store Bin";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/StoreBinList";
        }
        [HttpPost]
        public JsonResult SaveStoreBin(INV_StoreBin storeBin)
        {
            result = new Result();
            storeBinFactory = new StoreBinFactorys();

            result = storeBinFactory.SaveStoreBin(storeBin);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadStoreBin(int? storeBinID)
        {
            storeBinFactory = new StoreBinFactorys();

            List<INV_StoreBin> list = storeBinFactory.SearchStoreBin(storeBinID);
            var storeRackList = list.Select(x => new
            {
                x.StoreBinID,
                x.BranchID,
                x.StoreRackID,
                RackName = x.INV_StoreRack.Name,
                x.SET_CompanyBranch.Name,
                x.BinNO,
                x.Status

            });
            return Json(storeRackList, JsonRequestBehavior.AllowGet);
        }
    }
}