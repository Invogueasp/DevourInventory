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
    
    public partial class sp_PurchaseOdrList_Result
    {
        public int SPRID { get; set; }
        public int SPRDtlsID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int CategoryID1 { get; set; }
        public string CategoryName { get; set; }
        public int ProductID1 { get; set; }
        public string ProductName { get; set; }
        public int UnitID1 { get; set; }
        public string UnitName { get; set; }
        public int ReqQty { get; set; }
        public Nullable<int> ApprovedQty { get; set; }
    }
}