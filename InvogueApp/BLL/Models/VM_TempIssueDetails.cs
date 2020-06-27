using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_TempIssueDetails
    {
       
        public int IssueID { get; set; }
        public int IssueDtlsID { get; set; }
        public int SRDtlsID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public Decimal IssueQty { get; set; }
        public string Remarks { get; set; }

    }
}