using BLL.Common;
using BLL.Models;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Commercial
{
    public interface IPurchaseOrderFactory
    {
        Result SavePurchaseOrder(INV_PO pOrder, List<INV_PODtls> pOrderDtls, List<int> deletepOrderDtlsID,int? check);
        List<INV_PO> SearchPurchaseOrder(int? pOID);

        List<INV_PODtls> SearchPurchaseOrderDtls(int? pOID);
    }

    public interface IMRRFactory
    {
        Result SaveMRR(INV_MRR mRr, List<VM_TempMrrDtlsList> mRrDtls);
        List<INV_MRR> SearchMRR(int? mRRID);
       // List<INV_MRRDtls> SearchMRRDtls(int? pOID);
    }

    public interface IMRRHeadOfficeFactory
    {
        Result SaveMRRHeadOffice(INV_MRR_HO mRr, List<INV_MRR_HODtls> mRrDtls, List<int> deletepDtlsID);
        List<INV_MRR_HO> SearchMRRHeadOffice(int? mRRID);
    }
    public interface IMRSFactory
    {
        Result SaveMRS(INV_MaterialReceive mRr, List<INV_MaterialReceiveDtls> mRrDtls, List<int> deletepDtlsID);
        List<INV_MaterialReceive> SearchMRS(int? mRRID);

        List<INV_MaterialReceiveDtls> SearchMRSDtls(int? mRRdtlsID);
       
    }
    public interface IQCFactory
    {
        Result SaveQC(INV_QC mRr, List<INV_QCDtls> mRrDtls, List<int> deletepDtlsID);
        List<INV_QC> SearchQC(int? qcID);

    }
}
