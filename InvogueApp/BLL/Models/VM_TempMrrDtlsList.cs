using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class VM_TempMrrDtlsList
    {
        public int MRRID { get; set; }
        public int MRRDtlsID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public int ReceiveQty { get; set; }
        public Decimal UnitRate { get; set; }
        public Decimal Discount { get; set; }
        public Decimal LineTotal { get; set; }
        public int PODtlsID { get; set; }

    }
}
