using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_Stock
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int BranchID { get; set; }
        public int TotalStock { get; set; }
        public string Category { get; set; }
        public string Product { get; set; }
        public string Unit { get; set; }
        public string Code { get; set; }
        public string PartNumber { get; set; }
        public decimal? Quantity { get; set; }
        //public decimal? Spinining_ST { get; set; }
        //public decimal? Textile_ST { get; set; }
        //public decimal? Spinining_WH { get; set; }
        //public decimal? Textile_WH { get; set; }
    }
}