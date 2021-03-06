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
    
    public partial class SEC_UserInformation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SEC_UserInformation()
        {
            this.INV_Issue = new HashSet<INV_Issue>();
            this.INV_MaterialReceive = new HashSet<INV_MaterialReceive>();
            this.INV_MRR = new HashSet<INV_MRR>();
            this.INV_MRR_HO = new HashSet<INV_MRR_HO>();
            this.INV_PO = new HashSet<INV_PO>();
            this.INV_SPR = new HashSet<INV_SPR>();
            this.INV_SR = new HashSet<INV_SR>();
            this.SEC_Password = new HashSet<SEC_Password>();
            this.SEC_UserActionMapping = new HashSet<SEC_UserActionMapping>();
            this.SEC_UserGroup = new HashSet<SEC_UserGroup>();
        }
    
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int DepartmentID { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public System.Guid PasswordID { get; set; }
        public System.Guid SecurityQuestionID { get; set; }
        public Nullable<bool> IsEMailVerified { get; set; }
        public Nullable<bool> IsPhoneNoVerified { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> UserGroupID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_Issue> INV_Issue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_MaterialReceive> INV_MaterialReceive { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_MRR> INV_MRR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_MRR_HO> INV_MRR_HO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_PO> INV_PO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_SPR> INV_SPR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INV_SR> INV_SR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SEC_Password> SEC_Password { get; set; }
        public virtual SEC_Password SEC_Password1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SEC_UserActionMapping> SEC_UserActionMapping { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SEC_UserGroup> SEC_UserGroup { get; set; }
        public virtual SEC_UserGroup SEC_UserGroup1 { get; set; }
    }
}
