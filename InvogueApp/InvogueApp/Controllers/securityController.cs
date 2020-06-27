using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLL.Factory;
using BLL.Factory.Security;
using BLL.Interfaces;
using BLL.Interfaces.Security;
using BLL.Models;
using DAL.db;
using DAL.Helper;
using BLL.Common;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;

namespace Application.Controllers
{
    public class SecurityController : Controller
    {
        private IGenericFactory<SEC_UIPage> _uiPageFactory;
        private IGenericFactory<SEC_UIModule> _uiModuleFactory;
        private IGenericFactory<SEC_UserActionMapping> _uiMappingFactory;
        private IGenericFactory<SET_CompanyBranch> _companyBranchFactory;

        private ISecurityFactory _securityFactory;
        private IGenericFactory<SEC_UserInformation> _userInformationFactory;
        private IGenericFactory<SEC_LoginStatus> _loginStatusFactory;

        private ICompanyBranchFactory _comBranchFactory;
        public ActionResult Index()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "Dashboard");
                if (tblUserActionMapping.Select == true)
                {
                    return View();
                }
            }
            Session["logInSession"] = "false";
            return View();
        }
        public ActionResult SecurityMaster()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int userGroupId = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            if (userGroupId != 0)
            {
                ISecurityFactory securityLogInFactory = new SecurityFactorys();
                PagePermissionVM tblUserActionMapping = securityLogInFactory.GetCrudPermission(userGroupId, "SecurityMaster");
                if (tblUserActionMapping != null && tblUserActionMapping.Select == true)
                {
                    return View();
                }
            }
            Session["logInSession"] = "false";
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool getLan = false;
                    string visitorIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (String.IsNullOrEmpty(visitorIpAddress))
                        visitorIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    if (string.IsNullOrEmpty(visitorIpAddress))
                        visitorIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                    if (string.IsNullOrEmpty(visitorIpAddress) || visitorIpAddress.Trim() == "::1")
                    {
                        getLan = true;
                        visitorIpAddress = string.Empty;
                    }
                    if (getLan && string.IsNullOrEmpty(visitorIpAddress))
                    {
                        //This is for Local(LAN) Connected ID Address
                        string stringHostName = Dns.GetHostName();
                        //Get Ip Host Entry
                        IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                        ipHostEntries = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]);

                        //Get Ip Address From The Ip Host Entry Address List
                        IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                        try
                        {
                            visitorIpAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                        }
                        catch
                        {
                            try
                            {
                                visitorIpAddress = arrIpAddress[0].ToString();
                            }
                            catch
                            {
                                try
                                {
                                    arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                    visitorIpAddress = arrIpAddress[0].ToString();
                                }
                                catch
                                {
                                    visitorIpAddress = "127.0.0.1";
                                }
                            }
                        }

                    }

                    ////////////////////////////////////
                    _securityFactory = new SecurityFactorys(); 
                    _userInformationFactory = new UserFactory();
                    _companyBranchFactory = new CompanyBranchFactory();
                    var logInStatus = _securityFactory.CheckLogIn(new LogOnModel { CompanyID = model.CompanyID, BranchID = model.BranchID, UserName = model.UserName.ToLower().Trim(), Password = model.Password });

                    if (logInStatus.IsAllowed)
                    {
                        var aSecurityUser = _userInformationFactory.FindBy(x => x.UserName.Contains(model.UserName)).FirstOrDefault();
                        var branch = _companyBranchFactory.FindBy(x => x.BranchID == aSecurityUser.BranchID).FirstOrDefault();
                      

                        if (aSecurityUser != null)
                        {

                            //System.Web.HttpContext.Current.Session["LoginEmployee"] = aSecurityUser.PersonnelId;
                            System.Web.HttpContext.Current.Session["LoginUserID"] = aSecurityUser.ID;
                            System.Web.HttpContext.Current.Session["LoginCompanyID"] = aSecurityUser.CompanyID;
                            System.Web.HttpContext.Current.Session["LoginBranchID"] = aSecurityUser.BranchID;
                            System.Web.HttpContext.Current.Session["LoginUserName"] = aSecurityUser.UserName;
                            System.Web.HttpContext.Current.Session["LoginUserFullName"] = aSecurityUser.UserFullName + " (" + branch.Name + ")";
                            System.Web.HttpContext.Current.Session["UserGroupID"] = aSecurityUser.UserGroupID;
                            System.Web.HttpContext.Current.Session["IPAddress"] = visitorIpAddress;
                            string[] computerName = null;
                            //try
                            //{
                            //    computerName = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.Split(new Char[] { '.' });
                            //}
                            //catch (Exception)
                            //{

                            //}
                            if (computerName != null)
                            {
                                System.Web.HttpContext.Current.Session["PCName"] = computerName[0];
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session["PCName"] = "N/A";
                            }


                            if (!String.IsNullOrEmpty(model.UserName))
                            {
                                if (!aSecurityUser.UserName.Equals(model.UserName, StringComparison.Ordinal))
                                {
                                    return Json(new { success = false, message = "Incorrect User Name or Password." }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session["LoginUserID"] = 0;
                            }

                            if (!logInStatus.IsAllowed)
                            {
                                return Json(new { success = false, message = logInStatus.Message }, JsonRequestBehavior.AllowGet);
                            }
                            //if (String.IsNullOrEmpty(model.UserName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
                            //FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                            SEC_LoginStatus tblLogInStatus = new SEC_LoginStatus();
                            _loginStatusFactory = new LoginStatusFactory();
                            tblLogInStatus.UserID = aSecurityUser.ID;
                            tblLogInStatus.PresentLogInStatus = true;
                            tblLogInStatus.LogInTime = DateTime.Now;
                            tblLogInStatus.LogOutTime = DateTime.Now;
                            tblLogInStatus.ForcedLogOutStatus = false;
                            _loginStatusFactory.Add(tblLogInStatus);
                            _loginStatusFactory.Save();
                            Session["logInSession"] = "true";
                            return Json( new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                    //
                        }
                        return Json(new { success = false, message = "The user name or password provided is incorrect." }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = logInStatus.Message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "The user name or password provided is incorrect." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //Route();
                return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { success = false, message = "The user name or password provided is incorrect. 4" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOff()
        {
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                if (dictionary[3].Id != null || dictionary[3].Id != "")
                {
                    int userId = Convert.ToInt32(dictionary[3].Id);
                    _loginStatusFactory = new LoginStatusFactory();

                    SEC_LoginStatus loginStatus = _loginStatusFactory.FindBy(x => x.UserID == userId).FirstOrDefault();
                    loginStatus.PresentLogInStatus = false;
                    loginStatus.LogOutTime = DateTime.Now;
                    loginStatus.ForcedLogOutStatus = false;
                    _loginStatusFactory.Edit(loginStatus);
                    _loginStatusFactory.Save();

                    System.Web.HttpContext.Current.Session["LoginUserID"] = 0;
                    System.Web.HttpContext.Current.Session["LoginUserID"] = 0;
                    System.Web.HttpContext.Current.Session["LoginUserName"] = 0;
                    System.Web.HttpContext.Current.Session["LoginEmployee"] = 0;
                    System.Web.HttpContext.Current.Session["LoginCompanyID"] = 0;
                    System.Web.HttpContext.Current.Session["LoginBranchID"] = 0;
                    System.Web.HttpContext.Current.Session["LoginUserFullName"] = 0;
                    System.Web.HttpContext.Current.Session["UserGroupID"] = 0;
                    System.Web.HttpContext.Current.Session["IPAddress"] = 0;
                    Session["logInSession"] = "false";

                    return Redirect("/#!/");
                }
                return Redirect("/#!/");
            }
            catch (Exception)
            {
                return Redirect("/#!/");
            }
        }

        [HttpGet]
        public JsonResult GetSiteMenu()
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int _userGroupID = Convert.ToInt32(dictionary[6].Id == "" ? 0 : Convert.ToInt32(dictionary[6].Id));
            ISecurityFactory _securityLogInFactory = new SecurityFactorys();
            var _menu = _securityLogInFactory.PagePermissedList(_userGroupID);
            return Json(new { menu = _menu, userGroupID = _userGroupID }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllCountData()
        {
            DevourInvEntities db = new DevourInvEntities();
            var purchaseOrderCounter = 0;
            var materialReceiveCounter = 0;
            var qualityCertificateCounter = 0;
            var materialReceiveReportCounter = 0;
            // Spr Approval
            var sprPendingCount = db.INV_SPR.Where(x => x.FirstApproveStatus == null).Count();
            // Purchase Order
            var sprApprovalData = (from sp in db.INV_SPR
                where sp.ThirdApproveStatus == "3A"
                select new
                {
                    sp.SPRID
                }).ToList();
            foreach (var details in sprApprovalData)
            {
                var countData = db.INV_PO.Where(x => x.SPRID == details.SPRID).Count();
                if (countData < 1)
                {
                    purchaseOrderCounter++;
                }
                
            }
            //Purchase Order Approval
            var purchaseOrderFirstApproval = db.INV_PO.Where(x => x.FirstApproveStatus == null).Count();
            var purchaseOrderSecApproval = db.INV_PO.Where(x => x.FirstApproveStatus == "A" && x.SecondApproveStatus == null).Count();
            // Material Receive Store
            var materialReceiveHDOffice = (from sp in db.INV_MRR_HO
                select new
                {
                    sp.POID
                }).ToList();
            foreach (var details in materialReceiveHDOffice)
            {
                var countData = db.INV_MaterialReceive.Where(x => x.POID == details.POID).Count();
                if (countData < 1)
                {
                    materialReceiveCounter++;
                }

            }
            // Quality Certificate
            var materialReceive = (from sp in db.INV_MaterialReceive
                select new
                {
                    sp.MaterialReceiveID
                }).ToList();
            foreach (var details in materialReceive)
            {
                var countData = db.INV_QC.Where(x => x.MaterialReceiveID == details.MaterialReceiveID).Count();
                if (countData < 1)
                {
                    qualityCertificateCounter++;
                }

            }
            // // Material Receive Report
            var materialReceiveReport = (from sp in db.INV_QC
                join mr in db.INV_MaterialReceive on sp.MaterialReceiveID equals mr.MaterialReceiveID 
                select new
                {
                    mr.POID
                }).ToList();
            foreach (var details in materialReceiveReport)
            {
                var countData = db.INV_MRR.Where(x => x.POID == details.POID).Count();
                if (countData < 1)
                {
                    materialReceiveReportCounter++;
                }

            }
            // Item Return 
            var itemReturnApproval = db.INV_Return.Where(x => x.Status == "P").Count();
            // Store Requisition
            var storeRequisitionFirstApproval = db.INV_SR.Where(x => x.Status == "P").Count();
            var storeRequisitionSecApproval = db.INV_SR.Where(x => x.Status == "A").Count();
            // Item Issue
            var itemIssue = db.INV_SR.Where(x => x.Status == "2A").Count();
            var datalist = new
            {
                sprApprovalCountData = sprPendingCount,
                PurchaseOrderCountData = purchaseOrderCounter,
                PurchaseOrderFirstAppCountData = purchaseOrderFirstApproval,
                PurchaseOrderSecAppCountData = purchaseOrderSecApproval,
                MaterialReceiveCountData = materialReceiveCounter,
                QualityCertificateCountData = qualityCertificateCounter,
                MaterialReceiveReportCountData = materialReceiveReportCounter,
                ItemReturnCountData = itemReturnApproval,
                StoreRequisitionFirstAppCountData = storeRequisitionFirstApproval,
                StoreRequisitionSecAppCountData = storeRequisitionSecApproval,
                ItemIssueCountData = itemIssue
            };
            return Json(datalist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadCompany(int? companyID)
        {
            _securityFactory = new SecurityFactorys();
            List<SET_Company> list = _securityFactory.SearchCompany(companyID);
            var companyList = list.Select(x => new { x.CompanyID, x.ContactNo, x.Name, x.RegistrationNo, x.Code, x.EmailID, x.Address, x.TaxNo, x.Status });
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchCompanyWiseBranch(int? companyID)
        {
            Result result = new Result();
            _securityFactory = new SecurityFactorys();
            _comBranchFactory = new CompanyBranchFactorys();
            List<SET_CompanyBranch> list = _comBranchFactory.SearchCompanyBranch(companyID);
            var companyList = list.Select(x => new { x.BranchID, x.Name });
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }
    }
}