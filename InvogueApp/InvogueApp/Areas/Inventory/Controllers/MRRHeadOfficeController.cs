using Application.Common;
using BLL.Common;
using BLL.Factory.Commercial;
using BLL.Interfaces.Commercial;
using DAL.db;
using DAL.Helper;
using InvogueApp.Areas.Inventory.Models;
using InvogueApp.Areas.Settings.Models;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class MRRHeadOfficeController : Controller
    {
        // GET: Inventory/MRRHeadOffice
        private IPurchaseOrderFactory sprFactory;
        private IMRRHeadOfficeFactory mrrFactory;
        Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();

        private Result result;
        DateTime todayDate = DateTime.Now;
        public ActionResult MRRHeadOfficeCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Material Receive Report (Head Office)";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/MRRHeadOfficeList";
        }
        public ActionResult MRRHeadOfficeList()
        {
            ViewBag.CallingForm = "Commercial";
            ViewBag.CallingForm1 = "Material Receive Report (Head Office)";
            ViewBag.CallingViewPage = "#";
            return View();
        }


        //[HttpPost]
        //public JsonResult SaveMRRHead(INV_MRR_HO mRr, List<INV_MRR_HODtls> mRrDtls, List<int> deletepDtlsID)
        //{
        //    result = new Result();
        //    mrrFactory = new MRRHeadOfficeFactorys();

        //    if (mRr.MRRID > 0)
        //    {
        //        mRr.UpdatedBy = userID;
        //        mRr.UpdatedDate = todayDate;
        //    }
        //    else
        //    {
        //        mRr.Status = "N";
        //        mRr.CreatedBy = userID;
        //        mRr.CreatedDate = todayDate;
        //    }

        //    result = mrrFactory.SaveMRRHeadOffice(mRr, mRrDtls, deletepDtlsID);
        //    return Json(result);
        //}




        [HttpPost]
        public JsonResult SaveMRRHead()
        {
            result = new Result();
            mrrFactory = new MRRHeadOfficeFactorys();
            DevourInvEntities db = new DevourInvEntities();
            int userID = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));

            var mrrHo = Request.Form["mrrHo"];
            var productFile = JsonConvert.DeserializeObject<VM_MrrHoModel>(mrrHo);

            if (productFile.mRr.MRRID > 0)
            {
                productFile.mRr.UpdatedBy = userID;
                productFile.mRr.UpdatedDate = todayDate;
            }
            else
            {
                productFile.mRr.Status = "N";
                productFile.mRr.CreatedBy = userID;
                productFile.mRr.CreatedDate = todayDate;
            }
            result = mrrFactory.SaveMRRHeadOffice(productFile.mRr, productFile.mRrDtls, productFile.deletepDtlsID);
            if (result.isSucess == true)
            {
                var appData = db.INV_PO.Where(x=>x.POID == productFile.mRr.POID).FirstOrDefault();
                appData.SecondApproveStatus = "F";
                db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                  db.SaveChanges();
            }

            result.lastInsertedID = productFile.mRr.MRRID;
            string path = Server.MapPath("~/Content/File/MRR/Attachment/" + productFile.mRr.MRRID + "/");
            try
            {

                if (result.isSucess)
                {
                    if (Request.Files.Count > 0)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var uniqueName = productFile.mRr.MRRID.ToString();
                        foreach (string key in Request.Files)
                        {
                            HttpPostedFileBase file = Request.Files[key];
                            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName) ?? "";
                            var fileName = Path.GetFileName(file.FileName) ?? "";
                            var newFileName = fileName.Replace(fileNameWithoutExt, uniqueName);
                            var fileNameWithoutExt2 = Path.GetFileNameWithoutExtension(newFileName);
                            string fullPath = path + newFileName;

                            DirectoryInfo directory2 = new DirectoryInfo(path);
                            if (directory2.Exists)
                            {
                                var fileGetName2 = directory2.EnumerateFiles().Where(x => x.Name.ToLower() == (fileNameWithoutExt2.ToLower() + ".png") || x.Name.ToLower() == (fileNameWithoutExt2.ToLower() + ".jpg") || x.Name.ToLower() == (fileNameWithoutExt2.ToLower() + ".jpeg")).Select(x => x.Name).FirstOrDefault();
                                if (fileGetName2 != null)
                                {
                                    var delPath = path + fileGetName2;
                                    System.IO.File.Delete(delPath);
                                }
                                else
                                {
                                    if (System.IO.File.Exists(fullPath))
                                    {
                                        System.IO.File.Delete(fullPath);
                                    }
                                }
                            }
                            file.SaveAs(fullPath);
                        }
                    }



                }


            }
            catch (Exception ex)
            {
                result.isSucess = false;
                result.message = ex.InnerException.Message;
            }

            return Json(result);
        }


        [HttpPost]
        public JsonResult GetMrrImgFile(string mrrID)
        {
            string path = Server.MapPath("~/Content/File/MRR/Attachment/" + mrrID + "/");
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                var fileData = directory.EnumerateFiles().Where(x => x.Name == mrrID + ".jpg" || x.Name == mrrID + ".png" || x.Name == mrrID + ".pdf" || x.Name == mrrID + ".jpeg").Select(x => new { x.Name, x.FullName });
               
                return Json(fileData);
            }
            return Json("");
        }

        [HttpPost]
        public VM_ForProductImg GetDownloadPhoto(string code)
        {
            string path = Server.MapPath("~/Content/File/MRR/Attachment/" + code + "/");
            DirectoryInfo directory = new DirectoryInfo(path);
            VM_ForProductImg fileData = new VM_ForProductImg();
            if (directory.Exists)
            {
                var file = directory.EnumerateFiles().FirstOrDefault();   //.Where(x => x.Name.Contains("Photo")).Select(x => new { x.Name, x.FullName })
                if (file == null)
                {
                    string paths = Server.MapPath("~/Content/File/MRR/Attachment/");
                    DirectoryInfo directorys = new DirectoryInfo(paths);

                    file = directorys.EnumerateFiles().FirstOrDefault();   //.Where(x => x.Name.Contains("empPhoto")).Select(x => new { x.Name, x.FullName })
                }

                //  fileData.ImgPathName = file.Name;
                fileData.ImageURL = file.Name;
                return fileData;

            }
            else
            {
                fileData.ImageURL = "N";
                //string paths ="" ;
                //DirectoryInfo directorys = new DirectoryInfo(paths);

                //var file = directorys.EnumerateFiles().FirstOrDefault();
                //// .Where(x => x.Name.Contains("empPhoto")).Select(x => new { x.Name, x.FullName })
                //if (file != null)
                //{
                //    // fileData.ImgPathName = file.Name;
                //    fileData.ImageURL = file.Name;
                //}
                //else
                //{
                //    fileData.ImageURL = "";
                //}

                return fileData;
            }

        }

        [HttpPost]
        public JsonResult LoadMRRHead(int? mRRID, VM_Parameters param)
        {
            List<VM_MrrHo> mrrList = new List<VM_MrrHo>();
            DevourInvEntities db = new DevourInvEntities();
            //mrrFactory = new MRRHeadOfficeFactorys();
            //List<INV_MRR_HO> list = mrrFactory.SearchMRRHeadOffice(mRRID);
               var mRRList = (from x in db.INV_MRR_HO
                          join po in db.INV_PO on x.POID equals po.POID
                          join spr in db.INV_SPR on po.SPRID equals spr.SPRID
                          join u in db.SEC_UserInformation on x.CreatedBy equals u.ID
                          join d in db.INV_Department on u.DepartmentID equals d.DepartmentID
                          join cb in db.SET_CompanyBranch on spr.BranchID equals cb.BranchID
                          select new
                          {
                              x.POID,
                              x.MRRID,
                              x.Status,
                              x.SupplierID,
                              x.SupplierInv,
                              x.MRRNO,
                              x.INV_PO.PONO,
                              x.CreatedDate,
                              x.CreatedBy,
                              x.UpdatedBy,
                              x.MRRDate,
                              x.INV_Supplier.Name,
                              spr.BranchID,
                              u.DepartmentID,
                              Department = d.Name,
                              BranchName = cb.Name


                          }).ToList();

               if (param.FormDate != null && param.ToDate != null)
               {
                   mRRList = mRRList.Where(x => x.MRRDate >= param.FormDate && x.MRRDate <= param.ToDate).ToList();
               }
               if (param.DepartmentID > 0)
               {
                   mRRList = mRRList.Where(x => x.DepartmentID == param.DepartmentID).ToList();
               }
               if (param.BranchID > 0)
               {
                   mRRList = mRRList.Where(x => x.BranchID == param.BranchID).ToList();
               }

               if (mRRList.Count() > 0)
               {
                   foreach (var id in mRRList)
                   {
                       VM_MrrHo mrrModel = new VM_MrrHo();
                       var Eimg = GetDownloadPhoto(id.MRRID.ToString());
                       mrrModel.BranchID = id.BranchID;
                       mrrModel.DepartmentID = id.DepartmentID;
                       mrrModel.Department = id.Department;
                       mrrModel.BranchName = id.BranchName;
                       mrrModel.CreatedDate = id.CreatedDate;
                       mrrModel.ImageURL = Eimg.ImageURL;
                       mrrModel.MRRDate = id.MRRDate;
                       mrrModel.MRRID = id.MRRID;
                       mrrModel.MRRNO = id.MRRNO;
                       mrrModel.Name = id.Name;
                       mrrModel.POID = id.POID;
                       mrrModel.PONO = id.PONO;
                       mrrModel.Status = id.Status;
                       mrrModel.SupplierID = id.SupplierID;
                       mrrModel.SupplierInv = id.SupplierInv;
                       mrrModel.CreatedDate = id.CreatedDate;
                       mrrModel.CreatedBy = id.CreatedBy;
                       mrrModel.UpdatedBy = id.UpdatedBy;

                       mrrList.Add(mrrModel);
                   }
               }
            

            return Json(mrrList.OrderByDescending(x=>x.MRRID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadMrrHODtls(int? pOID)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", pOID);
            var mrRDtlsList = db.Database.SqlQuery<VM_MrrDtlsList>("sp_MrrHODtlsList @p1", p1).ToList();
            return Json(mrRDtlsList.OrderByDescending(x => x.MRRID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadMrrHODtlsForMRS(int? mrrID)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", mrrID);
            var mrRDtlsList = db.Database.SqlQuery<VM_MrrDtlsList>("sp_MrrHODtlsListForMRS @p1", p1).ToList();
            return Json(mrRDtlsList.OrderByDescending(x => x.MRRID), JsonRequestBehavior.AllowGet);
        }



   

        //======================================== Report ==============================
        CommonFunctions commonFunctions = new CommonFunctions();
        DateTime ToDate = DateTime.Now;


        [HttpPost]
        public JsonResult LoadReportList(int? pOID)
        {
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", pOID);
            var mrRDtlsList = db.Database.SqlQuery<VW_ReportList>("BasicInfoFrGetPassAndDC @P1", p1).ToList();
            var reportList = new
            {
                mrRDtlsLists = mrRDtlsList.OrderByDescending(x => x.MRRID),
                PONO = mrRDtlsList.Select(x=>x.PONO).FirstOrDefault(),
                MRRNO = mrRDtlsList.Select(x => x.MRRNO).FirstOrDefault()
            };
            return Json(reportList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GatePassReportView(VM_Perameter parameters)
        {
            DevourInvEntities db = new DevourInvEntities();

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";


            SqlParameter p1 = new SqlParameter("@iPOID", parameters.POID);
            SqlParameter p2 = new SqlParameter("@iSPRID", parameters.SPRID);

            var dataList = db.Database.SqlQuery<VM_MrrDtlsList>("sp_MrrHODtlsList @iPOID,@iSPRID", p1, p2).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "GatePassReport.rdlc");
                reportDataSetName = "MrrHODtlsDataSet";

            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
            }

            List<ReportParameter> reportParameterss = new List<ReportParameter>();
            ReportParameter parameter = new ReportParameter("ToDate");
            parameter.Values.Add(ToDate.ToString());
            reportParameterss.Add(parameter);

            
             int branchID = parameters.BranchID;

             int? loginUserID = 0;
             fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
            return Json(fileString);

        }



        public JsonResult DeliveryChallanReportView(VM_Perameter parameters)
        {
            DevourInvEntities db = new DevourInvEntities();

            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";


            SqlParameter p1 = new SqlParameter("@P1", parameters.POID);
            SqlParameter p2 = new SqlParameter("@P2", parameters.SPRID);
            var dataList = db.Database.SqlQuery<VM_MrrDtlsList>("sp_MrrHODtlsList @P1, @P2", p1,p2).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "DeliveryChallan.rdlc");
                reportDataSetName = "MrrHODtlsDataSet";

            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
            }

            List<ReportParameter> reportParameterss = new List<ReportParameter>();
            ReportParameter parameter = new ReportParameter("ToDate");
            parameter.Values.Add(ToDate.ToString());
            reportParameterss.Add(parameter);


            int? loginUserID = 0;
            fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
            return Json(fileString);

        }

        public JsonResult MaterialReceiveHOReport(VM_PerametersHO parameters)
        {


            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int branchID = Convert.ToInt32(dictionary[10].Id == "" ? 0 : Convert.ToInt32(dictionary[10].Id));

            DevourInvEntities db = new DevourInvEntities();

            string path = string.Empty;
            string reportDataSetName = string.Empty;
            string fileString = string.Empty;
            string docType = "pdf";


            SqlParameter p1 = new SqlParameter("@P1", parameters.MRRID);
            var dataList = db.Database.SqlQuery<VM_MrrDtlsLists>("sp_MrrHODtls_Report @p1", p1).ToList();


            if (dataList.Count > 0)
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "MrrHOReport.rdlc");
                reportDataSetName = "MRRHoDataSets";

            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
            }

            List<ReportParameter> reportParameterss = new List<ReportParameter>();

            int? loginUserID = 0;
            fileString = commonFunctions.CallReports(docType, reportParameterss, true, path, null, dataList, reportDataSetName, branchID, loginUserID);
            return Json(fileString);

        }

    }
    public class VM_PerametersHO
    {
        public int MRRID { get; set; }

    }
    public class VW_ReportList
    {
        public int SPRID { get; set; }
        public int MRRID { get; set; }
        public int POID { get; set; }
        public string SPRNO { get; set; }
        public string PONO { get; set; }
        public string MRRNO { get; set; }
    }
    public class VM_Perameter
    {
        public int POID { get; set; }
        public int BranchID { get; set; }
        public int SPRID { get; set; }

     
    }
    public class VM_MrrHo
    {
         public int POID { get; set; }
         public int MRRID { get; set; }
         public int SupplierID { get; set; }
         public int BranchID { get; set; }

         public DateTime CreatedDate { get; set; }
         public DateTime MRRDate { get; set; }

         public string Status { get; set; }
         public string SupplierInv { get; set; }
          public string MRRNO { get; set; }
         public string PONO { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public int? DepartmentID { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string BranchName { get; set; }
        public string ImageURL { get; set; }
        
    }

}