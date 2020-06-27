using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_IssueDtls
    {
        public int? IssueDtlsID { get; set; }
        public int? IssueID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int SRDtlsID { get; set; }
        public int SRID { get; set; }
        public int UnitID { get; set; }
        public int ApprovedQty { get; set; }
        public int? IssueQty { get; set; }
        public string Remarks { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public int ReqQty { get; set; }
        public DateTime IssueDate { get; set; }

        public string IssueNO { get; set; }
    }
}