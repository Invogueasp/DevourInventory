using BLL.Common;
using BLL.Factory.Commercial;
using BLL.Interfaces.Commercial;
using DAL.db;
using InvogueApp.Areas.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Helper;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class MRSController : Controller
    {
        // GET: Inventory/MRS
        private IPurchaseOrderFactory sprFactory;
        private IMRSFactory mrrFactory;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
        DevourInvEntities db = new DevourInvEntities();

        private Result result;
        DateTime todayDate = DateTime.Now;
        public ActionResult MRSList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Material Receive Report";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult MRSCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Material Receive In Store";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/MRSList";
        }
        [HttpPost]
        public JsonResult SaveMRS(INV_MaterialReceive mRr, List<INV_MaterialReceiveDtls> mRrDtls, List<int> deletepDtlsID)
        {
            DevourInvEntities db = new DevourInvEntities();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));

            result = new Result();
            mrrFactory = new MRSFactorys();

            if (mRr.MaterialReceiveID > 0)
            {
                mRr.UpdatedBy = userID;
                mRr.UpdatedDate = todayDate;
            }
            else
            {
                mRr.Status = "A";
                mRr.CreatedBy = userID;
                mRr.CreatedDate = todayDate;
            }

            result = mrrFactory.SaveMRS(mRr, mRrDtls, deletepDtlsID);

            if (result.isSucess == true)
            {
                var appData = db.INV_MRR_HO.Where(x => x.POID == mRr.POID).FirstOrDefault();
                appData.Status = "F";
                db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }


            return Json(result);
        }
        [HttpPost]
        public JsonResult LoadMRS(int? mRRID, VM_Parameters param)
        {
            mrrFactory = new MRSFactorys();
            //List<INV_MaterialReceive> list = mrrFactory.SearchMRS(mRRID);
            //var mRRList = list.Select(x => new
            //{
            //    x.POID,
            //    x.MaterialReceiveID,
            //    x.Status,
            //    x.ReceiveNO,
            //    x.INV_PO.PONO,
            //    x.CreatedDate,
            //    x.ReceiveDate,
                
            //});

            var mRRList = (from x in db.INV_MaterialReceive
                join po in db.INV_PO on x.POID equals po.POID
                join spr in db.INV_SPR on po.SPRID equals spr.SPRID
                join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                join cb in db.SET_CompanyBranch on spr.BranchID equals cb.BranchID
                select new
                {
                    x.POID,
                    x.MaterialReceiveID,
                    x.Status,
                    x.ReceiveNO,
                    po.PONO,
                    x.CreatedBy,
                    x.UpdatedBy,
                    x.UpdatedDate,
                    x.CreatedDate,
                    x.ReceiveDate,
                    spr.BranchID,
                    u.DepartmentID,
                    Department = d.Name,
                    BranchName = cb.Name
                }).ToList();

            if (mRRID > 0)
            {
                mRRList = mRRList.Where(x => x.MaterialReceiveID == mRRID).ToList();
            }
            if (param.FormDate != null && param.ToDate != null)
            {
                mRRList = mRRList.Where(x => x.ReceiveDate >= param.FormDate && x.ReceiveDate <= param.ToDate).ToList();
            }
            if (param.DepartmentID > 0)
            {
                mRRList = mRRList.Where(x => x.DepartmentID == param.DepartmentID).ToList();
            }
            if (param.BranchID > 0)
            {
                mRRList = mRRList.Where(x => x.BranchID == param.BranchID).ToList();
            }

            return Json(mRRList.OrderByDescending(x => x.MaterialReceiveID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadMRSDtls(int? mrrID)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", mrrID);
            var mrRDtlsList = db.Database.SqlQuery<VM_MRRStoreDtlsList>("sp_MRRSDtlsList @p1", p1).ToList();
            return Json(mrRDtlsList.OrderByDescending(x => x.MaterialReceiveID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadMRSDtlsFrList(int? mrrid)
        {
             DevourInvEntities db = new DevourInvEntities();
             SqlParameter p1 = new SqlParameter("@P1", mrrid);
             var mrRDtlsList = db.Database.SqlQuery<VM_MRSDtlsList>("sp_MRRSDtls @p1", p1).ToList();

            return Json(mrRDtlsList.OrderByDescending(x => x.MaterialReceiveID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadMRSDtlsForReturn(int? pOID)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", pOID);
            var mrRDtlsList = db.Database.SqlQuery<VM_MRSFrReturnDtlsList>("sp_ReturnMRRSDtlsList @p1", p1).ToList();
            return Json(mrRDtlsList.OrderByDescending(x => x.MaterialReceiveID), JsonRequestBehavior.AllowGet);
        }

    }
}