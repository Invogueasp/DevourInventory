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
    
    public partial class sp_MrrDtlsList_Result
    {
        public int POID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public int PODtlsID { get; set; }
        public int MRRDtlsID { get; set; }
        public int MRRID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public Nullable<int> ApprovedQty { get; set; }
        public int ReceiveQty { get; set; }
        public decimal UnitRate { get; set; }
        public decimal LineTotal { get; set; }
    }
}
