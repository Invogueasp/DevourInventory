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
    
    public partial class sp_MrrDtls_Report_Result
    {
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Code { get; set; }
        public string SPRNO { get; set; }
        public string SupplierInv { get; set; }
        public System.DateTime MRRDate { get; set; }
        public string PONO { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Address { get; set; }
        public string SupplierName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitRate { get; set; }
        public decimal LineTotal { get; set; }
    }
}
