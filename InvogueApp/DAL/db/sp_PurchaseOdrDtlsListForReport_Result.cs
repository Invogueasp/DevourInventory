//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.db
{
    using System;
    
    public partial class sp_PurchaseOdrDtlsListForReport_Result
    {
        public string Supplier { get; set; }
        public string SPRNO { get; set; }
        public Nullable<System.DateTime> SPRDate { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string PartNumber { get; set; }
        public string UnitName { get; set; }
        public Nullable<int> ReqQty { get; set; }
        public Nullable<int> ApprovedQty { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public Nullable<int> OrderAppQty { get; set; }
        public string SPRRemarks { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> LineTotal { get; set; }
        public string PONO { get; set; }
        public System.DateTime PODate { get; set; }
        public System.DateTime DueDate { get; set; }
    }
}
