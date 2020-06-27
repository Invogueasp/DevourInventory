using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Inventory.Models
{
    public class VM_PurchaseOdrList
    {

        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int PODtlsID { get; set; }

        public int SPRID { get; set; }
        public int SPRDtlsID { get; set; }
        public Decimal UnitRate { get; set; }
        public Decimal LineTotal { get; set; }
        public int ApprovedQty { get; set; }
        public int OrderQty { get; set; }
        public int ReqQty { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }



    }
    public class VM_PendingPurchaseOdrList
    {

        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int PODtlsID { get; set; }

        public int SPRID { get; set; }
        public int SPRDtlsID { get; set; }
        public Decimal UnitRate { get; set; }
        public Decimal LineTotal { get; set; }
        public int ApprovedQtys { get; set; }
        public int OrderQty { get; set; }
        public int ReqQty { get; set; }
        public int ReaminQty { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }



    }
    public class VM_PendingSprList
    {
        public int SPRID { get; set; }
        public string SPRNO { get; set; }

      
    }
}