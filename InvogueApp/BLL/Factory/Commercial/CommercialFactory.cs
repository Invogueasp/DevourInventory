using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Commercial;
using BLL.Models;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory.Commercial
{
    public class POrderFactory : GenericFactory<DevourInvEntities, INV_PO>
    {

    }
    public class POrderDtlsFactory : GenericFactory<DevourInvEntities, INV_PODtls>
    {

    }
    public class MRSFactory : GenericFactory<DevourInvEntities, INV_MaterialReceive>
    {

    }
    public class MRSDtlsFactory : GenericFactory<DevourInvEntities, INV_MaterialReceiveDtls>
    {

    }
    public class MRRFactory : GenericFactory<DevourInvEntities, INV_MRR>
    {

    }
    public class MRRHeadOfficeFactory : GenericFactory<DevourInvEntities, INV_MRR_HO>
    {

    }
    public class MRRDtlsFactory : GenericFactory<DevourInvEntities, INV_MRRDtls>
    {

    }
    public class MRRDtlsHeadOfficeFactory : GenericFactory<DevourInvEntities, INV_MRR_HODtls>
    {

    }

    public class QCFactory : GenericFactory<DevourInvEntities, INV_QC>
    {

    }
    public class QCDtlsFactory : GenericFactory<DevourInvEntities, INV_QCDtls>
    {

    }
    public class POrderFactorys : IPurchaseOrderFactory
    {
        private IGenericFactory<INV_PO> _poFactory;
        private IGenericFactory<INV_PODtls> _poDtlsFactory;

        Result _result = new Result();


        public Result SavePurchaseOrder(INV_PO pOrder, List<INV_PODtls> pOrderDtls, List<int> deletepOrderDtlsID,int? check)
        {
            _poFactory = new POrderFactory();
            _poDtlsFactory = new POrderDtlsFactory();


            try
            {
                if (pOrder.POID > 0)
                {

                    _poFactory.Edit(pOrder);
                    _result = _poFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deletepOrderDtlsID != null)
                        {
                            foreach (var detailsID in deletepOrderDtlsID)
                            {
                                _poDtlsFactory.Delete(x => x.PODtlsID == detailsID);
                                _result = _poDtlsFactory.Save();
                            }
                        }

                        if (pOrderDtls != null)
                        {
                            foreach (var dtls in pOrderDtls)
                            {
                                if (dtls.PODtlsID < 1)
                                {

                                    int pODtlsID = 1;
                                    var prvpODtlsID = _poDtlsFactory.GetLastRecord().OrderByDescending(x => x.PODtlsID).FirstOrDefault();

                                    if (prvpODtlsID != null)
                                    {
                                        pODtlsID = prvpODtlsID.PODtlsID + 1;
                                    }

                                    dtls.PODtlsID = pODtlsID;
                                    dtls.POID = pOrder.POID;
                                    _poDtlsFactory.Add(dtls);
                                    _result = _poDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        DevourInvEntities db = new DevourInvEntities();
                                        if (check>0)
                                        {
                                            var appData = db.INV_SPRDtls.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
                                            appData.OrderQty = appData.OrderQty + dtls.OrderQty;
                                            db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            var appData = db.INV_SPRDtls.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
                                            appData.OrderQty = dtls.OrderQty;
                                            db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                       

                                        if (pOrder.FirstApproveStatus == "A")
                                        {
                                            string tableName = "Purchase Order Approval";
                                            _result.message = _result.UpdateSuccessfull(tableName);
                                        }
                                        else
                                        {
                                            string tableName = "Purchase Order";
                                            _result.message = _result.UpdateSuccessfull(tableName);
                                        }


                                    }

                                }
                                else
                                {
                                    dtls.POID = pOrder.POID;
                                    _poDtlsFactory.Edit(dtls);
                                    _result = _poDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        DevourInvEntities db = new DevourInvEntities();
                                        if(check>0){
                                            var appData = db.INV_SPRDtls.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
                                            appData.OrderQty = appData.OrderQty + dtls.OrderQty;
                                            db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            var appData = db.INV_SPRDtls.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
                                            appData.OrderQty = dtls.OrderQty;
                                            db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    

                                        if (pOrder.FirstApproveStatus == "A")
                                        {
                                            string tableName = "Purchase Order Approval";
                                            _result.message = _result.UpdateSuccessfull(tableName);
                                        }
                                        else
                                        {
                                            string tableName = "Purchase Order";
                                            _result.message = _result.UpdateSuccessfull(tableName);
                                        }

                                    }
                                }
                            }

                        }

                    }

                }
                else
                {
                    int pOID = 1;
                    var prvpOID = _poFactory.GetLastRecord().OrderByDescending(x => x.POID).FirstOrDefault();

                    if (prvpOID != null)
                    {
                        pOID = prvpOID.POID + 1;
                    }

                    pOrder.POID = pOID;
                    _poFactory.Add(pOrder);
                    _result = _poFactory.Save();

                    if (_result.isSucess)
                    {
                        foreach (var dtls in pOrderDtls)
                        {
                            if (dtls.PODtlsID < 1)
                            {

                                int pODtlsID = 1;
                                var prvpODtlsID = _poDtlsFactory.GetLastRecord().OrderByDescending(x => x.PODtlsID).FirstOrDefault();

                                if (prvpODtlsID != null)
                                {
                                    pODtlsID = prvpODtlsID.PODtlsID + 1;
                                }

                                dtls.PODtlsID = pODtlsID;
                                dtls.POID = pOrder.POID;
                                _poDtlsFactory.Add(dtls);
                                _result = _poDtlsFactory.Save();


                            }
                            if (_result.isSucess)
                            {
                                if(check>0){
                                DevourInvEntities db = new DevourInvEntities();
                                var appData = db.INV_SPRDtls.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
                                appData.OrderQty = appData.OrderQty + dtls.OrderQty;
                                db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                DevourInvEntities db = new DevourInvEntities();
                                var appData = db.INV_SPRDtls.Where(x => x.SPRID == pOrder.SPRID).FirstOrDefault();
                                appData.OrderQty = dtls.OrderQty;
                                db.Entry(appData).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                              
                            }


                        }
                    }
                    if (_result.isSucess)
                    {
                        if (pOrder.FirstApproveStatus == "A")
                        {
                            string tableName = "Store Requisition Approval";
                            _result.message = _result.UpdateSuccessfull(tableName);
                        }
                        else
                        {
                            string tableName = "Store Purchas Requisition";
                            _result.message = _result.UpdateSuccessfull(tableName);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _poFactory.Delete(pOrder);
                _result = _poFactory.Save();
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_PO> SearchPurchaseOrder(int? pOID)
        {
            _poFactory = new POrderFactory();
            try
            {
                var list = new List<INV_PO>();
                if (pOID > 0)
                {
                    list = _poFactory.FindBy(x => x.POID == pOID).ToList();
                }
                else
                {
                    list = _poFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_PODtls> SearchPurchaseOrderDtls(int? pOID)
        {
           _poDtlsFactory = new POrderDtlsFactory();
            try
            {
                var list = new List<INV_PODtls>();
                if (pOID > 0)
                {
                    list = _poDtlsFactory.FindBy(x => x.POID == pOID).ToList();
                }
                else
                {
                    list = _poDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


    public class MRRFactorys : IMRRFactory
    {
        private IGenericFactory<INV_MRR> _mrrFactory;
        private IGenericFactory<INV_MRRDtls> _mrrDtlsFactory;

        Result _result = new Result();

        private SQLFactory sqlFactory;
        private Function function;


        //using procedure to save data 
        public Result SaveMRR(INV_MRR mRr, List<VM_TempMrrDtlsList> mRrDtls, List<int> deletepDtlsID)
        {
            _result = new Result();
            sqlFactory = new SQLFactory();
            function = new Function();
            string tableName = "MRR";

            _mrrDtlsFactory = new MRRDtlsFactory();
            if (deletepDtlsID != null)
            {
                foreach (var detailsID in deletepDtlsID)
                {
                    _mrrDtlsFactory.Delete(x => x.MRRDtlsID == detailsID);
                    _result = _mrrDtlsFactory.Save();
                }
            }


            //int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            try
            {
                

                DataTable details = function.ToDataTable(mRrDtls);
               // DataTable detailsID = function.ToDataTable(deletepDtlsID);
                SqlCommand cmd = new SqlCommand("sp_SaveMRR");
                SqlParameter prmErr = new SqlParameter("@rError", SqlDbType.Int);
                prmErr.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmErr);

                SqlParameter prmErr2 = new SqlParameter("@iLastID", SqlDbType.Int);
                prmErr2.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmErr2);

                cmd.Parameters.AddWithValue("@iMRRID", mRr.MRRID);
                cmd.Parameters.AddWithValue("@iMRRNO", mRr.MRRNO);
                cmd.Parameters.AddWithValue("@iPOIDID", mRr.POID);
                cmd.Parameters.AddWithValue("@iQCID", mRr.QCID);
                cmd.Parameters.AddWithValue("@iStatus", mRr.Status);
                cmd.Parameters.AddWithValue("@iMRRDate", mRr.MRRDate);
                cmd.Parameters.AddWithValue("@iSupplierID", mRr.SupplierID);
                cmd.Parameters.AddWithValue("@iSupplierInv", mRr.SupplierInv);
                cmd.Parameters.AddWithValue("@iCreatedBy", mRr.CreatedBy);
                cmd.Parameters.AddWithValue("@iUpdatedBy", mRr.UpdatedBy);
                cmd.Parameters.AddWithValue("@iMRRDetails", details);

                //cmd.Parameters.AddWithValue("@IDeletepDtlsID", detailsID);
                var isSave = sqlFactory.ExecuteSP(cmd);
                if (mRr.MRRID ==0)
                {
                    mRr.MRRID = Convert.ToInt32(cmd.Parameters["@iLastID"].Value);
                }
               
              
                if (isSave == "1")
                {
                    _result.isSucess = true;
                    _result.message = _result.SaveSuccessfull(tableName);
                }
                else
                {
                    _result.message = isSave;
                }
            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }


        //public Result SaveMRR(INV_MRR mRr, List<INV_MRRDtls> mRrDtls, List<int> deletepDtlsID)
        //{
        //    _mrrFactory = new MRRFactory();
        //    _mrrDtlsFactory = new MRRDtlsFactory();
        //    string tableName = "MRR";

        //    try
        //    { 

        //        //var dupOrder = _mrrFactory.FindBy(x=>x)

        //        if (mRr.MRRID > 0)
        //        {

        //            _mrrFactory.Edit(mRr);
        //            _result = _mrrFactory.Save();

        //            if (_result.isSucess)
        //            {// delete rows during edit
        //                if (deletepDtlsID != null)
        //                {
        //                    foreach (var detailsID in deletepDtlsID)
        //                    {
        //                        _mrrDtlsFactory.Delete(x => x.MRRDtlsID == detailsID);
        //                        _result = _mrrDtlsFactory.Save();
        //                    }
        //                }

        //                if (mRrDtls != null)
        //                {
        //                    foreach (var dtls in mRrDtls)
        //                    {
        //                        if (dtls.MRRDtlsID < 1)
        //                        {

        //                            int mRRDtlsID = 1;
        //                            var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.MRRDtlsID).FirstOrDefault();

        //                            if (prvmRRDtlsID != null)
        //                            {
        //                                mRRDtlsID = prvmRRDtlsID.MRRDtlsID + 1;
        //                            }

        //                            dtls.MRRDtlsID = mRRDtlsID;
        //                            dtls.MRRID = mRr.MRRID;
        //                            _mrrDtlsFactory.Add(dtls);
        //                            _result = _mrrDtlsFactory.Save();

        //                            if (_result.isSucess)
        //                            {
        //                             _result.message = _result.UpdateSuccessfull(tableName);
                                      
        //                            }

        //                        }
        //                        else
        //                        {
        //                            dtls.MRRID = mRr.MRRID;
        //                            _mrrDtlsFactory.Edit(dtls);
        //                            _result = _mrrDtlsFactory.Save();

        //                            if (_result.isSucess)
        //                            {
        //                               _result.message = _result.UpdateSuccessfull(tableName);
                                      

        //                            }
        //                        }
        //                    }

        //                }

        //            }

        //        }
        //        else
        //        {
        //            int mRRID = 1;
        //            var prvmRRID = _mrrFactory.GetLastRecord().OrderByDescending(x => x.MRRID).FirstOrDefault();

        //            if (prvmRRID != null)
        //            {
        //                mRRID = prvmRRID.MRRID + 1;
        //            }

        //            mRr.MRRID = mRRID;
        //            _mrrFactory.Add(mRr);
        //            _result = _mrrFactory.Save();

        //            if (_result.isSucess)
        //            {
        //                foreach (var dtls in mRrDtls)
        //                {
        //                    if (dtls.MRRDtlsID < 1)
        //                    {

        //                        int mRRDtlsID = 1;
        //                        var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.MRRDtlsID).FirstOrDefault();

        //                        if (prvmRRDtlsID != null)
        //                        {
        //                            mRRDtlsID = prvmRRDtlsID.MRRDtlsID + 1;
        //                        }

        //                        dtls.MRRDtlsID = mRRDtlsID;
        //                        dtls.MRRID = mRr.MRRID;
        //                        _mrrDtlsFactory.Add(dtls);
        //                        _result = _mrrDtlsFactory.Save();


        //                    }


        //                }
        //            }
        //            if (_result.isSucess)
        //            {
        //             _result.message = _result.UpdateSuccessfull(tableName);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _mrrFactory.Delete(mRr);
        //        _result = _mrrFactory.Save();
        //        _result.isSucess = false;
        //        _result.message = ex.Message;
        //    }
        //    return _result;
        //}
        public List<INV_MRR> SearchMRR(int? mRRID)
        {
            _mrrFactory = new MRRFactory();
            try
            {
                var list = new List<INV_MRR>();
                if (mRRID > 0)
                {
                    list = _mrrFactory.FindBy(x => x.MRRID == mRRID).ToList();
                }
                else
                {
                    list = _mrrFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<INV_PODtls> SearchMRRDtls(int? pOID)
        //{
        //    _poDtlsFactory = new POrderDtlsFactory();
        //    try
        //    {
        //        var list = new List<INV_PODtls>();
        //        if (pOID > 0)
        //        {
        //            list = _poDtlsFactory.FindBy(x => x.POID == pOID).ToList();
        //        }
        //        else
        //        {
        //            list = _poDtlsFactory.GetAll().ToList();
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    public class MRRHeadOfficeFactorys : IMRRHeadOfficeFactory
    {
        private IGenericFactory<INV_MRR_HO> _mrrFactory;
        private IGenericFactory<INV_MRR_HODtls> _mrrDtlsFactory;

        Result _result = new Result();

        private SQLFactory sqlFactory;
        private Function function;


        //using procedure to save data 
        //public Result SaveMRRHeadOffice(INV_MRR mRr, List<VM_TempMrrDtlsList> mRrDtls, List<int> deletepDtlsID)
        //{
        //    _result = new Result();
        //    sqlFactory = new SQLFactory();
        //    function = new Function();
        //    string tableName = "MRR";
        //    //int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
        //    try
        //    {


        //        DataTable details = function.ToDataTable(mRrDtls);
        //        // DataTable detailsID = function.ToDataTable(deletepDtlsID);
        //        SqlCommand cmd = new SqlCommand("sp_SaveMRR");
        //        SqlParameter prmErr = new SqlParameter("@rError", SqlDbType.Int);
        //        prmErr.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(prmErr);

        //        cmd.Parameters.AddWithValue("@iMRRID", mRr.MRRID);
        //        cmd.Parameters.AddWithValue("@iMRRNO", mRr.MRRNO);
        //        cmd.Parameters.AddWithValue("@iPOIDID", mRr.POID);
        //        cmd.Parameters.AddWithValue("@iStatus", mRr.Status);
        //        cmd.Parameters.AddWithValue("@iMRRDate", mRr.MRRDate);
        //        cmd.Parameters.AddWithValue("@iSupplierID", mRr.SupplierID);
        //        cmd.Parameters.AddWithValue("@iSupplierInv", mRr.SupplierInv);
        //        cmd.Parameters.AddWithValue("@iCreatedBy", mRr.CreatedBy);
        //        cmd.Parameters.AddWithValue("@iUpdatedBy", mRr.UpdatedBy);
        //        cmd.Parameters.AddWithValue("@iMRRDetails", details);

        //        //cmd.Parameters.AddWithValue("@IDeletepDtlsID", detailsID);
        //        var isSave = sqlFactory.ExecuteSP(cmd);
        //        if (isSave == "1")
        //        {
        //            _result.isSucess = true;
        //            _result.message = _result.SaveSuccessfull(tableName);
        //        }
        //        else
        //        {
        //            _result.message = isSave;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _result.isSucess = false;
        //        _result.message = ex.Message;
        //    }
        //    return _result;
        //}


        public Result SaveMRRHeadOffice(INV_MRR_HO mRr, List<INV_MRR_HODtls> mRrDtls, List<int> deletepDtlsID)
        {
            _mrrFactory = new MRRHeadOfficeFactory();
            _mrrDtlsFactory = new MRRDtlsHeadOfficeFactory();
            string tableName = "MRR Head Office";

            try
            {

                //var dupOrder = _mrrFactory.FindBy(x=>x)

                if (mRr.MRRID > 0)
                {

                    _mrrFactory.Edit(mRr);
                    _result = _mrrFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deletepDtlsID != null)
                        {
                            foreach (var detailsID in deletepDtlsID)
                            {
                                _mrrDtlsFactory.Delete(x => x.MRRDtlsID == detailsID);
                                _result = _mrrDtlsFactory.Save();
                            }
                        }

                        if (mRrDtls != null)
                        {
                            foreach (var dtls in mRrDtls)
                            {
                                if (dtls.MRRDtlsID < 1)
                                {

                                    int mRRDtlsID = 1;
                                    var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.MRRDtlsID).FirstOrDefault();

                                    if (prvmRRDtlsID != null)
                                    {
                                        mRRDtlsID = prvmRRDtlsID.MRRDtlsID + 1;
                                    }

                                    dtls.MRRDtlsID = mRRDtlsID;
                                    dtls.MRRID = mRr.MRRID;
                                    _mrrDtlsFactory.Add(dtls);
                                    _result = _mrrDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);

                                    }

                                }
                                else
                                {
                                    dtls.MRRID = mRr.MRRID;
                                    _mrrDtlsFactory.Edit(dtls);
                                    _result = _mrrDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);


                                    }
                                }
                            }

                        }

                    }

                }
                else
                {
                    int mRRID = 1;
                    var prvmRRID = _mrrFactory.GetLastRecord().OrderByDescending(x => x.MRRID).FirstOrDefault();

                    if (prvmRRID != null)
                    {
                        mRRID = prvmRRID.MRRID + 1;
                    }

                    mRr.MRRID = mRRID;
                    _mrrFactory.Add(mRr);
                    _result = _mrrFactory.Save();

                    if (_result.isSucess)
                    {
                        foreach (var dtls in mRrDtls)
                        {
                            if (dtls.MRRDtlsID < 1)
                            {

                                int mRRDtlsID = 1;
                                var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.MRRDtlsID).FirstOrDefault();

                                if (prvmRRDtlsID != null)
                                {
                                    mRRDtlsID = prvmRRDtlsID.MRRDtlsID + 1;
                                }

                                dtls.MRRDtlsID = mRRDtlsID;
                                dtls.MRRID = mRr.MRRID;
                                _mrrDtlsFactory.Add(dtls);
                                _result = _mrrDtlsFactory.Save();


                            }


                        }
                    }
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _mrrFactory.Delete(mRr);
                _result = _mrrFactory.Save();
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }


        public List<INV_MRR_HO> SearchMRRHeadOffice(int? mRRID)
        {
            _mrrFactory = new MRRHeadOfficeFactory();
            try
            {
                var list = new List<INV_MRR_HO>();
                if (mRRID > 0)
                {
                    list = _mrrFactory.FindBy(x => x.MRRID == mRRID).ToList();
                }
                else
                {
                    list = _mrrFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


    public class MRSFactorys : IMRSFactory
    {
        private IGenericFactory<INV_MaterialReceive> _mrrFactory;
        private IGenericFactory<INV_MaterialReceiveDtls> _mrrDtlsFactory;

        Result _result = new Result();

        private SQLFactory sqlFactory;
        private Function function;



        public Result SaveMRS(INV_MaterialReceive mRr, List<INV_MaterialReceiveDtls> mRrDtls, List<int> deletepDtlsID)
        {
            _mrrFactory = new MRSFactory();
            _mrrDtlsFactory = new MRSDtlsFactory();
            string tableName = "MR In Store";

            try
            {
                //var dupOrder = _mrrFactory.FindBy(x=>x)

                if (mRr.MaterialReceiveID > 0)
                {
                    _mrrFactory.Edit(mRr);
                    _result = _mrrFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deletepDtlsID != null)
                        {
                            foreach (var detailsID in deletepDtlsID)
                            {
                                _mrrDtlsFactory.Delete(x => x.ReceiveDtlsID == detailsID);
                                _result = _mrrDtlsFactory.Save();
                            }
                        }

                        if (mRrDtls != null)
                        {
                            foreach (var dtls in mRrDtls)
                            {
                                if (dtls.ReceiveDtlsID < 1)
                                {

                                    int mRRDtlsID = 1;
                                    var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.ReceiveDtlsID).FirstOrDefault();

                                    if (prvmRRDtlsID != null)
                                    {
                                        mRRDtlsID = prvmRRDtlsID.ReceiveDtlsID + 1;
                                    }

                                    dtls.ReceiveDtlsID = mRRDtlsID;
                                    dtls.MaterialReceiveID = mRr.MaterialReceiveID;
                                    _mrrDtlsFactory.Add(dtls);
                                    _result = _mrrDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);

                                    }

                                }
                                else
                                {
                                    dtls.MaterialReceiveID = mRr.MaterialReceiveID;
                                    _mrrDtlsFactory.Edit(dtls);
                                    _result = _mrrDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);


                                    }
                                }
                            }

                        }

                    }

                }
                else
                {
                    int mRRID = 1;
                    var prvmRRID = _mrrFactory.GetLastRecord().OrderByDescending(x => x.MaterialReceiveID).FirstOrDefault();

                    if (prvmRRID != null)
                    {
                        mRRID = prvmRRID.MaterialReceiveID + 1;
                    }

                    mRr.MaterialReceiveID = mRRID;
                    _mrrFactory.Add(mRr);
                    _result = _mrrFactory.Save();

                    if (_result.isSucess)
                    {
                        foreach (var dtls in mRrDtls)
                        {
                            if (dtls.ReceiveDtlsID < 1)
                            {

                                int mRRDtlsID = 1;
                                var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.ReceiveDtlsID).FirstOrDefault();

                                if (prvmRRDtlsID != null)
                                {
                                    mRRDtlsID = prvmRRDtlsID.ReceiveDtlsID + 1;
                                }

                                dtls.ReceiveDtlsID = mRRDtlsID;
                                dtls.MaterialReceiveID = mRr.MaterialReceiveID;
                                _mrrDtlsFactory.Add(dtls);
                                _result = _mrrDtlsFactory.Save();
                            }
                        }
                    }
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _mrrFactory.Delete(mRr);
                _result = _mrrFactory.Save();
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        

        public List<INV_MaterialReceive> SearchMRS(int? mRRID)
        {
            _mrrFactory = new MRSFactory();
            try
            {
                var list = new List<INV_MaterialReceive>();
                if (mRRID > 0)
                {
                    list = _mrrFactory.FindBy(x => x.MaterialReceiveID == mRRID).ToList();
                }
                else
                {
                    list = _mrrFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_MaterialReceiveDtls> SearchMRSDtls(int? mRRdtlsID)
        {
            _mrrDtlsFactory = new MRSDtlsFactory();
            try
            {
                var list = new List<INV_MaterialReceiveDtls>();
                if (mRRdtlsID > 0)
                {
                    list = _mrrDtlsFactory.FindBy(x => x.ReceiveDtlsID == mRRdtlsID).ToList();
                }
                else
                {
                    list = _mrrDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }

    public class QCFactorys : IQCFactory
    {
        private IGenericFactory<INV_QC> _mrrFactory;
        private IGenericFactory<INV_QCDtls> _mrrDtlsFactory;

        Result _result = new Result();

        private SQLFactory sqlFactory;
        private Function function;



        public Result SaveQC(INV_QC mRr, List<INV_QCDtls> mRrDtls, List<int> deletepDtlsID)
        {
            _mrrFactory = new QCFactory();
            _mrrDtlsFactory = new QCDtlsFactory();
            string tableName = "Quality Certificats";

            try
            {

                //var dupOrder = _mrrFactory.FindBy(x=>x)

                if (mRr.QCID > 0)
                {

                    _mrrFactory.Edit(mRr);
                    _result = _mrrFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deletepDtlsID != null)
                        {
                            foreach (var detailsID in deletepDtlsID)
                            {
                                _mrrDtlsFactory.Delete(x => x.QCID == detailsID);
                                _result = _mrrDtlsFactory.Save();
                            }
                        }

                        if (mRrDtls != null)
                        {
                            foreach (var dtls in mRrDtls)
                            {
                                if (dtls.QCDtlsID < 1)
                                {

                                    int mRRDtlsID = 1;
                                    var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.QCDtlsID).FirstOrDefault();

                                    if (prvmRRDtlsID != null)
                                    {
                                        mRRDtlsID = prvmRRDtlsID.QCDtlsID + 1;
                                    }

                                    dtls.QCDtlsID = mRRDtlsID;
                                    dtls.QCID = mRr.QCID;
                                    _mrrDtlsFactory.Add(dtls);
                                    _result = _mrrDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);

                                    }

                                }
                                else
                                {
                                    dtls.QCID = mRr.QCID;
                                    _mrrDtlsFactory.Edit(dtls);
                                    _result = _mrrDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);


                                    }
                                }
                            }

                        }

                    }

                }
                else
                {
                    int mRRID = 1;
                    var prvmRRID = _mrrFactory.GetLastRecord().OrderByDescending(x => x.QCID).FirstOrDefault();

                    if (prvmRRID != null)
                    {
                        mRRID = prvmRRID.QCID + 1;
                    }

                    mRr.QCID = mRRID;
                    _mrrFactory.Add(mRr);
                    _result = _mrrFactory.Save();

                    if (_result.isSucess)
                    {
                        foreach (var dtls in mRrDtls)
                        {
                            if (dtls.QCDtlsID < 1)
                            {

                                int mRRDtlsID = 1;
                                var prvmRRDtlsID = _mrrDtlsFactory.GetLastRecord().OrderByDescending(x => x.QCDtlsID).FirstOrDefault();

                                if (prvmRRDtlsID != null)
                                {
                                    mRRDtlsID = prvmRRDtlsID.QCDtlsID + 1;
                                }

                                dtls.QCDtlsID = mRRDtlsID;
                                dtls.QCID = mRr.QCID;
                                _mrrDtlsFactory.Add(dtls);
                                _result = _mrrDtlsFactory.Save();


                            }


                        }
                    }
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _mrrFactory.Delete(mRr);
                _result = _mrrFactory.Save();
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }


        public List<INV_QC> SearchQC(int? qcID)
        {
            _mrrFactory = new QCFactory();
            try
            {
                var list = new List<INV_QC>();
                if (qcID > 0)
                {
                    list = _mrrFactory.FindBy(x => x.QCID == qcID).ToList();
                }
                else
                {
                    list = _mrrFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
