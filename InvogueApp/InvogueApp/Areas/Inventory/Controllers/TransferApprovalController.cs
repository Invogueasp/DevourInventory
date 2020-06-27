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
    public class TransferApprovalController : Controller
    {
        // GET: Inventory/TransferApproval
        private ITransferFactory transferFactory;

        private Result result;
        int userID = 1;
        DateTime todayDate = DateTime.Now;
        public ActionResult TransferApprovalList()
        {
            ViewBag.CallingForm = "Inventory";
            ViewBag.CallingForm1 = "Transfer Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }

        [HttpPost]
        public JsonResult SaveTransferApp(INV_Transfer transfer, List<VM_TempTransferDtls> transferDtls, List<int> deletetransferID)
        {
            result = new Result();
            transferFactory = new TransferFactorys();

            if (transfer.TransferID > 0)
            {
                transfer.Status = "A";
                transfer.UpdatedBy = userID;
                transfer.UpdatedDate = todayDate;
            }
            else
            {
                //transfer.Status = "P";
                transfer.CreatedBy = userID;
                transfer.CreatedDate = todayDate;
            }

            result = transferFactory.SaveTransfer(transfer, transferDtls, deletetransferID);
            return Json(result);
        }
    }
}