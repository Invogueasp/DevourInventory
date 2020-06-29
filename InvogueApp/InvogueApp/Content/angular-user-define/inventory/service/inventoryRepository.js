angular.module('app').factory('inventoryRepository', function ($http, $location) {
    return {

        // ============== Store Requisition =============================================

        //saveStoreReq: function (storeReq, storeReqDtls, deleteStoreReqDtlsID) {
        //    debugger
        //    return $http.post('/StoreRequisition/SaveStoreReq', { "storeReq": storeReq, "storeReqDtls": storeReqDtls, "deleteStoreReqDtlsID": deleteStoreReqDtlsID });
        //},
        //loadStoreReq: function (id) {
        //    return $http.post('/StoreRequisition/LoadStoreReq', { "srID": id });
        //},
        //loadStoreReqDtls: function (id) {
        //    return $http.post('/StoreRequisition/LoadStoreDtls', { "sRID": id });
        //},
        saveStoreReqApp: function (storeReq, storeReqDtls, deleteStoreReqDtlsID) {
            debugger
            return $http.post('/SRApproval/saveStoreReqApp', { "storeReq": storeReq, "storeReqDtls": storeReqDtls, "deleteStoreReqDtlsID": deleteStoreReqDtlsID });
        },
        secondAppStoreReqApp: function (storeReq, storeReqDtls, deleteStoreReqDtlsID) {
            debugger
            return $http.post('/SRApproval/SecondAppStoreReq', { "storeReq": storeReq, "storeReqDtls": storeReqDtls, "deleteStoreReqDtlsID": deleteStoreReqDtlsID });
        },
        loadStoreReq: function (id, departmentID,fromDate ,toDate) {
            return $http.post('/Issue/LoadStoreReq', { "storeID": id, "departmentID": departmentID, "fromDate": fromDate, "toDate": toDate });
        },
        loadStoreReqDtls: function (id) {
            return $http.post('/Issue/LoadStoreDtls', { "sRID": id });
        },
        saveIssue: function (issueItem, issueDtl) {
            debugger
            return $http.post('/Issue/SaveIssue', { "issue": issueItem, "issueDtls": issueDtl });
        },
        //============================================ Transfer =================================================
        saveTransfer: function (transfer, transferDtls, deleteTransferDtlsID) {
        debugger
        return $http.post('/ItemTransfer/SaveTransfer', { "transfer": transfer, "transferDtls": transferDtls, "deletetransferID": deleteTransferDtlsID });
        },
        loadTransfer: function (id) {
            return $http.post('/ItemTransfer/LoadTransfer', { "transferID": id });
        },
        loadTransferDtls: function (id) {
            return $http.post('/ItemTransfer/LoadTransferDtls', { "transferID": id });
        },
        saveTransferApp: function (transfer, transferDtls, deleteTransferDtlsID) {
            debugger
            return $http.post('/TransferApproval/SaveTransferApp', { "transfer": transfer, "transferDtls": transferDtls, "deletetransferID": deleteTransferDtlsID });
        },
        //================================ Item ItemReturn ==========================================================
        loadIssueID: function (id) {
            return $http.post('/ItemReturn/LoadIssueID', { "srID": id });
        },
        loadIssueDtls: function (id) {
            return $http.post('/ItemReturn/LoadIssueDtls', { "issueID": id });
        },
        loadTransferNo: function (fromStoreID,toStoreID) {
            return $http.post('/ItemReturn/LoadTransferByStoreID', { "fromStoreID": fromStoreID, "toStoreID": toStoreID });
        },
        saveOpeningStock: function (oStock,stocko) {
            return $http.post('/OpeningStock/SaveStockIn', { "oStock": oStock,"stocko":stocko });
        },
        loadTransferDtlsById: function (transferID) {
            return $http.post('/ItemReturn/LoadTransferDtls', { "transferID": transferID });
        },
        loadRtnTransferDtls: function (transferID) {
            return $http.post('/ItemReturn/LoadRtnTransferDtls', { "transferID": transferID });
        },
        saveIetmReturn: function (itemReturn, itemRetunDtlss) {
            debugger
            return $http.post('/ItemReturn/SaveIetmReturn', { "ietmReturn": itemReturn, "ietmReturnDtls": itemRetunDtlss });
        },
        loadReturn: function (returnID) {
            return $http.post('/ItemReturn/LoadReturn', { "returnID": returnID });
        },
        loadReturnDtls: function (returnID) {
            return $http.post('/ItemReturn/LoadReturnDtls', { "returnID": returnID });
        },
        saveIetmReturnDtls: function (itemReturn, itemRetunDtlss) {
            debugger
            return $http.post('/IRApproval/SaveIetmReturnDtls', { "ietmReturn": itemReturn, "ietmReturnDtls": itemRetunDtlss });
        },
        //================================ SPR ==========================================================
        saveSpr: function (storePR, sprDtls, deleteSPRDtlsID) {
            debugger
            return $http.post('/PurchaseRequisition/SaveSPR', { "storePR": storePR, "storePRDtls": sprDtls, "deleteSPRDtlsID": deleteSPRDtlsID });
        },
        loadSpr: function (sPRID, param) {
            return $http.post('/PurchaseRequisition/LoadSPR', { "sPRID": sPRID, "param": param });
        },
        loadSprFrPendingPO: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSprFrPendingPO', { "sPRID": sPRID });
        },
        loadSprBy3rdApp: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSprBy3rdApp', { "sPRID": sPRID });
        },
        loadSprApp: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSPR', { "sPRID": sPRID });
        },
        loadSpr2ndApp: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSPR2ndApp', { "sPRID": sPRID });
        },
        loadSpr3rdApp: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSPR3rdApp', { "sPRID": sPRID });
        },
        loadSprByDate: function (sPRID, FrmRequiredDate, ToRequiredDate) {
            return $http.post('/PurchaseRequisition/LoadSPRByDate', { "sPRID": sPRID, "fromDate": FrmRequiredDate, "toDate": ToRequiredDate });
        },
        loadSprDtls: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSPRDtls', { "sPRID": sPRID });
        },
        loadSprDtlsList: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSPRDtlss', { "sPRID": sPRID });
        },
        loadSprDtlsListFrPendingPO: function (sPRID) {
            return $http.post('/PurchaseRequisition/LoadSPRDtlsListFrPendingPo', { "sPRID": sPRID });
        },
        loadPODtlsList: function (pOID) {
            return $http.post('/PurchaseRequisition/LoadPODtlsList', { "pOID": pOID });
        },
            //================================ SPR App ==========================================================
        saveSPRApp: function (storePR, sprDtls, deleteSPRDtlsID) {
                debugger
                return $http.post('/SPRApproval/SaveSPRApp', { "storePR": storePR, "storePRDtls": sprDtls, "deleteSPRDtlsID": deleteSPRDtlsID });
        },
        saveSPR2ndApp: function (storePR, sprDtls, deleteSPRDtlsID) {
            debugger
            return $http.post('/SPRApproval/SaveSPR2ndApp', { "storePR": storePR, "storePRDtls": sprDtls, "deleteSPRDtlsID": deleteSPRDtlsID });
        },
        saveSPR3rdApp: function (storePR, sprDtls, deleteSPRDtlsID) {
            debugger
            return $http.post('/SPRApproval/SaveSPR3rdApp', { "storePR": storePR, "storePRDtls": sprDtls, "deleteSPRDtlsID": deleteSPRDtlsID });
        },
   
        //================================ Purchase Order ==========================================================
        savePurchase: function (pOrder, pOrderDtls, deletepOrderDtlsID) {
            debugger
            return $http.post('/PurchaseOrder/SavePurchaseOrder', { "pOrder": pOrder, "pOrderDtls": pOrderDtls, "deletepOrderDtlsID": deletepOrderDtlsID });
        },
        savePendingPurchase: function (pOrder, pOrderDtls, deletepOrderDtlsID, check) {
            debugger
            return $http.post('/PurchaseOrder/SavePurchaseOrder', { "pOrder": pOrder, "pOrderDtls": pOrderDtls, "deletepOrderDtlsID": deletepOrderDtlsID, "check": check });
        },
        saveAppPurchase: function (pOrder, pOrderDtls, deletepOrderDtlsID) {
            debugger
            return $http.post('/POApproval/SavePurchaseOrder', { "pOrder": pOrder, "pOrderDtls": pOrderDtls, "deletepOrderDtlsID": deletepOrderDtlsID });
        },
        savePO2ndApp: function (pOrder, pOrderDtls, deletepOrderDtlsID) {
            debugger
            return $http.post('/POApproval/SavePO2ndApp', { "pOrder": pOrder, "pOrderDtls": pOrderDtls, "deletepOrderDtlsID": deletepOrderDtlsID });
        },
        loadPurchaseOrder: function (pOID, param) {
            return $http.post('/PurchaseOrder/LoadPurchaseOrder', { "pOID": pOID, "param": param });
        },
        loadPurchaseOrderDtls: function (pOID) {
            return $http.post('/PurchaseOrder/LoadPurchaseOrderDtls', { "pOID": pOID });
        },
        //================================ Purchase Order App ==========================================================
        loadPurchaseOrderApp: function (pOID) {
            return $http.post('/POApproval/LoadPurchaseOrder', { "pOID": pOID });
        },
        loadPO2ndApp: function (pOID) {
            return $http.post('/POApproval/LoadPO2ndApp', { "pOID": pOID });
        },
        loadIssue: function (srID) {
            return $http.post('/Issue/LoadIssue', { "srID": srID });
        },
        //================================ MATERIAL RECEIVE REPORT ==========================================================
        loadAppPurchaseOrder: function (pOID) {
            return $http.post('/MRR/LoadPurchaseOrder', { "pOID": pOID });
        },
        loadMrrDtls: function (mrrID) {
            return $http.post('/MRR/LoadMrrDtls', { "mrrID": mrrID });
        },
        loadAppPurchaseOrderDtls: function (pOID) {
            return $http.post('/MRR/LoadPurchaseOrderDtls', { "pOID": pOID });
        },
        saveMRR: function (mMr, sprDtls) {
            debugger
            return $http.post('/MRR/SaveMRR', { "mRr": mMr, "mRrDtls": sprDtls });
        },
        loadMRR: function (mRRID, param) {
            return $http.post('/MRR/LoadMRR', { "mRRID": mRRID, "param": param });
        },


        //================================== MRR Head Office ==================================================

        saveMRRHeadOffice: function (uploadImage, mrrHo) {
            debugger
            var formData = new FormData();
            formData.append("file", uploadImage);
            formData.append("mrrHo", angular.toJson(mrrHo));
            var config = {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }
            return $http.post('/MRRHeadOffice/SaveMRRHead', formData, config);

        },
        getMrrImgFile: function (mrrID) {
            return $http.post('/MRRHeadOffice/GetMrrImgFile', { "mrrID": mrrID });
        },
        loadMrrHODtls: function (pOID) {
            return $http.post('/MRRHeadOffice/LoadMrrHODtls', { "pOID": pOID });
        },
        loadMrrHODtlsForMRS: function (mrrID) {
            return $http.post('/MRRHeadOffice/LoadMrrHODtlsForMRS', { "mrrID": mrrID });
        },
        
        loadReportList: function (pOID) {
           return $http.post('/MRRHeadOffice/LoadReportList', { "pOID": pOID });
        },

        //saveMRRHeadOffice: function (mMr, loadAppPurchaseOrderDtlss, deleteDtlsID) {
        //    debugger
        //    return $http.post('/MRRHeadOffice/SaveMRRHead', { "mRr": mMr, "mRrDtls": loadAppPurchaseOrderDtlss, "deletepDtlsID": deleteDtlsID });
        //},
        loadMRRHeadOffice: function (mRRID, param) {
            return $http.post('/MRRHeadOffice/LoadMRRHead', { "mRRID": mRRID, "param": param });
        },
        //================================ Stock List ==========================================================
        
        loadStockByStore: function (storeID) {
            return $http.post('/CurrentStock/LoadByStore', { "storeID": storeID });
        },
        loadStockByWarehouse: function (warehouseID) {
            return $http.post('/CurrentStock/LoadByWarehouse', { "warehouseID": warehouseID });
        },
        loadStock: function (branchID, categoryID, Namevalue, Pageindex, Pagesize) {
            return $http.post('/CurrentStock/LoadStock2', { "branchID": branchID, "categoryID": categoryID, "Namevalue": Namevalue, "Pageindex": Pageindex, "Pagesize": Pagesize });
        },
        //==================================== LoginBranchID ====================================================
        
        loadLoginBranchID: function () {
            return $http.post('/StoreRequisition/LoadLoginBranchID');
        },
        loadStockQty: function (BranchID, CategoryID, ProductID, UnitID) {
            return $http.post('/StoreRequisition/LoadStockQty', { "BranchID": BranchID, "CategoryID": CategoryID, "ProductID": ProductID, "UnitID": UnitID });
        },
        //================================ MATERIAL RECEIVE In Store ==========================================================
    
        loadMRSDtls: function (mrrID) {
            return $http.post('/MRS/LoadMRSDtls', { "mrrid": mrrID });
        },
        loadMRSDtlsList: function (mrrid) {
            return $http.post('/MRS/LoadMRSDtlsFrList', { "mrrid": mrrid });
        },
        loadMRSDtlsFrReturn: function (pOID) {
            return $http.post('/MRS/LoadMRSDtlsForReturn', { "pOID": pOID });
        },
        saveMRS: function (mRs, loadAppPurchaseOrderDtlss, deleteDtlsID) {
            debugger
            return $http.post('/MRS/SaveMRS', { "mRr": mRs, "mRrDtls": loadAppPurchaseOrderDtlss, "deletepDtlsID": deleteDtlsID });
        },
        loadMRS: function (mRRID, param) {
            return $http.post('/MRS/LoadMRS', { "mRRID": mRRID, "param": param });
        },
        //================================ Quality Certificats ==========================================================

        loadQCDtls: function (pOID) {
            return $http.post('/QC/LoadQCDtls', { "pOID": pOID });
        },

        saveQC: function (mRs, loadAppPurchaseOrderDtlss, deleteDtlsID) {
            debugger
            return $http.post('/QC/SaveQC', { "mRr": mRs, "mRrDtls": loadAppPurchaseOrderDtlss, "deletepDtlsID": deleteDtlsID });
        },
        loadQC: function (qcID, param) {
            return $http.post('/QC/LoadQC', { "qcID": qcID, "param": param });
        },
        loadMRSForQCDtls: function (qcid) {
            return $http.post('/QC/LoadMRSDtls', { "qcid": qcid });
        },
        //============== SRNO Search =============================
        loadSRIDBySRNO: function (name) {
            return $http.post('/TechnoReports/LoadSRNOForDropdown', { "name": name });
        },
        //================== Scrap Return ====================================================================================

        saveScrapReturn: function (sreturn, sreturnDtls, deletesreturnDtlsID) {
            debugger
            return $http.post('/ScrapReturn/SaveScrapReturn', { "scrapReturn": sreturn, "scrapReturnDtls": sreturnDtls, "deletescrapReturnDtls": deletesreturnDtlsID });
        },
        loadScrapDtls: function (id) {
            return $http.post('/ScrapReturn/LoadScrapReturnDtls', { "id": id });
        },
        loadScrapReturn: function (id) {
            return $http.post('/ScrapReturn/LoadScrapReturn', { "id": id });
        },
    }
});