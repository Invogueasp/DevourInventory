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
    
    public partial class INV_MRR_HODtls
    {
        public int MRRDtlsID { get; set; }
        public int MRRID { get; set; }
        public int PODtlsID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int ReceiveQty { get; set; }
        public decimal UnitRate { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public decimal LineTotal { get; set; }
    
        public virtual INV_Category INV_Category { get; set; }
        public virtual INV_MRR_HO INV_MRR_HO { get; set; }
        public virtual INV_Product INV_Product { get; set; }
        public virtual INV_Unit INV_Unit { get; set; }
        public virtual INV_PODtls INV_PODtls { get; set; }
    }
}