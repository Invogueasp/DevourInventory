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
    
    public partial class sp_MRRSDtlsList_Result
    {
        public Nullable<int> MaterialReceiveID { get; set; }
        public Nullable<int> ReceiveDtlsID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public Nullable<decimal> ReceiveQty { get; set; }
        public string Code { get; set; }
        public string PartNumber { get; set; }
    }
}
