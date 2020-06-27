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
    public class DepartmentController : Controller
    {
        // GET: Settings/Size
        private IDepartmentFactory deptFactory;
        private Result result;
        public ActionResult DepartmentList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Department";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult DepartmentCreate()
        {
            DefaultLoad();
            return View();
        }

        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Department";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/DepartmentList";
        }
        [HttpPost]
        public JsonResult SaveDepartment(INV_Department dept)
        {
            result = new Result();
            deptFactory = new DepartmentFactorys();


            result = deptFactory.SaveDepartment(dept);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadDepartment(int? deptID)
        {
            deptFactory = new DepartmentFactorys();
            List<INV_Department> list = deptFactory.SearchDepartment(deptID);
            var departmentList = list.Select(x => new
            {
                x.DepartmentID,
                x.Code,
                x.Name,
                x.Description,
                x.Status

            });
            return Json(departmentList, JsonRequestBehavior.AllowGet);
        }

    }
}