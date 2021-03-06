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
    using System.Collections.Generic;
    
    public partial class INV_ReturnDtls
    {
        public int ReturnDtlsID { get; set; }
        public int ReturnID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int ReturnQty { get; set; }
        public int ProductType { get; set; }
        public string Remarks { get; set; }
        public string ApprovalRemarks { get; set; }
        public Nullable<int> TransferDtlsID { get; set; }
        public Nullable<int> PODtlsID { get; set; }
        public Nullable<int> IssueDtlsID { get; set; }
    
        public virtual INV_Category INV_Category { get; set; }
        public virtual INV_Product INV_Product { get; set; }
        public virtual INV_Return INV_Return { get; set; }
        public virtual INV_ReturnDtls INV_ReturnDtls1 { get; set; }
        public virtual INV_ReturnDtls INV_ReturnDtls2 { get; set; }
        public virtual INV_Unit INV_Unit { get; set; }
    }
}
