using BLL.Common;
using BLL.Factory.Commercial;
using BLL.Interfaces.Commercial;
using DAL.db;
using DAL.Helper;
using InvogueApp.Areas.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class POApprovalController : Controller
    {
        // GET: Inventory/POApproval
        private IPurchaseOrderFactory sprFactory;
        private Result result;
        int userID = 1;
        DateTime todayDate = DateTime.Now;
        public ActionResult POApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Purchase Order Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult PO2ndApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Purchase Order 2nd Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
     


        [HttpPost]
        public JsonResult SavePurchaseOrder(INV_PO pOrder, List<INV_PODtls> pOrderDtls, List<int> deletepOrderDtlsID,int? check)
        {
            
            result = new Result();
            sprFactory = new POrderFactorys();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
           
                pOrder.FirstApproveStatus = "A";
                pOrder.FirstApproveBy = loginUserID;
                pOrder.FirstApproveDate = todayDate;
                pOrder.CreatedBy = loginUserID;
                pOrder.CreatedDate = todayDate;

            result = sprFactory.SavePurchaseOrder(pOrder, pOrderDtls, deletepOrderDtlsID,check);
            return Json(result);
        }


        [HttpPost]
        public JsonResult SavePO2ndApp(INV_PO pOrder, List<INV_PODtls> pOrderDtls, List<int> deletepOrderDtlsID,int? check)
        {
            result = new Result();
            sprFactory = new POrderFactorys();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));

            pOrder.SecondApproveStatus = "2A";
            pOrder.SecondApproveBy = loginUserID;
            pOrder.SecondApproveDate = todayDate;
            pOrder.CreatedBy = loginUserID;
            pOrder.CreatedDate = todayDate;

            result = sprFactory.SavePurchaseOrder(pOrder, pOrderDtls, deletepOrderDtlsID, check);
            return Json(result);
        }
        [HttpPost]
        public JsonResult LoadPurchaseOrder(int? pOID)
        {
            sprFactory = new POrderFactorys();
     
            //Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            //int empID = Convert.ToInt32(dictionary[1].Id);
            DevourInvEntities db = new DevourInvEntities();
            var data = db.Database.SqlQuery<VM_PurchaseOrderAppList>("sp_PurchaseOrderAppList").ToList();
            data = data.OrderByDescending(x => x.POID).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public JsonResult LoadPO2ndApp(int? pOID)
        {
            sprFactory = new POrderFactorys();

            //Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            //int empID = Convert.ToInt32(dictionary[1].Id);
            DevourInvEntities db = new DevourInvEntities();
            var data = db.Database.SqlQuery<VM_PurchaseOrderAppList>("sp_PurchaseOrderAppList").ToList();
            data = data.OrderByDescending(x => x.POID).ToList();
            return Json(data.Where(x=>x.FirstApproveStatus == "A"), JsonRequestBehavior.AllowGet);

        }
    }
}