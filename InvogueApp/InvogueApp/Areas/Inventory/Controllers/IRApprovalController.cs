using BLL.Common;
using BLL.Factory.Inventory;
using BLL.Interfaces.Inventory;
using BLL.Models;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class IRApprovalController : Controller
    {
        // GET: Inventory/IRApproval

        private IItemReturnFactory itemReturnFactory;

        private Result result;
        public ActionResult IRApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Item Return Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        [HttpPost]
        public JsonResult SaveIetmReturnDtls(INV_Return ietmReturn, List<VM_TempItemReturnDtls> ietmReturnDtls)
        {
            result = new Result();
            itemReturnFactory = new ItemReturnFactorys();

            ietmReturn.Status = "A";
            result = itemReturnFactory.SaveIetmReturn(ietmReturn, ietmReturnDtls);
            return Json(result);
        }
    }
}