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
    public class SupplierController : Controller
    {
        // GET: Settings/Supplier
        private ISupplierFactory suppilerFactory;

        private Result result;
        public ActionResult SupplierList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Supplier";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult SupplierCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Supplier";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/SupplierList";
        }
        [HttpPost]
        public JsonResult SaveSupplier(INV_Supplier supplier, List<INV_SupplierProduct> supplierProduct, List<int> deleteSupplierProductRowID)
        {
            result = new Result();
            suppilerFactory = new SupplierFactorys();


            result = suppilerFactory.SaveSupplier(supplier, supplierProduct, deleteSupplierProductRowID);
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadSupplier(int? supplierID)
        {
            suppilerFactory = new SupplierFactorys();
            List<INV_Supplier> list = suppilerFactory.SearchSupplier(supplierID);
            var supplierList = list.Select(x => new
            {
                x.SupplierID,
                x.SupplierCode,
               SupplierName = x.Name,
                x.CurrencyID,
                x.SET_Currency.Name,
               x.CountryID,
               x.CreditLimit,
               x.BankAcNO,
               x.Address,
               x.BankName,
               x.Mobile,
               x.Note,
               x.PaymentTerms,
               x.Phone,
               x.Website,
               x.ContactPerson,
               x.Email
               

            });
            return Json(supplierList.OrderByDescending(x => x.SupplierID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadSupplierProduct(int? supplierID)
        {
            suppilerFactory = new SupplierFactorys();
            List<INV_SupplierProduct> list = suppilerFactory.SearchSupplierProduct(supplierID);
            var supplierList = list.Select(x => new
            {
               x.SupplierProductID,
               x.SupplierID,
               x.ProductID,
              SupplierName = x.INV_Supplier.Name,
              ProductName = x.INV_Product.Name,
              CategoryName = x.INV_Category.Name,
              x.CategoryID
            });
            return Json(supplierList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadCurrency(int? currencyID)
        {
            suppilerFactory = new SupplierFactorys();
            List<SET_Currency> list = suppilerFactory.SearchCurrency(currencyID);
            var currencyList = list.Select(x => new
            {
               x.CurrencyID,
               x.Name,
               x.ShortName,
               x.CurrencySymbol
            });
            return Json(currencyList.OrderByDescending(x => x.CurrencyID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SeaarchCountry(int? companyID)
        {
            DevourInvEntities db = new DevourInvEntities();

            var companyList = db.SET_Country.Select(x => new {x.CountryID,x.Name });
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }

    }
}