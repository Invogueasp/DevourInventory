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
    public class ItemTransferController : Controller
    {
        // GET: Inventory/ItemTransfer
        private ITransferFactory transferFactory;

        private Result result;
        int userID = 1;
        DateTime todayDate = DateTime.Now;
        public ActionResult TransferList()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Item Transfer";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult TransferCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Item Transfer";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/TransferList";
        }

        [HttpPost]
        public JsonResult SaveTransfer(INV_Transfer transfer, List<VM_TempTransferDtls> transferDtls, List<int> deletetransferID)
        {
            result = new Result();
            transferFactory = new TransferFactorys();

            if (transfer.TransferID > 0)
            {

                transfer.UpdatedBy = userID;
                transfer.UpdatedDate = todayDate;
            }
            else
            {
                transfer.Status = "P";
                transfer.CreatedBy = userID;
                transfer.CreatedDate = todayDate;
            }

            result = transferFactory.SaveTransfer(transfer, transferDtls, deletetransferID);
            return Json(result);
        }


        [HttpPost]
        public JsonResult LoadTransfer(int? transferID)
        {
            DevourInvEntities db = new DevourInvEntities();
            transferFactory = new TransferFactorys();
        
            var transferList = from t in db.INV_Transfer
                               join s in db.SET_CompanyBranch on t.ToStoreID equals s.BranchID
                               join s2 in db.SET_CompanyBranch on t.FromStoreID equals s2.BranchID

                               select new
                               {t.TransferDate,
                                   t.TransferID,
                                   t.TransferNO,
                                   t.Status,
                                   t.CreatedDate,
                                   t.CreatedBy,
                                   t.ToStoreID,
                                   t.FromStoreID,
                                   ToStore = s.Name,
                                   FromStore = s2.Name,


                               };

            return Json(transferList.OrderByDescending(x=>x.TransferID).OrderByDescending(x => x.TransferID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadTransferDtls(int? transferID)
        {
            transferFactory = new TransferFactorys();
            List<INV_TransferDtls> list = transferFactory.SearchTransferDtls(transferID);
            var transferDtlsList = list.Select(x => new
            {
                x.CategoryID,
                x.ProductID,
                x.ApprovalRemarks,
                x.TransferDtlsID,
                x.UnitID,
                x.Remarks,
                x.TransferQty,
                CategoryName = x.INV_Category.Name,
                ProductName = x.INV_Product.Name,
                UnitName = x.INV_Unit.Name

            });
            return Json(transferDtlsList, JsonRequestBehavior.AllowGet);
        }
    }
}