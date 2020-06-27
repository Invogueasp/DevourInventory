using BLL.Factory.Inventory;
using BLL.Interfaces.Inventory;
using DAL.db;
using InvogueApp.Areas.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class CurrentStockController : Controller
    {
        // GET: Inventory/CurrentStock
        private ICurrentStock stockFactory;
        private DevourInvEntities db;
        public ActionResult CurrentStockList()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Current Stock";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        [HttpPost]
        public JsonResult LoadByStore(int? storeID)
        {
            stockFactory = new CurrentStockFactorys();
            List<INV_Stock> list = stockFactory.SearchStockByStore(storeID);
            var stockList = list.Select(x => new
            {
                x.CategoryID,
                x.ProductID,
                x.UnitID,
                x.StockID,
                x.BranchID,
                StoreName = x.SET_CompanyBranch == null ? "" : x.SET_CompanyBranch.Name,
                x.Quantity,
                WarehouseName = x.SET_CompanyBranch == null ? "" : x.SET_CompanyBranch.Name,
                CategoryName = x.INV_Category.Name,
                ProductName = x.INV_Product.Name,
                UnitName = x.INV_Unit.Name

            });
            return Json(stockList, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public JsonResult LoadByWarehouse(int? warehouseID)
        //{
        //    stockFactory = new CurrentStockFactorys();
        //    List<INV_Stock> list = stockFactory.SearchStockByWarehouse(warehouseID);
        //    var stockList = list.Select(x => new
        //    {
        //        x.CategoryID,
        //        x.ProductID,
        //        x.UnitID,
        //        x.StockID,
        //        x.BranchID,
        //        StoreName = x.SET_CompanyBranch == null ? "" : x.SET_CompanyBranch.Name,
        //        x.Quantity,
        //        WarehouseName = x.SET_CompanyBranch == null ? "" : x.SET_CompanyBranch.Name,
        //        CategoryName = x.INV_Category.Name,
        //        ProductName = x.INV_Product.Name,
        //        UnitName = x.INV_Unit.Name

        //    });
        //    return Json(stockList, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult LoadStock(int? stockID)
        {
            stockFactory = new CurrentStockFactorys();
            List<INV_Stock> list = stockFactory.SearchStock(stockID);
            var stockList = list.Select(x => new
            {
                x.CategoryID,
                x.ProductID,
                x.UnitID,
                x.StockID,
                x.BranchID,
                StoreName = x.SET_CompanyBranch == null ? "" : x.SET_CompanyBranch.Name,
                x.Quantity,
                CategoryName = x.INV_Category.Name,
                ProductName = x.INV_Product.Name,
                UnitName = x.INV_Unit.Name

            });
            return Json(stockList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadStock2(string branchID, string categoryID, string Namevalue, int Pageindex, int Pagesize)
        {
            db = new DevourInvEntities();
            if (branchID == null)
            {
                branchID = "";
            }
            if (categoryID == null)
            {
                categoryID = "";
            }
            SqlParameter p1 = new SqlParameter("@iBranchID", branchID);
            SqlParameter p2 = new SqlParameter("@iCategoryID", categoryID);
            SqlParameter p3 = new SqlParameter("@iNamevalue", Namevalue);
            SqlParameter p4 = new SqlParameter("@iPageindex", Pageindex);
            SqlParameter p5 = new SqlParameter("@iPagesize", Pagesize);
            var stockList = db.Database.SqlQuery<VM_Stock>("sp_StockList @iBranchID, @iCategoryID, @iNamevalue, @iPageindex, @iPagesize", p1, p2, p3, p4, p5).ToList();
            return Json(stockList, JsonRequestBehavior.AllowGet);
        }
    }
}