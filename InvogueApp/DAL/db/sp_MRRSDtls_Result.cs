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
    
    public partial class sp_MRRSDtls_Result
    {
        public int POID { get; set; }
        public int PODtlsID { get; set; }
        public int ReceiveDtlsID { get; set; }
        public int MaterialReceiveID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal MRSReceiveQty { get; set; }
        public int ReceiveQty { get; set; }
        public int ReceiveQtys { get; set; }
    }
}
