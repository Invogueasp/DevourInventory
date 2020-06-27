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
    public class MachineController : Controller
    {
        private IMachineFactory machineFactory;
        private Result result;
        public ActionResult MachineList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Machine";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult MachineCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Machine";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/MachineList";
        }
        [HttpPost]
        public JsonResult LoadMachine(int? id)
        {
            machineFactory = new MachineFactorys();
            List<INV_Machine> list = machineFactory.SearchMachine(id);
            var machineList = list.Select(x => new
            {
                x.MachineID,
                x.Name,
                x.Code,
                x.Status
            });

            return Json(machineList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveMachine(INV_Machine machine)
        {
            result = new Result();
            machineFactory = new MachineFactorys();
            result = machineFactory.SaveMachine(machine);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}