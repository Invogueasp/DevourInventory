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
    public class ModelController : Controller
    {
        // GET: Settings/Model
       
        private IModelFactory modelFactory;
        Result result;
        public ActionResult ModelList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Model";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult ModelCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Model";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/ModelList";
        }
        [HttpPost]
        public JsonResult LoadModel(int? id)
        {
            modelFactory = new ModelFactorys();
            List<INV_Model> list = modelFactory.SearchModel(id);
            var machineList = list.Select(x => new
            {
                x.ModelID,
                x.MachineID,
                MachineName = x.INV_Machine.Name,
                x.Name,
                x.Status
            });

            return Json(machineList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMachineWiseModel(int id)
        {
            modelFactory = new ModelFactorys();
            List<INV_Model> list = modelFactory.SearchMachineWiseModel(id);
            var machineList = list.Select(x => new
            {
                x.ModelID,
                x.Name
            });

            return Json(machineList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveModel(INV_Model model)
        {
            modelFactory = new ModelFactorys();
            result = new Result();
            result = modelFactory.SaveModel(model);
            return Json(result);
        }
    }
}