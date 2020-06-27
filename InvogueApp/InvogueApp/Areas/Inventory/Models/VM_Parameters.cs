using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_Parameters
    {
        public DateTime? FormDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DepartmentID { get; set; }
        public int? BranchID { get; set; }
        
    }
}