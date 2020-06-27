using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Reports.Models
{
    public class VM_SRandIssueDtls
    {
        public int ApprovedQty { get; set; }
        public int? IssueQty { get; set; }
        public string BranchName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public int ReqQty { get; set; }
        public string Code { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string SRNO { get; set; }
    }
    public class VM_SearchSRNO
    {
        public int SRID { get; set; }
        public string SRNO { get; set; }
    }
}