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
    
    public partial class INV_MRR_HO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INV_MRR_HO()
        {
            this.INV_MRR_HODtls = new HashSet<INV_MRR_HODtls>();
        }
    
        public int MRRID { get; set; }
        public int POID { get; set; }
        public string MRRNO { get; set; }
        public System.DateTime MRRDate { get; set; }
        public string SupplierInv { get; set; }
        public int SupplierID { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> TotalDiscount { get; set; }
        public Nullable<decimal> TotalVat { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Status { get; set; }
    
        public virtual SEC_UserInformation SEC_UserInformation { get; set; }
        public virtual INV_PO INV_PO { get; set; }
        public virtual INV_Supplier INV_Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_MRR_HODtls> INV_MRR_HODtls { get; set; }
    }
}