using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_MrrDtlsList
    {
        public int POID { get; set; }
        public int PODtlsID { get; set; }
        public int MRRDtlsID { get; set; }
        public int MRRID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int ApprovedQty { get; set; }
        public int SentQty { get; set; }
        public int ReceiveQty { get; set; }
        public int ReceiveQtys { get; set; }
       // public string Remarks { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Code { get; set; }
        public string PartNumber { get; set; }
        public Decimal UnitRate { get; set; }
      //  public DateTime IssueDate { get; set; }

        public Decimal LineTotal { get; set; }
        public int BranchID { get; set; }
    }
    public class VM_MRSDtlsList
    {
        public int POID { get; set; }
        public int PODtlsID { get; set; }
        public int ReceiveDtlsID { get; set; }
        public int MaterialReceiveID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int ReceiveQty { get; set; }
        public int ReceiveQtys { get; set; }
        public decimal MRSReceiveQty { get; set; }
        
        // public string Remarks { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        //public int BranchID { get; set; }
    }

    public class VM_MRRStoreDtlsList
    {
        public int MRRID { get; set; }
        public int POID { get; set; }
        public int PODtlsID { get; set; }
        public int? ReceiveDtlsID { get; set; }
        public int? MaterialReceiveID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int SentQty { get; set; }
        public decimal? ReceiveQty { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Code { get; set; }
        public string PartNumber { get; set; }
    }

    public class VM_QCDtlsList
    {
        public int QCID { get; set; }
        public int POID { get; set; }
        public int QCDtlsID { get; set; }
        public int PODtlsID { get; set; }
        public int MaterialReceiveID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public decimal ReceiveQty { get; set; }
        public decimal MRSReceiveQty { get; set; }
        public decimal QCFailQty { get; set; }
        public decimal QCPassQty { get; set; }

        // public string Remarks { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Code { get; set; }
        public string PartNumber { get; set; }
        public string Remarks { get; set; }
        //public int BranchID { get; set; }
    }

    public class VM_MRSFrReturnDtlsList
    {
        public int POID { get; set; }
        public int PODtlsID { get; set; }
        public int ReceiveDtlsID { get; set; }
        public int MaterialReceiveID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public decimal ReceiveQty { get; set; }
        public decimal MRSReceiveQty { get; set; }

        // public string Remarks { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        //public int BranchID { get; set; }
    }
}