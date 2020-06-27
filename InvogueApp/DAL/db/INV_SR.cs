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
    
    public partial class INV_SR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INV_SR()
        {
            this.INV_Issue = new HashSet<INV_Issue>();
            this.INV_SRDtls = new HashSet<INV_SRDtls>();
        }
    
        public int SRID { get; set; }
        public string SRNO { get; set; }
        public int BranchID { get; set; }
        public System.DateTime RequisitionDate { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_Issue> INV_Issue { get; set; }
        public virtual SET_CompanyBranch SET_CompanyBranch { get; set; }
        public virtual SEC_UserInformation SEC_UserInformation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_SRDtls> INV_SRDtls { get; set; }
    }
}