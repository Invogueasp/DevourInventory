using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvogueApp.Areas.Settings.Models
{
    public class VM_ProductModel
    {
        public List<INV_ProductDepartment> ProductDept { get; set; }
        public INV_Product Product { get; set; }

        public List<int> editProductdeptID { get; set; }
       
    }

    public class VM_MrrHoModel
    {
        public List<INV_MRR_HODtls> mRrDtls { get; set; }
        public INV_MRR_HO mRr { get; set; }

        public List<int> deletepDtlsID { get; set; }

    }

}