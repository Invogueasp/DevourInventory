using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_ItemReturnDtlsForSTW
    {
        public int IssueID { get; set; }
        public int IssueDtlsID { get; set; }
        public int ReturnDtlsID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int ReturnQty { get; set; }
        public int ProductType { get; set; }
        public int UnitID { get; set; }
        public int IssueQty { get; set; }
        public string Remarks { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
    }
}