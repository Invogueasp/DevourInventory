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
    
    public partial class INV_ScrapReturnDtls
    {
        public int ScrapReturnDtlsID { get; set; }
        public int ScrapReturnID { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int UnitID { get; set; }
        public decimal Quantity { get; set; }
        public string Location { get; set; }
        public Nullable<System.DateTime> DisposalDate { get; set; }
        public string DisposalRef { get; set; }
        public Nullable<decimal> DisposalAmount { get; set; }
        public string Remarks { get; set; }
    
        public virtual INV_Category INV_Category { get; set; }
        public virtual INV_Product INV_Product { get; set; }
        public virtual INV_ScrapReturn INV_ScrapReturn { get; set; }
        public virtual INV_Unit INV_Unit { get; set; }
    }
}
