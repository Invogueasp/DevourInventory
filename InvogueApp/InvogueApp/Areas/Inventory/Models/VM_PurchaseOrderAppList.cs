using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_PurchaseOrderAppList
    {
        public int SPRID { get; set; }
        public int POID { get; set; }
        public int TotalAmount { get; set; }
        public int SupplierID { get; set; }
        public DateTime PODate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PONO { get; set; }
        public string SupplierName { get; set; }
        public string FirstApproveStatus { get; set; }
        public string SecondApproveStatus { get; set; }
        public DateTime ? FirstApproveDate { get; set; }
        public DateTime ? SecondApproveDate { get; set; }

        public int FirstApproveBy { get; set; }
        public int SecondApproveBy { get; set; }
        public int? BranchID { get; set; }
        public int? DepartmentID { get; set; }
        public string BranchName { get; set; }
        public string Department { get; set; }
    }
}