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
    
    public partial class INV_ProductDepartment
    {
        public int ProductDeptID { get; set; }
        public int ProductID { get; set; }
        public int DepartmentID { get; set; }
    
        public virtual INV_Department INV_Department { get; set; }
        public virtual INV_Product INV_Product { get; set; }
    }
}
