using BLL.Common;
using BLL.Models;
using DAL.db;
using InvogueApp.Areas.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Inventory
{
    public interface IStoreReqFactory
    {
        Result SaveStoreReq(INV_SR storeReq, List<INV_SRDtls> storeReqDtls, List<int> deleteStoreReqDtlsID);
        List<INV_SR> SearchStoreReq(int? srID);

        List<INV_SRDtls> SearchStoreReqDtls(int? sRID);
    }
    public interface IScrapReturnFactory
    {
        Result SaveScrapReturn(INV_ScrapReturn scrapReturn, List<INV_ScrapReturnDtls> scrapReturnDtls, List<int> deletescrapReturnDtls);
        List<INV_ScrapReturn> SearchScrapReturn(int? ScrapReturnID);
        List<INV_ScrapReturnDtls> SearchScrapReturnDtls(int? ScrapReturnID);
    }
    public interface IIssueItemFactory
    {
        Result SaveIssueItem(INV_Issue issue, List<VM_TempIssueDetails> issueDtls);
        List<INV_SR> SearchStoreReq(int? storeID);
        List<INV_IssueDtls> SearchIssueDtls(int? issueID);
    }

    public interface ITransferFactory
    {
        Result SaveTransfer(INV_Transfer transfer, List<VM_TempTransferDtls> transferDtls, List<int> deletetransferID);
        List<INV_Transfer> SearchTransfer(int? transferID);

        List<INV_TransferDtls> SearchTransferDtls(int? transferID);
    }
    public interface ISPRFactory
    {
        Result SaveSPR(INV_SPR storePR, List<INV_SPRDtls> storePRDtls, List<int> deleteSPRDtlsID);
        List<INV_SPR> SearchSPR(int? sPRID);
        List<INV_SPRDtls> SearchSprDtls(int? sPRID);
    }

   public interface IItemReturnFactory
    {
       Result SaveIetmReturn(INV_Return ietmReturn, List<VM_TempItemReturnDtls> ietmReturnDtls);
       List<INV_Issue> SearchIssue(int? srID);
       List<INV_IssueDtls> SearchIssueDtls(int? issueID);

       List<INV_Return> SearchReturn(int? returnID);
       List<INV_ReturnDtls> SearchReturnDtls(int? returnID);
    }
   public interface ICurrentStock
   {
       List<INV_Stock> SearchStockByStore(int? storeID);
       List<INV_Stock> SearchStock(int? stockID);
   }





}
