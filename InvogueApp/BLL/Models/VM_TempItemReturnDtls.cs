using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
   public class VM_TempItemReturnDtls
    {
        public int ReturnID { get; set; }
        public int ReturnDtlsID { get; set; }
        public int TransferDtlsID { get; set; }
        public int PODtlsID { get; set; }
        public int IssueDtlsID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public Decimal ReturnQty { get; set; }
        public int ProductType { get; set; }
        public string ApprovalRemarks { get; set; }
        public string Remarks { get; set; }
       
    }
}
