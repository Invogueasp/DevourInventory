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
    public class SubCategoryController : Controller
    {
        // GET: Settings/SubCategory
        private ISubCategoryFactory subCategoryFactory;
        private Result result;
        public ActionResult SubCategoryList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Sub Category";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult SubCategoryCreate()
        {
            DefaultLoad();
            return View();
        }
        [HttpPost]
        public JsonResult SaveSubCategory(INV_SubCategory subCategory)
        {
            result = new Result();
            subCategoryFactory = new SubCategoryFactorys();


            result = subCategoryFactory.SaveSubCategory(subCategory);
            return Json(result);
        }


        [HttpPost]
        public JsonResult IdWiseSubCategory(int? categoryID)
        {
            subCategoryFactory = new SubCategoryFactorys();
            List<INV_SubCategory> list = subCategoryFactory.IdWiseSubCategory(categoryID);
            var subCategoryList = list.Select(x => new
            {
                x.CategoryID,
                x.Code,
                x.Name,
                CategoryName = x.INV_Category.Name,
                x.Status,
                x.SubCategoryID

            });
            return Json(subCategoryList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadSubCategory(int? subCategoryID)
        {
            subCategoryFactory = new SubCategoryFactorys();
            List<INV_SubCategory> list = subCategoryFactory.SearchSubCategory(subCategoryID);
            var subCategoryList = list.Select(x => new
            {
                x.CategoryID,
                x.Code,
                x.Name,
                CategoryName = x.INV_Category.Name,
                x.Status,
                x.SubCategoryID

            });
            return Json(subCategoryList, JsonRequestBehavior.AllowGet);
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Sub Category";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/SubCategoryList";
        }
    }
}