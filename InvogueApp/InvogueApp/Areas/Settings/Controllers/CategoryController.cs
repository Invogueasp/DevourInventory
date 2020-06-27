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
    public class CategoryController : Controller
    {
        private ICategoryFactory categoryFactory;
        private Result result;
        // GET: Settings/Category
        public ActionResult CategoryList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Category";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult CategoryCreate()
        {
            DefaultLoad();
            return View();
        }
        [HttpPost]
        public JsonResult SaveCategory(INV_Category category)
        {
            result = new Result();
            categoryFactory = new CategoryFactorys();


            result = categoryFactory.SaveCategory(category);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadCategory(int? categoryID)
        {
            categoryFactory = new CategoryFactorys();
            List<INV_Category> list = categoryFactory.SearchCategory(categoryID);
            var categoryList = list.Select(x => new
            {
               x.CategoryID ,
                x.Code,
                Name = x.Name + " " + x.Code,
                x.Status
               
            });
            return Json(categoryList, JsonRequestBehavior.AllowGet);
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Category";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/CategoryList";
        }
    }
}