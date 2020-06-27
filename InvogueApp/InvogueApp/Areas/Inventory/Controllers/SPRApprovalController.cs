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
    public class SPRApprovalController : Controller
    {
        private ISPRFactory sprFactory;
        private Result result;
        int userID = 1;
        DateTime todayDate = DateTime.Now;
        // GET: Inventory/SPRApproval
        public ActionResult SPRApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Store Purchase Requisition Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult SPR2ndApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Store Purchase Requisition 2nd Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult SPR3rdApprovalList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Store Purchase Requisition 3rd Approval";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        [HttpPost]
        public JsonResult SaveSPRApp(INV_SPR storePR, List<INV_SPRDtls> storePRDtls, List<int> deleteSPRDtlsID)
        {
            result = new Result();
            sprFactory = new SPRFactorys();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (storePR.SPRID > 0)
            {
                storePR.FirstApproveStatus = "A";
                storePR.FirstApproveBy = loginUserID;
                storePR.FirstApproveDate = todayDate;
                storePR.SecondApproveStatus = "2A";
                storePR.SecondApproveBy = loginUserID;
                storePR.SecondApproveDate = todayDate;
                storePR.ThirdApproveStatus = "3A";
                storePR.ThirdApproveBy = loginUserID;
                storePR.ThirdApproveDate = todayDate;
                storePR.UpdatedBy = loginUserID;
                storePR.UpdatedDate = todayDate;
            }
            else
            {
                storePR.FirstApproveStatus = "A";
                storePR.FirstApproveBy = loginUserID;
                storePR.FirstApproveDate = todayDate;
                storePR.SecondApproveStatus = "2A";
                storePR.SecondApproveBy = loginUserID;
                storePR.SecondApproveDate = todayDate;
                storePR.ThirdApproveStatus = "3A";
                storePR.ThirdApproveBy = loginUserID;
                storePR.ThirdApproveDate = todayDate;
                storePR.CreatedBy = userID;
                storePR.CreatedDate = todayDate;
            }

            result = sprFactory.SaveSPR(storePR, storePRDtls, deleteSPRDtlsID);
            return Json(result);
        }

        
        [HttpPost]
        public JsonResult SaveSPR2ndApp(INV_SPR storePR, List<INV_SPRDtls> storePRDtls, List<int> deleteSPRDtlsID)
        {
            result = new Result();
            sprFactory = new SPRFactorys();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (storePR.SPRID > 0)
            {
                storePR.SecondApproveStatus = "2A";
                storePR.SecondApproveBy = loginUserID;
                storePR.SecondApproveDate = todayDate;
                storePR.UpdatedBy = loginUserID;
                storePR.UpdatedDate = todayDate;
            }
            else
            {
                storePR.SecondApproveStatus = "2A";
                storePR.SecondApproveBy = loginUserID;
                storePR.SecondApproveDate = todayDate;
                storePR.CreatedBy = userID;
                storePR.CreatedDate = todayDate;
            }

            result = sprFactory.SaveSPR(storePR, storePRDtls, deleteSPRDtlsID);
            return Json(result);
        }

        [HttpPost]
        public JsonResult SaveSPR3rdApp(INV_SPR storePR, List<INV_SPRDtls> storePRDtls, List<int> deleteSPRDtlsID)
        {
            result = new Result();
            sprFactory = new SPRFactorys();
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int loginUserID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
            if (storePR.SPRID > 0)
            {
                storePR.ThirdApproveStatus = "3A";
                storePR.ThirdApproveBy = loginUserID;
                storePR.ThirdApproveDate = todayDate;
                storePR.UpdatedBy = loginUserID;
                storePR.UpdatedDate = todayDate;
            }
            else
            {
                storePR.ThirdApproveStatus = "3A";
                storePR.ThirdApproveBy = loginUserID;
                storePR.ThirdApproveDate = todayDate;
                storePR.CreatedBy = userID;
                storePR.CreatedDate = todayDate;
            }

            result = sprFactory.SaveSPR(storePR, storePRDtls, deleteSPRDtlsID);
            return Json(result);
        }
        
        

    }
}