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
    
    public partial class INV_OpeningStock
    {
        public int OpeningStockID { get; set; }
        public int BranchID { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int UnitID { get; set; }
        public Nullable<int> SizeID { get; set; }
        public decimal Quantity { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual SET_CompanyBranch SET_CompanyBranch { get; set; }
        public virtual INV_Product INV_Product { get; set; }
    }
}
