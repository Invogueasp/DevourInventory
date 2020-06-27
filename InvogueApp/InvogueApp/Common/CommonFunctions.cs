using DAL.db;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Application.Common
{
    public class CommonFunctions
    {
        DevourInvEntities db = new DevourInvEntities();
        public string CallReports(string DocType, List<ReportParameter> reportParameters, bool isPortrait, string reportPath, DataTable dataTable, IEnumerable<dynamic> dataObject, string reportDataSetName, int branchID, int? loginUserID)
        {
            string errorMessage = string.Empty;
            string fileString = string.Empty;
            try
            {
                //var org = db.OrganizationCores.FirstOrDefault();

                //ReportParameter companyName = new ReportParameter("companyName");
                //companyName.Values.Add(org.OrganizationName);
                //reportParameters.Add(companyName);

                //ReportParameter address = new ReportParameter("address");
                //address.Values.Add(org.OrganizationAddress);
                //reportParameters.Add(address);

                //ReportParameter contactInfo = new ReportParameter("contactInfo");
                //contactInfo.Values.Add("PHONE :" + org.Phone + " Email : " + org.Email + " WEB : " + org.Web);
                //reportParameters.Add(contactInfo);

                LocalReport lr = new LocalReport();
                ReportDataSource rd = new ReportDataSource();
                string path = string.Empty;
                if (dataTable != null && dataTable.Rows.Count > 0 || dataObject != null && dataObject.Count() > 0)
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
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/AttendanceReports/RDLCS"), "DailyAttendance.rdlc");
                    }
                }

                if (System.IO.File.Exists(path))
                {
                    lr.EnableExternalImages = true;
                    lr.ReportPath = path;
                    //Set Default Report Parameter
                    if (dataTable != null && dataTable.Rows.Count > 0 || dataObject != null && dataObject.Count() > 0)
                    {
                        var organization = db.SET_CompanyBranch.Where(x => x.BranchID == branchID).FirstOrDefault();

                        var userID = db.SEC_UserInformation.Where(x => x.ID == loginUserID).FirstOrDefault();
                        

                        if (loginUserID > 0)
                        {
                            var deptName = db.INV_Department.Where(x => x.DepartmentID == userID.DepartmentID).FirstOrDefault();
                            reportParameters.Add(new ReportParameter("departmentName", deptName.Name));
                            reportParameters.Add(new ReportParameter("UserFullName", userID.UserFullName));
                        }

                        if (organization != null)
                        {
                            string contactInfo = "Phone : " + organization.ContactNO + " Email : " + organization.Email ;
                        
                  
                            reportParameters.Add(new ReportParameter("orgName", organization.Name));
                            reportParameters.Add(new ReportParameter("orgAddress", organization.Address));
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
                                string logoPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/img"), "TECHNO.png");
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
                else if (dataObject != null)
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
        //public string CallReports(string DocType, List<ReportParameter> reportParameters, bool isPortrait, string reportPath, DataTable dataTable, IEnumerable<dynamic> dataObject, string reportDataSetName)
        //{
        //    string errorMessage = string.Empty;
        //    string fileString = string.Empty;
        //    try
        //    {
        //        var org = db.OrganizationCores.FirstOrDefault();

        //                ReportParameter companyName = new ReportParameter("companyName");
        //        companyName.Values.Add(org.OrganizationName);
        //        reportParameters.Add(companyName);

        //        ReportParameter address = new ReportParameter("address");
        //        address.Values.Add(org.OrganizationAddress);
        //        reportParameters.Add(address);

        //        ReportParameter contactInfo = new ReportParameter("contactInfo");
        //        contactInfo.Values.Add("PHONE :" + org.Phone + " Email : " + org.Email + " WEB : " + org.Web);
        //        reportParameters.Add(contactInfo);

        //        LocalReport lr = new LocalReport();
        //        ReportDataSource rd = new ReportDataSource();
        //        string path = string.Empty;
        //        if (dataTable != null && dataTable.Rows.Count > 0 || dataObject != null && dataObject.Count() > 0)
        //        {
        //            path = reportPath;
        //        }
        //        else
        //        {
        //            if (isPortrait)
        //            {
        //                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/AttendanceReports/RDLCS"), "DailyAttendance.rdlc");
        //            }
        //            else
        //            {
        //                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/AttendanceReports/RDLCS"), "DailyAttendance.rdlc");
        //            }
        //        }

        //        if (System.IO.File.Exists(path))
        //        {
        //            lr.ReportPath = path;
        //            lr.SetParameters(reportParameters);

        //        }
        //        else
        //        {
        //            errorMessage = "File Not Exists.";
        //        }
        //        if (dataTable != null)
        //        {
        //            rd = new ReportDataSource(reportDataSetName, dataTable);

        //        }
        //        else if (dataObject != null)
        //        {
        //            rd = new ReportDataSource(reportDataSetName, dataObject);
        //        }

        //        lr.DataSources.Add(rd);
        //        // Convert Report To Base64String
        //        string reportType = DocType;
        //        string mimeType;
        //        string encoding;
        //        string fileNameExtension;
        //        string deviceInfo =
        //        "<DeviceInfo>" +
        //        "  <OutputFormat>" + DocType + "</OutputFormat>" +
        //        "</DeviceInfo>";

        //        Warning[] warnings;
        //        string[] streams;
        //        byte[] renderedBytes;

        //        renderedBytes = lr.Render(
        //            reportType,
        //            deviceInfo,
        //            out mimeType,
        //            out encoding,
        //            out fileNameExtension,
        //            out streams,
        //            out warnings);
        //        fileString = Convert.ToBase64String(renderedBytes.ToArray());
        //        return fileString;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = ex.Message;
        //        return errorMessage;
        //    }
        //}
    
    }

    public class Parameters
    {
        public int? EmpID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public int MonthID { get; set; }
        public string MonthName { get; set; }

    }
    static class Constants
    {
        public const string Token = "ZURINV19";
    }
}