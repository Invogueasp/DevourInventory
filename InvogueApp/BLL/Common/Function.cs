using DAL.Common;
using DAL.db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Text;

namespace BLL.Common
{
    public class Function
    {
        DevourInvEntities db = new DevourInvEntities();
        public static String GetValidationErrorsDtls(DbEntityValidationException ex)
        {
            StringBuilder sb = new StringBuilder("BOQ ", 1000);
            foreach (var eve in ex.EntityValidationErrors)
            {
                sb.AppendLine("Entity has the following validation Error => ");
                foreach (var ve in eve.ValidationErrors)
                {
                    sb.AppendLine(ve.PropertyName + " Error : " + ve.ErrorMessage); 
                }
            }
            return sb.ToString();
        }

        //***************************** Start :: Common *****************************
        public bool CanView(string MenuCode)  
        {
            bool IsPermissionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] !=null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                if (objFormPermissionList != null && objFormPermissionList.Where(stringToCheck => stringToCheck.MenuCode.Contains(MenuCode)).FirstOrDefault() == null)
                {
                    IsPermissionFlag = false;
                }
            }
            return IsPermissionFlag;
        }

        public bool CanInsert(string sMenuCode)
        {
            bool IsActionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] != null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                DataTable dtPermission = new DataTable();
                string sql = "Select * From ObjecTPermission Where MenuCode = '" + sMenuCode + "' And UserGroupId = '" + HttpContext.Current.Session["UserGroupId"].ToString() + "'   ";
                dtPermission = DataManager.ExecuteQuery(sql);
                if (dtPermission.Rows.Count > 0)
                {
                    DataRow[] drs = dtPermission.Select("MenuCode='" + sMenuCode + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["PermissionNumber"].ToString().IndexOf("I") != -1)
                        {
                            return IsActionFlag = true;
                        }
                    }
                    return IsActionFlag = false;
                }
            }
            return IsActionFlag;
        } 
        public bool CanUpdate(string sMenuCode)
        {
            bool IsActionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] != null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                DataTable dtPermission = new DataTable();
                string sql = "Select * From ObjecTPermission Where MenuCode = '" + sMenuCode + "' And UserGroupId = '" + HttpContext.Current.Session["UserGroupId"].ToString() + "'   ";
                dtPermission = DataManager.ExecuteQuery(sql);
                if (dtPermission.Rows.Count > 0)
                {
                    DataRow[] drs = dtPermission.Select("MenuCode='" + sMenuCode + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["PermissionNumber"].ToString().IndexOf("U") != -1)
                        {
                            return IsActionFlag = true;
                        }
                    }
                    return IsActionFlag = false;
                }
            }
            return IsActionFlag;
        } 
        public bool CanDelete(string sMenuCode)
        {
            bool IsActionFlag = true;
            List<BLL.Common.HC_FormPermissionList> objFormPermissionList = HttpContext.Current.Session["FormPermissionList"] as List<BLL.Common.HC_FormPermissionList>;
            if (HttpContext.Current.Session["UserGroupHead"] != null && HttpContext.Current.Session["UserGroupHead"].ToString() == "Administrator")
            {

            }
            else
            {
                DataTable dtPermission = new DataTable();
                string sql = "Select * From ObjecTPermission Where MenuCode = '" + sMenuCode + "' And UserGroupId = '" + HttpContext.Current.Session["UserGroupId"].ToString() + "'   ";
                dtPermission = DataManager.ExecuteQuery(sql);
                if (dtPermission.Rows.Count > 0)
                {
                    DataRow[] drs = dtPermission.Select("MenuCode='" + sMenuCode + "'");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["PermissionNumber"].ToString().IndexOf("D") != -1)
                        {
                            return IsActionFlag = true;
                        }
                    }
                    return IsActionFlag = false;
                }
            }
            return IsActionFlag;
        }

             
        public enum Status
        {
            Active = 0,
            Inactive = 1
        }
        public static string GetPrevDate(string sDate)
        {
            string sSql = "Select dbo.FxDateStr(DateAdd(Month,-1,'" + sDate + "')) ";
            string VarNo = "";
            DataTable dt = DataManager.ExecuteQuery(sSql);
            if (dt.Rows.Count > 0)
            {
                VarNo = dt.Rows[0][0].ToString();

            }
            return VarNo;
        } 
        public static string getServerDateTime()
        {
            string sql = "Select Getdate()";
            DataTable dt = new DataTable();
            dt = DataManager.ExecuteQuery(sql);
            return dt.Rows[0][0].ToString();

        } 
        public static string GetMonthNameFromMonthNo(int MonNo)
        {
            string Tm = "";

            switch (MonNo)
            {
                case 1:
                    Tm = "Jan"; break;
                case 2:
                    Tm = "Feb"; break;
                case 3:
                    Tm = "Mar"; break;
                case 4:
                    Tm = "Apr"; break;
                case 5:
                    Tm = "May"; break;
                case 6:
                    Tm = "Jun"; break;
                case 7:
                    Tm = "Jul"; break;
                case 8:
                    Tm = "Aug"; break;
                case 9:
                    Tm = "Sep"; break;
                case 10:
                    Tm = "Oct"; break;
                case 11:
                    Tm = "Nov"; break;
                case 12:
                    Tm = "Dec"; break;
                default:
                    Tm = "";
                    break;
            }
            return Tm;
        }  
        public static string GetMaxRowID(string sSql)
        {
            string VarNo = "";

            DataTable dt = DataManager.ExecuteQuery(sSql);
            if (dt.Rows.Count > 0)
            {
                VarNo = dt.Rows[0][0].ToString();

            }
            return VarNo;
        } 
        public static string FormatDatedd_MMM_yyyy(DateTime Dt)
        {
            string BalMonth = "";
            string YearLength = Dt.Year.ToString();
            string MonthLength = GetMonthNameFromMonthNo(Dt.Month);
            if (Dt.Day.ToString().Length == 1)
            {
                BalMonth = "0" + Dt.Day.ToString() + "-" + MonthLength + "-" + YearLength;
            }
            else
                BalMonth = Dt.Day.ToString() + "-" + MonthLength + "-" + YearLength;
            {

            }
            return BalMonth;
        }

        //****************************** End :: Common ******************************
        

     
        
        //------------------------ Start :: Create DataTable ------------------------
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        //------------------------- End :: Create DataTable -------------------------


        //-------------------- Start :: Model Validation Function -------------------
        public static Dictionary<string, object> GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in modelState.Keys)
            {
                // Only send the errors to the client.
                if (modelState[key].Errors.Count > 0)
                {
                    errors[key] = modelState[key].Errors;
                }
            }

            return errors;
        }
        public static IList<SelectListItem> GetEnumSelectList(Type type)
        {
            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(type))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Enum.GetName(type, value);
                enums.Add(item);
            }
            return enums;
        }

        public static IEnumerable FillModelStateError(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Count() > 0);
            }
            return null;
        }

        #region Cookies Get Set
        public void SetCookies()
        {
            //string url = HttpContext.Current.Request.Url.AbsolutePath;
            //url = HttpContext.Current.Request.Url.AbsoluteUri;
            string url = HttpContext.Current.Request.Url.ToString();
            if (HttpContext.Current.Response.Cookies["latestVisit"] == null)
            {
                HttpCookie myCookie = new HttpCookie("latestVisit");
                myCookie.Expires = DateTime.Now.AddHours(2); // Last visit page keept in cookies till 2 hours.
                myCookie.Values["url"] = System.Web.HttpUtility.UrlEncode(url);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else
            {
                var myCookie = HttpContext.Current.Request.Cookies["latestVisit"];
                var cookieCollection = myCookie.Values;
                string[] CookieTitles = cookieCollection.AllKeys;

                //mj-y: If the url is reapeated, move it to end(means make it newer by removing it and adding it again)
                string cookieURL = "";
                foreach (string cookTit in CookieTitles)
                {
                    cookieURL = System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies["latestVisit"].Values[cookTit]);
                    if (cookieURL == url)
                    {
                        cookieCollection.Remove(cookTit);
                        cookieCollection.Set("url", System.Web.HttpUtility.UrlEncode(url));
                        HttpContext.Current.Response.SetCookie(myCookie);
                        return;
                    }
                }
                //mj-y: If it was not repeated ...
                cookieCollection.Set("url", System.Web.HttpUtility.UrlEncode(url));
                if (cookieCollection.Count > 15) // store just 15 item         
                    cookieCollection.Remove(CookieTitles[0]);
                HttpContext.Current.Response.SetCookie(myCookie);
            }
        }

        public string GetCookies()
        {
            string url = string.Empty;
            if (HttpContext.Current.Response.Cookies["latestVisit"] != null)
            {
                var myCookie = HttpContext.Current.Request.Cookies["latestVisit"];
                url = System.Web.HttpUtility.UrlDecode(myCookie.Values["url"]);
            }
            return url;
        }
        #endregion

        #region Reports
        public string CallReports(string DocType, List<ReportParameter> reportParameters, bool isPortrait, string reportPath, DataTable dataTable, IEnumerable<dynamic> dataObject, string reportDataSetName)
        {
            string errorMessage = string.Empty;
            string fileString = string.Empty;
            try
            {
                LocalReport lr = new LocalReport();
                ReportDataSource rd = new ReportDataSource();
                string path = string.Empty;
                if (dataTable !=null && dataTable.Rows.Count > 0 || dataObject !=null && dataObject.Count() > 0)
                {
                    path = reportPath;
                }
                else
                {
                    if (isPortrait)
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
                    }
                    else
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankLandscape.rdlc");
                    }
                }

                if (System.IO.File.Exists(path))
                {
                    lr.EnableExternalImages = true;
                    lr.ReportPath = path;
                    //Set Default Report Parameter
                    if (dataTable != null && dataTable.Rows.Count > 0 || dataObject != null && dataObject.Count() > 0)
                    {
                        var organization = db.OrganizationCores.FirstOrDefault();
                        if (organization != null)
                        {
                            string contactInfo = "Phone : " + organization.Phone + " Email : " + organization.Email + " Web : " + organization.Web;
                            reportParameters.Add(new ReportParameter("orgName", organization.OrganizationName));
                            reportParameters.Add(new ReportParameter("orgAddress", organization.OrganizationAddress));
                            reportParameters.Add(new ReportParameter("orgContactInfo", contactInfo));
                        }

                        // checking declared report parameters
                        ReportParameterInfoCollection ps;
                        ps = lr.GetParameters();
                        ReportParameter paramV = new ReportParameter();
                        foreach (ReportParameterInfo p in ps)
                        {
                            if (p.Name == "logo")
                            {
                                string logoPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/user_define/logo"), "r_logo.png");
                                if (System.IO.File.Exists(logoPath))
                                {                                    
                                    reportParameters.Add(new ReportParameter("logo", "File:\\" + logoPath, true));
                                }
                            }
                        }
                        

                        lr.SetParameters(reportParameters);
                    }                   
                }
                else
                {
                    errorMessage = "File Not Exists.";
                }               
                if (dataTable != null)
                {
                    rd = new ReportDataSource(reportDataSetName, dataTable);
                }
                else if(dataObject != null)
                {
                    rd = new ReportDataSource(reportDataSetName, dataObject);
                }
               
                lr.DataSources.Add(rd);
                // Convert Report To Base64String
                string reportType = DocType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + DocType + "</OutputFormat>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileString = Convert.ToBase64String(renderedBytes.ToArray());
                return fileString;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return errorMessage;
            }            
        }

        public string CallReportsMultipleDS(string DocType, List<ReportParameter> reportParameters, bool isPortrait, string reportPath, IEnumerable<dynamic> dataObject1, IEnumerable<dynamic> dataObject2, string reportDS1, string reportDS2)
        {
            string errorMessage = string.Empty;
            string fileString = string.Empty;
            try
            {
                LocalReport lr = new LocalReport();
                ReportDataSource rd = new ReportDataSource();
                lr.EnableExternalImages = true;
                string path = string.Empty;
                if (dataObject1 != null && dataObject1.Count() > 0 || dataObject2 != null && dataObject2.Count() > 0)
                {
                    path = reportPath;
                }
                else
                {
                    if (isPortrait)
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
                    }
                    else
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankLandscape.rdlc");
                    }
                }

                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                    //Set Default Report Parameter
                    if (dataObject1 != null && dataObject1.Count() > 0 || dataObject2 != null && dataObject2.Count() > 0)
                    {
                        var organization = db.OrganizationCores.FirstOrDefault();
                        if (organization != null)
                        {
                            string contactInfo = "Phone : " + organization.Phone + " Email : " + organization.Email + " Web : " + organization.Web;
                            reportParameters.Add(new ReportParameter("orgName", organization.OrganizationName));
                            reportParameters.Add(new ReportParameter("orgAddress", organization.OrganizationAddress));
                            reportParameters.Add(new ReportParameter("orgContactInfo", contactInfo));
                        }

                        // checking declared report parameters
                        ReportParameterInfoCollection ps;
                        ps = lr.GetParameters();
                        ReportParameter paramV = new ReportParameter();
                        foreach (ReportParameterInfo p in ps)
                        {
                            if (p.Name == "logo")
                            {
                                string logoPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/user_define/logo"), "r_logo.png");
                                if (System.IO.File.Exists(logoPath))
                                {
                                    reportParameters.Add(new ReportParameter("logo", "File:\\" + logoPath, true));
                                }
                            }
                        }

                        lr.SetParameters(reportParameters);
                    }
                }
                else
                {
                    errorMessage = "File Not Exists.";
                }
                //if (dataObjectAsset != null)
                //{
                //    rd = new ReportDataSource(reportDSAsset, dataObjectAsset);                   
                //}
                //if (dataObjectLiabilities != null)
                //{
                //    rd = new ReportDataSource(reportDSLiabilites, dataObjectLiabilities);
                //}
                lr.DataSources.Clear();
                ReportDataSource rd1 = new ReportDataSource(reportDS1, dataObject1);
                ReportDataSource rd2 = new ReportDataSource(reportDS2, dataObject2);
                lr.DataSources.Add(rd1);
                lr.DataSources.Add(rd2);

                // Convert Report To Base64String
                string reportType = DocType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + DocType + "</OutputFormat>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileString = Convert.ToBase64String(renderedBytes.ToArray());
                return fileString;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return errorMessage;
            }
        }


        //use multiple dataset and data table  for report
        public string CallReportsForMultipleDataSet(string DocType, List<ReportParameter> reportParameters, bool isPortrait, string reportPath, DataTable dataTable1, DataTable dataTable2, DataTable dataTable3, DataTable dataTable4, DataTable dataTable5, DataTable dataTable6, string reportDataSetName1, string reportDataSetName2, string reportDataSetName3, string reportDataSetName4, string reportDataSetName5, string reportDataSetName6)
        {
            string errorMessage = string.Empty;
            string fileString = string.Empty;
            try
            {
                LocalReport lr = new LocalReport();
                ReportDataSource rd1 = new ReportDataSource();
                ReportDataSource rd2 = new ReportDataSource();
                ReportDataSource rd3 = new ReportDataSource();
                ReportDataSource rd4 = new ReportDataSource();
                ReportDataSource rd5 = new ReportDataSource();
                ReportDataSource rd6 = new ReportDataSource();
                string path = string.Empty;
                if (dataTable1 != null && dataTable1.Rows.Count > 0 || dataTable2 != null && dataTable2.Rows.Count > 0 || dataTable3 != null && dataTable3.Rows.Count > 0 || dataTable4 != null && dataTable4.Rows.Count > 0 || dataTable5 != null && dataTable5.Rows.Count > 0 || dataTable6 != null && dataTable6.Rows.Count > 0)
                {
                    path = reportPath;
                }
                else
                {
                    if (isPortrait)
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankPortrait.rdlc");
                    }
                    else
                    {
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/Reports/RDLC"), "BlankLandscape.rdlc");
                    }
                }

                if (System.IO.File.Exists(path))
                {
                    lr.EnableExternalImages = true;
                    lr.ReportPath = path;
                    //Set Default Report Parameter
                    if (dataTable1 != null && dataTable1.Rows.Count > 0 || dataTable2 != null && dataTable2.Rows.Count > 0 || dataTable3 != null && dataTable3.Rows.Count > 0 || dataTable4 != null && dataTable4.Rows.Count > 0 || dataTable5 != null && dataTable5.Rows.Count > 0 || dataTable6 != null && dataTable6.Rows.Count > 0)
                    {
                        var organization = db.OrganizationCores.FirstOrDefault();
                        if (organization != null)
                        {
                            string contactInfo = "Phone : " + organization.Phone + " Email : " + organization.Email + " Web : " + organization.Web;
                            reportParameters.Add(new ReportParameter("orgName", organization.OrganizationName));
                            reportParameters.Add(new ReportParameter("orgAddress", organization.OrganizationAddress));
                            reportParameters.Add(new ReportParameter("orgContactInfo", contactInfo));
                        }

                        // checking declared report parameters
                        ReportParameterInfoCollection ps;
                        ps = lr.GetParameters();
                        ReportParameter paramV = new ReportParameter();
                        foreach (ReportParameterInfo p in ps)
                        {
                            if (p.Name == "logo")
                            {
                                string logoPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/user_define/logo"), "r_logo.png");
                                if (System.IO.File.Exists(logoPath))
                                {
                                    reportParameters.Add(new ReportParameter("logo", "File:\\" + logoPath, true));
                                }
                            }
                        }


                        lr.SetParameters(reportParameters);
                    }
                }
                else
                {
                    errorMessage = "File Not Exists.";
                }
                if (dataTable1 != null && dataTable2 != null && dataTable3 != null && dataTable4 != null && dataTable5 != null && dataTable6 != null)
                {
                    rd1 = new ReportDataSource(reportDataSetName1, dataTable1);
                    rd2 = new ReportDataSource(reportDataSetName2, dataTable2);
                    rd3 = new ReportDataSource(reportDataSetName3, dataTable3);
                    rd4 = new ReportDataSource(reportDataSetName4, dataTable4);
                    rd5 = new ReportDataSource(reportDataSetName5, dataTable5);
                    rd6 = new ReportDataSource(reportDataSetName6, dataTable6);
                }
                //else if (dataObject != null)
                //{
                //    rd = new ReportDataSource(reportDataSetName, dataObject);
                //}

                lr.DataSources.Add(rd1);
                lr.DataSources.Add(rd2);
                lr.DataSources.Add(rd3);
                lr.DataSources.Add(rd4);
                lr.DataSources.Add(rd5);
                lr.DataSources.Add(rd6);
                // Convert Report To Base64String
                string reportType = DocType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + DocType + "</OutputFormat>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                fileString = Convert.ToBase64String(renderedBytes.ToArray());
                return fileString;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return errorMessage;
            }
        }

        #endregion

    }
}
        
    
