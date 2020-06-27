using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Inventory;
using BLL.Models;
using DAL.db;
using InvogueApp.Areas.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory.Inventory
{

    public class StockFactory : GenericFactory<DevourInvEntities, INV_Stock>
    {

    }
    public class StoreReqFactory : GenericFactory<DevourInvEntities, INV_SR>
    {

    }

    public class StoreReqDtlsFactory : GenericFactory<DevourInvEntities, INV_SRDtls>
    {

    }
    public class IssueFactory : GenericFactory<DevourInvEntities, INV_Issue>
    {

    }
    public class IssueDtlsFactory : GenericFactory<DevourInvEntities, INV_IssueDtls>
    {

    }

    public class TransferFactory : GenericFactory<DevourInvEntities, INV_Transfer>
    {

    }
    public class TransferDtlsFactory : GenericFactory<DevourInvEntities, INV_TransferDtls>
    {

    }

    public class itemReturnFactory : GenericFactory<DevourInvEntities, INV_Return>
    {

    }
    public class itemReturnDtlsFactory : GenericFactory<DevourInvEntities, INV_ReturnDtls>
    {

    }

    public class SPRFactory : GenericFactory<DevourInvEntities, INV_SPR>
    {

    }
    public class SPRDtlsFactory : GenericFactory<DevourInvEntities, INV_SPRDtls>
    {

    }

    public class ScrapReturnFactory : GenericFactory<DevourInvEntities, INV_ScrapReturn>
    {

    }

    public class ScrapReturnDtlsFactory : GenericFactory<DevourInvEntities, INV_ScrapReturnDtls>
    {

    }

    public class ScrapReturnFactorys : IScrapReturnFactory
    {
        private IGenericFactory<INV_ScrapReturn> _scrapReturnFactory;
        private IGenericFactory<INV_ScrapReturnDtls> _scrapReturnDtlsFactory;

        Result _result = new Result();
        string tableName = "Store Requisition";

        public Result SaveScrapReturn(INV_ScrapReturn scrapReturn, List<INV_ScrapReturnDtls> scrapReturnDtls, List<int> deletescrapReturnDtls)
        {
            _scrapReturnFactory = new ScrapReturnFactory();
            _scrapReturnDtlsFactory = new ScrapReturnDtlsFactory();
            try
            {
                if (scrapReturn.ScrapReturnID > 0)
                {
                    _scrapReturnFactory.Edit(scrapReturn);
                    _result = _scrapReturnFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deletescrapReturnDtls != null)
                        {
                            foreach (var detailsID in deletescrapReturnDtls)
                            {
                                _scrapReturnFactory.Delete(x => x.ScrapReturnID == detailsID);
                                _result = _scrapReturnFactory.Save();
                            }
                        }

                        if (scrapReturnDtls != null)
                        {
                            foreach (var scrapr in scrapReturnDtls)
                            {
                                if (scrapr.ScrapReturnDtlsID < 1)
                                {

                                    int sRDtlsID = 1;
                                    var prvsRDtlsID = _scrapReturnDtlsFactory.GetLastRecord().OrderByDescending(x => x.ScrapReturnDtlsID).FirstOrDefault();

                                    if (prvsRDtlsID != null)
                                    {
                                        sRDtlsID = prvsRDtlsID.ScrapReturnDtlsID + 1;
                                    }

                                    scrapr.ScrapReturnDtlsID = sRDtlsID;
                                    scrapr.ScrapReturnID = scrapReturn.ScrapReturnID;
                                    _scrapReturnDtlsFactory.Add(scrapr);
                                    _result = _scrapReturnDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }

                                }
                                else
                                {

                                    _scrapReturnDtlsFactory.Edit(scrapr);
                                    _result = _scrapReturnDtlsFactory.Save();

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
                    int sRID = 1;
                    var prvsRID = _scrapReturnFactory.GetLastRecord().OrderByDescending(x => x.ScrapReturnID).FirstOrDefault();

                    if (prvsRID != null)
                    {
                        sRID = prvsRID.ScrapReturnID + 1;
                    }

                    scrapReturn.ScrapReturnID = sRID;
                    _scrapReturnFactory.Add(scrapReturn);
                    _result = _scrapReturnFactory.Save();

                    if (scrapReturnDtls != null)
                    {
                        foreach (var scrapr in scrapReturnDtls)
                        {
                            
                                int sRDtlsID = 1;
                                var prvsRDtlsID = _scrapReturnDtlsFactory.GetLastRecord().OrderByDescending(x => x.ScrapReturnDtlsID).FirstOrDefault();

                                if (prvsRDtlsID != null)
                                {
                                    sRDtlsID = prvsRDtlsID.ScrapReturnDtlsID + 1;
                                }

                                scrapr.ScrapReturnDtlsID = sRDtlsID;
                                scrapr.ScrapReturnID = scrapReturn.ScrapReturnID;
                                _scrapReturnDtlsFactory.Add(scrapr);
                                _result = _scrapReturnDtlsFactory.Save();

                                if (_result.isSucess)
                                {
                                    _result.message = _result.UpdateSuccessfull(tableName);
                                }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                _scrapReturnFactory.Delete(scrapReturn);
                _result = _scrapReturnFactory.Save();
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_ScrapReturn> SearchScrapReturn(int? ScrapReturnID)
        {
            _scrapReturnFactory = new ScrapReturnFactory();
            try
            {
                var list = new List<INV_ScrapReturn>();
                if (ScrapReturnID > 0)
                {
                    list = _scrapReturnFactory.FindBy(x => x.ScrapReturnID == ScrapReturnID).ToList();
                }
                else
                {
                    list = _scrapReturnFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_ScrapReturnDtls> SearchScrapReturnDtls(int? ScrapReturnID)
        {
            _scrapReturnDtlsFactory = new ScrapReturnDtlsFactory();
            try
            {
                var list = new List<INV_ScrapReturnDtls>();
                if (ScrapReturnID > 0)
                {
                    list = _scrapReturnDtlsFactory.FindBy(x => x.ScrapReturnID == ScrapReturnID).ToList();
                }
                else
                {
                    list = _scrapReturnDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    public class StoreReqFactorys : IStoreReqFactory
    {
        private IGenericFactory<INV_SR> _storeReqFactory;
        private IGenericFactory<INV_SRDtls> _storeReqDtlsFactory;

        Result _result = new Result();
        string tableName = "Store Requisition";

        public Result SaveStoreReq(INV_SR storeReq, List<INV_SRDtls> storeReqDtls, List<int> deleteStoreReqDtlsID)
        {
            _storeReqFactory = new StoreReqFactory();
            _storeReqDtlsFactory = new StoreReqDtlsFactory();
            try
            {
                if (storeReq.SRID > 0)
                {
                    _storeReqFactory.Edit(storeReq);
                    _result = _storeReqFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deleteStoreReqDtlsID != null)
                        {
                            foreach (var detailsID in deleteStoreReqDtlsID)
                            {
                                _storeReqDtlsFactory.Delete(x => x.SRDtlsID == detailsID);
                                _result = _storeReqDtlsFactory.Save();
                            }
                        }

                        if (storeReqDtls != null)
                        {
                            foreach (var store in storeReqDtls)
                            {
                                if (store.SRDtlsID < 1)
                                {

                                    int sRDtlsID = 1;
                                    var prvsRDtlsID = _storeReqDtlsFactory.GetLastRecord().OrderByDescending(x => x.SRDtlsID).FirstOrDefault();

                                    if (prvsRDtlsID != null)
                                    {
                                        sRDtlsID = prvsRDtlsID.SRDtlsID + 1;
                                    }

                                    store.SRDtlsID = sRDtlsID;
                                    store.SRID = storeReq.SRID;
                                    _storeReqDtlsFactory.Add(store);
                                    _result = _storeReqDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }

                                }
                                else
                                {

                                    _storeReqDtlsFactory.Edit(store);
                                    _result = _storeReqDtlsFactory.Save();

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
                    int sRID = 1;
                    var prvsRID = _storeReqFactory.GetLastRecord().OrderByDescending(x => x.SRID).FirstOrDefault();

                    if (prvsRID != null)
                    {
                        sRID = prvsRID.SRID + 1;
                    }

                    storeReq.SRID = sRID;
                    _storeReqFactory.Add(storeReq);
                    _result = _storeReqFactory.Save();

                    if (_result.isSucess)
                    {
                        foreach (var store in storeReqDtls)
                        {
                              
                            if (store.SRDtlsID < 1)
                            {

                                int sRDtlsID = 1;
                                var prvsRDtlsID = _storeReqDtlsFactory.GetLastRecord().OrderByDescending(x => x.SRDtlsID).FirstOrDefault();

                                if (prvsRDtlsID != null)
                                {
                                    sRDtlsID = prvsRDtlsID.SRDtlsID + 1;
                                }

                                store.SRDtlsID = sRDtlsID;
                                store.SRID = storeReq.SRID;
                                _storeReqDtlsFactory.Add(store);
                                _result = _storeReqDtlsFactory.Save();

                                if (_result.isSucess)
                                {
                                    _result.message = _result.UpdateSuccessfull(tableName);
                                }

                            }
                            if (_result.isSucess)
                            {
                                _result.message = _result.SaveSuccessfull(tableName);
                            }
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                _storeReqFactory.Delete(storeReq);
                _result = _storeReqFactory.Save();
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_SR> SearchStoreReq(int? srID)
        {
            _storeReqFactory = new StoreReqFactory();
            try
            {
                var list = new List<INV_SR>();
                if (srID > 0)
                {
                    list = _storeReqFactory.FindBy(x => x.SRID == srID).ToList();
                }
                else
                {
                    list = _storeReqFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_SRDtls> SearchStoreReqDtls(int? sRID)
        {
            _storeReqDtlsFactory = new StoreReqDtlsFactory();
            try
            {
                var list = new List<INV_SRDtls>();
                if (sRID > 0)
                {
                    list = _storeReqDtlsFactory.FindBy(x => x.SRID == sRID).ToList();
                }
                else
                {
                    list = _storeReqDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


    public class IssueItemFactorys : IIssueItemFactory
    {
        private IGenericFactory<INV_SR> _storeReqFactory;
        private IGenericFactory<INV_Issue> _issueFactory;
        private IGenericFactory<INV_IssueDtls> _issueDtlsFactory;
        private IGenericFactory<INV_SRDtls> _storeReqsDtlsFactory;

        private SQLFactory sqlFactory;
        private Function function;
       
        Result _result = new Result();
        string tableName = "Issue Item";

        public Result SaveIssueItem(INV_Issue issue, List<VM_TempIssueDetails> issueDtls)
        {
            _result = new Result();
            sqlFactory = new SQLFactory();
            function = new Function();
            string tableName = "Issue Item";
            //int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            try
            {


                DataTable details = function.ToDataTable(issueDtls);
                // DataTable detailsID = function.ToDataTable(deletepDtlsID);
                SqlCommand cmd = new SqlCommand("sp_SaveIssue");
                SqlParameter prmErr = new SqlParameter("@rError", SqlDbType.VarChar, 100);
                prmErr.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmErr);

                cmd.Parameters.AddWithValue("@iIssueID", issue.IssueID);
                cmd.Parameters.AddWithValue("@iSRID", issue.SRID);
                cmd.Parameters.AddWithValue("@iIssueNO", issue.IssueNO);
                cmd.Parameters.AddWithValue("@iIssueDate", issue.IssueDate);
                cmd.Parameters.AddWithValue("@iStatus", issue.Status);
                cmd.Parameters.AddWithValue("@iCreatedBy", issue.CreatedBy);
                cmd.Parameters.AddWithValue("@iUpdatedBy", issue.UpdatedBy);

                cmd.Parameters.AddWithValue("@iIssueDetails", details);

                //cmd.Parameters.AddWithValue("@IDeletepDtlsID", detailsID);
                var isSave = sqlFactory.ExecuteSP(cmd);

                var error = cmd.Parameters["@rError"].Value.ToString();
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






        //public Result SaveIssueItem(INV_Issue issue, List<INV_IssueDtls> issueDtls)
        //{
        //    _issueFactory = new IssueFactory();
        //    _issueDtlsFactory = new IssueDtlsFactory();
        //    _storeReqsDtlsFactory = new StoreReqDtlsFactory();
        //    INV_SRDtls srdtls = new INV_SRDtls();

        //    try
        //    {
        //          var datas = _issueFactory.FindBy(x => x.SRID == issue.SRID).FirstOrDefault();
        //          if (datas != null)
        //          {
        //              issue.IssueID = datas.IssueID;
        //            _issueFactory.Edit(issue);
        //            _result = _issueFactory.Save();

        //            if (_result.isSucess)
        //            {// delete rows during edit
        //                if (issueDtls != null)
        //                {
        //                    int indexer = 0;
        //                    foreach (var issues in issueDtls)
        //                    {
        //                        if (issues.IssueDtlsID < 1)
        //                        {

        //                            int issueDtlsID = 1;
        //                            var prvissueDtlsID = _issueDtlsFactory.GetLastRecord().OrderByDescending(x => x.IssueDtlsID).FirstOrDefault();

        //                            if (prvissueDtlsID != null)
        //                            {
        //                                issueDtlsID = prvissueDtlsID.IssueDtlsID + 1;
        //                            }

        //                            issues.IssueDtlsID = issueDtlsID;
        //                            issues.IssueID = issue.IssueID;
        //                            _issueDtlsFactory.Add(issues);
        //                            _result = _issueDtlsFactory.Save();


        //                            if (_result.isSucess)
        //                            {
        //                                _result.message = _result.UpdateSuccessfull(tableName);
        //                            }

        //                        }
        //                        else
        //                        {

        //                            _issueDtlsFactory.Edit(issues);
        //                            _result = _issueDtlsFactory.Save();

        //                            var data = _storeReqsDtlsFactory.FindBy(x => x.SRID == issue.SRID).ToList();
        //                            for (int i = 0; i < data.Count(); i++)
        //                            {
        //                                if (indexer == i)
        //                                {

        //                                    data[i].IssueQty = issues.IssueQty;
        //                                    _storeReqsDtlsFactory.Edit(data[i]);
        //                                    _result = _storeReqsDtlsFactory.Save();
        //                                }
        //                            }

        //                            if (_result.isSucess)
        //                            {
        //                                _result.message = _result.UpdateSuccessfull(tableName);
        //                            }
        //                        }
        //                        indexer++;
        //                    }

        //                }

        //            }

        //        }
        //        else
        //        {
        //            int issueID = 1;
        //            var prvissueID = _issueFactory.GetLastRecord().OrderByDescending(x => x.IssueID).FirstOrDefault();

        //            if (prvissueID != null)
        //            {
        //                issueID = prvissueID.IssueID + 1;
        //            }

        //            issue.IssueID = issueID;
        //            _issueFactory.Add(issue);
        //            _result = _issueFactory.Save();

        //            if (_result.isSucess)
        //            {
        //                int indexer = 0;
        //                foreach (var issues in issueDtls)
        //                {

        //                    if (issues.IssueDtlsID < 1)
        //                    {
        //                        int issueDtlsID = 1;
        //                        var prvissueDtlsID = _issueDtlsFactory.GetLastRecord().OrderByDescending(x => x.IssueDtlsID).FirstOrDefault();

        //                        if (prvissueDtlsID != null)
        //                        {
        //                            issueDtlsID = prvissueDtlsID.IssueDtlsID + 1;
        //                        }
        //                        issues.IssueID = issue.IssueID;
        //                        issues.IssueDtlsID = issueDtlsID;
        //                        _issueDtlsFactory.Add(issues);
        //                        _result = _issueDtlsFactory.Save();

        //                        if (_result.isSucess)
        //                        {

        //                            var data = _storeReqsDtlsFactory.FindBy(x => x.SRID == issue.SRID).ToList();
        //                            for (int i = 0; i < data.Count(); i++)
        //                            {
        //                                if (indexer == i)
        //                                {

        //                                    data[i].IssueQty = issues.IssueQty;
        //                                    _storeReqsDtlsFactory.Edit(data[i]);
        //                                    _result = _storeReqsDtlsFactory.Save();
        //                                }
        //                            }

        //                        }

        //                    }

        //                    indexer++;
        //                }

        //                if (_result.isSucess)
        //                {
        //                    _result.message = _result.UpdateSuccessfull(tableName);
        //                }

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _result.isSucess = false;
        //        _result.message = ex.Message;
        //    }
        //    return _result;
        //}
        public List<INV_SR> SearchStoreReq(int? branchID)
        {
            _storeReqFactory = new StoreReqFactory();
            try
            {
                var list = new List<INV_SR>();
                if (branchID > 0)
                {
                    list = _storeReqFactory.FindBy(x => x.BranchID == branchID).ToList();
                }
                else
                {
                    list = _storeReqFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_IssueDtls> SearchIssueDtls(int? issueID)
        {
            _issueDtlsFactory = new IssueDtlsFactory();
            try
            {
                var list = new List<INV_IssueDtls>();
                if (issueID > 0)
                {
                    list = _issueDtlsFactory.FindBy(x => x.IssueID == issueID).ToList();
                }
                else
                {
                    list = _issueDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


    public class TransferFactorys : ITransferFactory
    {
        private IGenericFactory<INV_Transfer> _transferFactory;
        private IGenericFactory<INV_TransferDtls> _transferDtlsFactory;

        Result _result = new Result();

        private SQLFactory sqlFactory;
        private Function function;

        public Result SaveTransfer(INV_Transfer transfer, List<VM_TempTransferDtls> transferDtls, List<int> deletetransferID)
        {
            _result = new Result();
            sqlFactory = new SQLFactory();
            function = new Function();
            string tableName = "Transfer";

           // var prvsPRDtlsID = _sprDtlsFactory.GetLastRecord().OrderByDescending(x => x.SPRDtlsID).FirstOrDefault();
            //int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            _transferFactory = new TransferFactory();
            _transferDtlsFactory = new TransferDtlsFactory();
            if (deletetransferID != null)
            {
                foreach (var detailsID in deletetransferID)
                {
                    _transferDtlsFactory.Delete(x => x.TransferDtlsID == detailsID);
                    _result = _transferDtlsFactory.Save();
                }
            }

            try
            {
                DataTable details = function.ToDataTable(transferDtls);
                SqlCommand cmd = new SqlCommand("sp_SaveTransfer");
                SqlParameter prmErr = new SqlParameter("@rError", SqlDbType.Int);
                prmErr.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmErr);

                cmd.Parameters.AddWithValue("@iTransferID", transfer.TransferID);
                cmd.Parameters.AddWithValue("@iTransferNO", transfer.TransferNO);
                cmd.Parameters.AddWithValue("@iTransferDate", transfer.TransferDate);
                cmd.Parameters.AddWithValue("@iFromStoreID", transfer.FromStoreID);
                cmd.Parameters.AddWithValue("@iToStoreID", transfer.ToStoreID);
                cmd.Parameters.AddWithValue("@iCreatedBy", transfer.CreatedBy);
                cmd.Parameters.AddWithValue("@iUpdatedBy", transfer.UpdatedBy);
                cmd.Parameters.AddWithValue("@iStatus", transfer.Status);

                cmd.Parameters.AddWithValue("@iTransferDetails", details);
                var isSave = sqlFactory.ExecuteSP(cmd);

                //var error = (int)cmd.Parameters["@rError"].Value;
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



        //public Result SaveTransfer(INV_Transfer transfer, List<INV_TransferDtls> transferDtls, List<int> deletetransferID)
        //{
        //    _transferFactory = new TransferFactory();
        //    _transferDtlsFactory = new TransferDtlsFactory();

           
        //    try
        //    {
        //        if (transfer.TransferID > 0)
        //        {

        //            _transferFactory.Edit(transfer);
        //            _result = _transferFactory.Save();

        //            if (_result.isSucess)
        //            {// delete rows during edit
        //                if (deletetransferID != null)
        //                {
        //                    foreach (var detailsID in deletetransferID)
        //                    {
        //                        _transferDtlsFactory.Delete(x => x.TransferDtlsID == detailsID);
        //                        _result = _transferDtlsFactory.Save();
        //                    }
        //                }

        //                if (transferDtls != null)
        //                {
        //                    foreach (var dtls in transferDtls)
        //                    {
        //                        if (dtls.TransferDtlsID < 1)
        //                        { 

        //                            int trDtlsID = 1;
        //                            var prvtrDtlsID = _transferDtlsFactory.GetLastRecord().OrderByDescending(x => x.TransferDtlsID).FirstOrDefault();

        //                            if (prvtrDtlsID != null)
        //                            {
        //                                trDtlsID = prvtrDtlsID.TransferDtlsID + 1;
        //                            }

        //                            dtls.TransferDtlsID = trDtlsID;
        //                            dtls.TransferID = transfer.TransferID;
        //                            _transferDtlsFactory.Add(dtls);
        //                            _result = _transferDtlsFactory.Save();

        //                            if (_result.isSucess)
        //                            {

        //                                if (transfer.Status == "A")
        //                                {
        //                                    string tableName = "Transfer Requisition Approval";
        //                                    _result.message = _result.UpdateSuccessfull(tableName);
        //                                }
        //                                else
        //                                {
        //                                    string tableName = "Transfer Requisition";
        //                                    _result.message = _result.UpdateSuccessfull(tableName);
        //                                }

                                        
        //                            }

        //                        }
        //                        else
        //                        {
        //                            dtls.TransferID = transfer.TransferID;
        //                            _transferDtlsFactory.Edit(dtls);
        //                            _result = _transferDtlsFactory.Save();

        //                            if (_result.isSucess)
        //                            {
        //                                if (transfer.Status == "A")
        //                                {
        //                                    string tableName = "Transfer Requisition Approval";
        //                                    _result.message = _result.UpdateSuccessfull(tableName);
        //                                }
        //                                else
        //                                {
        //                                    string tableName = "Transfer Requisition";
        //                                    _result.message = _result.UpdateSuccessfull(tableName);
        //                                }
                                        
        //                            }
        //                        }
        //                    }

        //                }

        //            }

        //        }
        //        else
        //        {
        //            int trnID = 1;
        //            var prvstrnID = _transferFactory.GetLastRecord().OrderByDescending(x => x.TransferID).FirstOrDefault();

        //            if (prvstrnID != null)
        //            {
        //                trnID = prvstrnID.TransferID + 1;
        //            }

        //            transfer.TransferID = trnID;
        //            _transferFactory.Add(transfer);
        //            _result = _transferFactory.Save();

        //            if (_result.isSucess)
        //            {
        //                foreach (var dtls in transferDtls)
        //                {
        //                    if (dtls.TransferDtlsID < 1)
        //                    {

        //                        int trDtlsID = 1;
        //                        var prvtrDtlsID = _transferDtlsFactory.GetLastRecord().OrderByDescending(x => x.TransferDtlsID).FirstOrDefault();

        //                        if (prvtrDtlsID != null)
        //                        {
        //                            trDtlsID = prvtrDtlsID.TransferDtlsID + 1;
        //                        }

        //                        dtls.TransferDtlsID = trDtlsID;
        //                        dtls.TransferID = transfer.TransferID;
        //                        _transferDtlsFactory.Add(dtls);
        //                        _result = _transferDtlsFactory.Save();



        //                    }


        //                }
        //            }
        //            if (_result.isSucess)
        //            {
        //                if (transfer.Status == "A")
        //                {
        //                    string tableName = "Transfer Requisition Approval";
        //                    _result.message = _result.UpdateSuccessfull(tableName);
        //                }
        //                else
        //                {
        //                    string tableName = "Transfer Requisition";
        //                    _result.message = _result.UpdateSuccessfull(tableName);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _result.isSucess = false;
        //        _result.message = ex.Message;
        //    }
        //    return _result;
        //}
        public List<INV_Transfer> SearchTransfer(int? transferID)
        {
            _transferFactory = new TransferFactory();
            try
            {
                var list = new List<INV_Transfer>();
                if (transferID > 0)
                {
                    list = _transferFactory.FindBy(x => x.TransferID == transferID).ToList();
                }
                else
                {
                    list = _transferFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_TransferDtls> SearchTransferDtls(int? transferID)
        {
            _transferDtlsFactory = new TransferDtlsFactory();
            try
            {
                var list = new List<INV_TransferDtls>();
                if (transferID > 0)
                {
                    list = _transferDtlsFactory.FindBy(x => x.TransferID == transferID).ToList();
                }
                else
                {
                    list = _transferDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }



    public class ItemReturnFactorys : IItemReturnFactory
    {
        private IGenericFactory<INV_Issue> _issueFactory;
        private IGenericFactory<INV_IssueDtls> _issueDtlsFactory;


        private IGenericFactory<INV_Return> _returnFactory;
        private IGenericFactory<INV_ReturnDtls> _returnDtlsFactory;

        Result _result = new Result();
        string tableName = "Item Return";
        private SQLFactory sqlFactory;
        private Function function;



        public Result SaveIetmReturn(INV_Return ietmReturn, List<VM_TempItemReturnDtls> ietmReturnDtls)
        {
            _result = new Result();
            sqlFactory = new SQLFactory();
            function = new Function();
            string tableName = "Item Return";
            //int empID = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            try
            {
                DataTable details = function.ToDataTable(ietmReturnDtls);
                SqlCommand cmd = new SqlCommand("sp_SaveReturn");
                SqlParameter prmErr = new SqlParameter("@rError", SqlDbType.Int);
                prmErr.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmErr);
                SqlParameter prmErr2 = new SqlParameter("@iLastInsertedID", SqlDbType.Int);
                prmErr2.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmErr2);

                SqlParameter lastID = new SqlParameter("@iLastInsertedID", SqlDbType.Int);
                lastID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(lastID);

                cmd.Parameters.AddWithValue("@iReturnID", ietmReturn.ReturnID);
                cmd.Parameters.AddWithValue("@iFromStoreID", ietmReturn.FromStoreID);
                cmd.Parameters.AddWithValue("@iFromWarehouseID", ietmReturn.FromWarehouseID);
                cmd.Parameters.AddWithValue("@iPurchaseOrderID", ietmReturn.PurchaseOrderID);
                cmd.Parameters.AddWithValue("@iReturnDate", ietmReturn.ReturnDate);
                cmd.Parameters.AddWithValue("@iReturnNO", ietmReturn.ReturnNO);
                cmd.Parameters.AddWithValue("@iReturnType", ietmReturn.ReturnType);
                cmd.Parameters.AddWithValue("@iReturnTypeID", ietmReturn.ReturnTypeID);
                cmd.Parameters.AddWithValue("@iSRID", ietmReturn.SRID);
                cmd.Parameters.AddWithValue("@iStatus", ietmReturn.Status);
                cmd.Parameters.AddWithValue("@iToStoreID", ietmReturn.ToStoreID);
                cmd.Parameters.AddWithValue("@iToSupplierID", ietmReturn.ToSupplierID);
                cmd.Parameters.AddWithValue("@iToWarehouseID", ietmReturn.ToWarehouseID);
                cmd.Parameters.AddWithValue("@iTransferID", ietmReturn.TransferID);

                cmd.Parameters.AddWithValue("@iReturnDetails", details);
                var isSave = sqlFactory.ExecuteSP(cmd);
                ietmReturn.ReturnID = (int)cmd.Parameters["@iLastInsertedID"].Value;
                if (isSave == "1")
                {
                    _result.lastInsertedID = Convert.ToInt32(cmd.Parameters["@iLastInsertedID"].Value.ToString());
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
        //public Result SaveIetmReturn(INV_Return ietmReturn, List<INV_ReturnDtls> ietmReturnDtls)
        //{ DevourInvEntities _db = new DevourInvEntities();
        //    _returnFactory = new itemReturnFactory();
        //    _returnDtlsFactory = new itemReturnDtlsFactory();
        //    //using (var transaction = _db.Database.BeginTransaction())
        //    //{

        //    try
        //    {
        //        if (ietmReturn.ReturnID > 0)
        //        {
        //            _returnFactory.Edit(ietmReturn);
        //            _result = _returnFactory.Save();
        //           // transaction.Commit();
        //            if (_result.isSucess)
        //            {// delete rows during edit


        //                if (ietmReturnDtls != null)
        //                {
        //                    foreach (var returns in ietmReturnDtls)
        //                    {
        //                        if (returns.ReturnDtlsID < 1)
        //                        {

        //                            int returnDtlsID = 1;
        //                            var prvreturnDtlsID = _returnDtlsFactory.GetLastRecord().OrderByDescending(x => x.ReturnDtlsID).FirstOrDefault();

        //                            if (prvreturnDtlsID != null)
        //                            {
        //                                returnDtlsID = prvreturnDtlsID.ReturnDtlsID + 1;
        //                            }
        //                            returns.ReturnID = ietmReturn.ReturnID;
        //                            returns.ReturnDtlsID = returnDtlsID;
        //                            _returnDtlsFactory.Add(returns);
        //                            _result = _returnDtlsFactory.Save();
        //                           // transaction.Commit();

        //                            if (_result.isSucess)
        //                            {
        //                                _result.message = _result.UpdateSuccessfull(tableName);
        //                            }

        //                        }
        //                        else
        //                        {

        //                            _returnDtlsFactory.Edit(returns);
        //                            _result = _returnDtlsFactory.Save();
        //                           // transaction.Commit();
        //                            //if (_result.isSucess)
        //                            //{
        //                            //    var data = _returnDtlsFactory.FindBy(x => x. == issue.SRID).FirstOrDefault();

        //                            //    data.IssueQty = issues.IssueQty; ;
        //                            //    _storeReqsDtlsFactory.Edit(data);
        //                            //    _result = _storeReqsDtlsFactory.Save();


        //                            //    // _result.message = _result.UpdateSuccessfull(tableName);
        //                            //}

        //                            if (_result.isSucess)
        //                            {
        //                                _result.message = _result.UpdateSuccessfull(tableName);
        //                            }
        //                        }
        //                    }

        //                }

        //            }

        //        }
        //        else
        //        {
        //            int returnID = 1;
        //            var prvreturnID = _returnFactory.GetLastRecord().OrderByDescending(x => x.ReturnID).FirstOrDefault();

        //            if (prvreturnID != null)
        //            {
        //                returnID = prvreturnID.ReturnID + 1;
        //            }

        //            ietmReturn.ReturnID = returnID;
        //            _returnFactory.Add(ietmReturn);
        //            _result = _returnFactory.Save();
        //            //transaction.Commit();
        //            if (_result.isSucess)
        //            {
        //                foreach (var returns in ietmReturnDtls)
        //                {
        //                    if (returns.ReturnDtlsID < 1)
        //                    {

        //                        int returnDtlsID = 1;
        //                        var prvreturnDtlsID = _returnDtlsFactory.GetLastRecord().OrderByDescending(x => x.ReturnDtlsID).FirstOrDefault();

        //                        if (prvreturnDtlsID != null)
        //                        {
        //                            returnDtlsID = prvreturnDtlsID.ReturnDtlsID + 1;
        //                        }

        //                        returns.ReturnDtlsID = returnDtlsID;
        //                        returns.ReturnID = ietmReturn.ReturnID; 
        //                        _returnDtlsFactory.Add(returns);
        //                        _result = _returnDtlsFactory.Save();

        //                        //transaction.Commit();
        //                        if (_result.isSucess)
        //                        {
        //                            _result.message = _result.UpdateSuccessfull(tableName);
        //                        }

        //                    }

        //                }

        //                if (_result.isSucess)
        //                {
        //                    _result.message = _result.UpdateSuccessfull(tableName);
        //                }

        //            }

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        //transaction.Rollback();
        //        _result.isSucess = false;
        //        _result.message = ex.Message;
               
        //    }
        //    return _result;
        ////}

        //}
        public List<INV_Return> SearchReturn(int? returnID)
        {
            _returnFactory = new itemReturnFactory();

            _issueFactory = new IssueFactory();
            try
            {
                var list = new List<INV_Return>();
                if (returnID > 0)
                {
                    list = _returnFactory.FindBy(x => x.ReturnID == returnID).ToList();
                }
                else
                {
                    list = _returnFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<INV_ReturnDtls> SearchReturnDtls(int? returnID)
        {

            _returnDtlsFactory = new itemReturnDtlsFactory();
            try
            {
                var list = new List<INV_ReturnDtls>();
                if (returnID > 0)
                {
                    list = _returnDtlsFactory.FindBy(x => x.ReturnID == returnID).ToList();
                }
                else
                {
                    list = _returnDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<INV_Issue> SearchIssue(int? srID)
        {
            _issueFactory = new IssueFactory();
            try
            {
                var list = new List<INV_Issue>();
                if (srID > 0)
                {
                    list = _issueFactory.FindBy(x => x.SRID == srID).ToList();
                }
                else
                {
                    list = _issueFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<INV_IssueDtls> SearchIssueDtls(int? issueID)
        {
            _issueDtlsFactory = new IssueDtlsFactory();
            try
            {
                var list = new List<INV_IssueDtls>();
                if (issueID > 0)
                {
                    list = _issueDtlsFactory.FindBy(x => x.IssueID == issueID).ToList();
                }
                else
                {
                    list = _issueDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class SPRFactorys : ISPRFactory
    {
        private IGenericFactory<INV_SPR> _sprFactory;
        private IGenericFactory<INV_SPRDtls> _sprDtlsFactory;

        Result _result = new Result();


        public Result SaveSPR(INV_SPR storePR, List<INV_SPRDtls> storePRDtls, List<int> deleteSPRDtlsID)
        {
            _sprFactory = new SPRFactory();
            _sprDtlsFactory = new SPRDtlsFactory();


            try
            {
                if (storePR.SPRID > 0)
                {

                    _sprFactory.Edit(storePR);
                    _result = _sprFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deleteSPRDtlsID != null)
                        {
                            foreach (var detailsID in deleteSPRDtlsID)
                            {
                                _sprDtlsFactory.Delete(x => x.SPRDtlsID == detailsID);
                                _result = _sprDtlsFactory.Save();
                            }
                        }

                        if (storePRDtls != null)
                        {
                            foreach (var dtls in storePRDtls)
                            {
                                if (dtls.SPRDtlsID < 1)
                                {

                                    int sPRDtlsID = 1;
                                    var prvsPRDtlsID = _sprDtlsFactory.GetLastRecord().OrderByDescending(x => x.SPRDtlsID).FirstOrDefault();

                                    if (prvsPRDtlsID != null)
                                    {
                                        sPRDtlsID = prvsPRDtlsID.SPRDtlsID + 1;
                                    }

                                    dtls.SPRDtlsID = sPRDtlsID;
                                    dtls.SPRID = storePR.SPRID;
                                    _sprDtlsFactory.Add(dtls);
                                    _result = _sprDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                       


                                        if (storePR.FirstApproveStatus == "A")
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
                                else
                                {
                                    dtls.SPRID = storePR.SPRID;
                                    _sprDtlsFactory.Edit(dtls);
                                    _result = _sprDtlsFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        

                                        if (storePR.FirstApproveStatus == "A")
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

                        }

                    }

                }
                else
                {
                    int sPRID = 1;
                    var prvSPRID = _sprFactory.GetLastRecord().OrderByDescending(x => x.SPRID).FirstOrDefault();

                    if (prvSPRID != null)
                    {
                        sPRID = prvSPRID.SPRID + 1;
                    }

                    storePR.SPRID = sPRID;
                    _sprFactory.Add(storePR);
                    _result = _sprFactory.Save();

                    if (_result.isSucess)
                    {
                        foreach (var dtls in storePRDtls)
                        {
                            if (dtls.SPRDtlsID < 1)
                            {

                                int sPRDtlsID = 1;
                                var prvsPRDtlsID = _sprDtlsFactory.GetLastRecord().OrderByDescending(x => x.SPRDtlsID).FirstOrDefault();

                                if (prvsPRDtlsID != null)
                                {
                                    sPRDtlsID = prvsPRDtlsID.SPRDtlsID + 1;
                                }

                                dtls.SPRDtlsID = sPRDtlsID;
                                dtls.SPRID = storePR.SPRID;
                                _sprDtlsFactory.Add(dtls);
                                _result = _sprDtlsFactory.Save();

                            }
                        }
                    }
                    if (_result.isSucess)
                    {
                        if (storePR.FirstApproveStatus == "A")
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
                _sprFactory.Delete(storePR);
                _result = _sprFactory.Save();
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_SPR> SearchSPR(int? sPRID)
        {
            _sprFactory = new SPRFactory();
            try
            {
                var list = new List<INV_SPR>();
                if (sPRID > 0)
                {
                    list = _sprFactory.FindBy(x => x.SPRID == sPRID).ToList();
                }
                else
                {
                    list = _sprFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_SPRDtls> SearchSprDtls(int? sPRID)
        {
            _sprDtlsFactory = new SPRDtlsFactory();
            try
            {
                var list = new List<INV_SPRDtls>();
                if (sPRID > 0)
                {
                    list = _sprDtlsFactory.FindBy(x => x.SPRID == sPRID).ToList();
                }
                else
                {
                    list = _sprDtlsFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class CurrentStockFactorys : ICurrentStock
    {
        private IGenericFactory<INV_Stock> _stockFactory;
        public List<INV_Stock> SearchStockByStore(int? branchID)
        {
            _stockFactory = new StockFactory();
            try
            {
                var list = new List<INV_Stock>();
                if (branchID > 0)
                {
                    list = _stockFactory.FindBy(x => x.BranchID == branchID).ToList();
                }
                else
                {
                    list = _stockFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<INV_Stock> SearchStock(int? branchID)
        {
            _stockFactory = new StockFactory();
            try
            {
                var list = new List<INV_Stock>();
                if (branchID > 0)
                {
                    list = _stockFactory.FindBy(x => x.BranchID == branchID).ToList();
                }
                else
                {
                    list = _stockFactory.GetAll().ToList();
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
