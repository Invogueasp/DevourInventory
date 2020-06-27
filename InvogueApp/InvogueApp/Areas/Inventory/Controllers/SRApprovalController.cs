using BLL.Common;
using BLL.Factory.Inventory;
using BLL.Interfaces.Inventory;
using DAL.db;
using DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class SRApprovalController : Controller
    {
        // GET: Inventory/SRApproval
        private IStoreReqFactory storeReqFactory;

        private Result result;
        int userID = 1;
        DateTime todayDate = DateTime.Now;
        public ActionResult SRApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Store Requisition 1st Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult SR2ndApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Store Requisition 2nd Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }

        [HttpPost]
        public JsonResult SaveStoreReqApp(INV_SR storeReq, List<INV_SRDtls> storeReqDtls, List<int> deleteStoreReqDtlsID)
        {
            result = new Result();
            storeReqFactory = new StoreReqFactorys();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (storeReq.SRID > 0)
            {   
                storeReq.Status = "A";
                storeReq.UpdatedBy = loginUserID;
                storeReq.UpdatedDate = todayDate;
            }
       

            result = storeReqFactory.SaveStoreReq(storeReq, storeReqDtls, deleteStoreReqDtlsID);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SecondAppStoreReq(INV_SR storeReq, List<INV_SRDtls> storeReqDtls, List<int> deleteStoreReqDtlsID)
        {
            result = new Result();
            storeReqFactory = new StoreReqFactorys();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (storeReq.SRID > 0)
            {
                storeReq.Status = "2A";
                storeReq.UpdatedBy = loginUserID;
                storeReq.UpdatedDate = todayDate;
            }


            result = storeReqFactory.SaveStoreReq(storeReq, storeReqDtls, deleteStoreReqDtlsID);
            return Json(result);
        }



    }
}