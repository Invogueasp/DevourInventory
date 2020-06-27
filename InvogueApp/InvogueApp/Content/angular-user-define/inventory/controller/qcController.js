angular.module('app').controller('qcController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    $scope.removeCookies = function () {
        debugger
        $scope.mRs = {};
        $cookieStore.put('editmMr', $scope.mRs);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }


    $scope.parameters = {};
    $scope.mRs = {};
    $scope.loadParameters = function () {
        $scope.parameters.FormDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.parameters.ToDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.loadStore();
        $scope.loadDepartment();
    }
    $scope.loadDropdowns = function () {
        debugger
        $scope.loadLoginBranchID();
        $scope.loadStore();
        $scope.loadAppPurchase();
        $scope.mRs = $cookieStore.get('editmMr');
        if ($scope.mRs.QCID) {
             $scope.loadMRSFRQC();
        } else {
            $scope.getNewPersonnelCode();
        }
    }
    $scope.clearData = function () {
        debugger
        $scope.mRs = {};
        $scope.Reload();
        $cookieStore.put('editmMr', $scope.mRs);
    }
    $scope.editRow = function (row) {
        debugger
        $scope.mRs = row;

        //$scope.reqWisePODtls();
        $cookieStore.put('editmMr', $scope.mRs);
        $location.path("/QCCreate");

    }
    $scope.loadLoginBranchID = function () {
        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                $scope.mRs.BranchID = response.data.branchsID;
                $scope.mRs.UserFullName = response.data.userName;
                $scope.mRs.Department = response.data.department;
            }
        })
    }
    $scope.loadDepartment = function () {

        $scope.deptList = [];
        settingRepository.loadDepartment().then(function (response) {
            if (response.data) {
                $scope.deptList = response.data;
            }
        })
    }
    $scope.loadStore = function () {
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.storeList = response.data;
            }
        })
    }
    $scope.qcID = 0;
    $scope.loadmRRList = function () {
        debugger
        $scope.mRRList = [];
        inventoryRepository.loadQC($scope.qcID, $scope.parameters).then(function (response) {
            if (response.data && response.data.length > 0) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].QCDate != null) {
                        var QCDate = response.data[i].QCDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].QCDate = $filter('date')(QCDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                }
                $scope.mRRList = response.data;
            }
        })
    }


    $scope.saveQC = function () {
        debugger
        if ($scope.MRRForm.$valid) {
            var saveType = inventoryRepository.saveQC($scope.mRs, $scope.loadAppPurchaseOrderDtlss, $scope.deleteDtlsID).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
                    $scope.reportView(response.data.lastInsertedID);
                    $scope.clearData();
                } else {
                    if (response.data.message == "LogOut") {
                        $location.path('#!/Login')
                    }
                    toastr.error(response.data.message);
                }
            })
        } else {
            toastr.error("Please fill-up all required field !!!");
        }
    }
    //===================================== MRR Report ==================================================
    $scope.parameters = {};
    $scope.reportView = function (row) {

        var url = '/Inventory/QC/QCReportView';
        $scope.parameters.QCID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    //==================================================================================================
    $scope.deleteDtlsID = [];
    $scope.removeRow = function (index, details) {
        debugger
        $scope.loadAppPurchaseOrderDtlss.splice(index, 1);
        if (details.QCDtlsID > 0) {
            $scope.deleteDtlsID.push(details.QCDtlsID);
        }

    }



    $scope.total = {};
    $scope.RemainFailQty = 0;
    $scope.checkRemainQty = function (row) {
        $scope.mRs.MaterialRecordID = row.MaterialReceiveID;
        debugger 
        if (row.QCPassQty > row.ReceiveQty) {
            toastr.error("QC Pass Quantity can not bigger then Received Quantity");
            row.QCPassQty = null;
            row.QCFailQty = null;
        }
        else if (row.QCPassQty == '') {
            row.QCFailQty = null;
        }
        else {

            if (row.QCPassQty) {
                row.QCFailQty = (parseFloat(row.ReceiveQty) - parseFloat(row.QCPassQty));
            }
           
            for (var i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                if ($scope.loadAppPurchaseOrderDtlss[i].Index == row.Index) {
                    $scope.loadAppPurchaseOrderDtlss[i].CategoryID = row.CategoryID;
                    $scope.loadAppPurchaseOrderDtlss[i].ProductID = row.ProductID;
                    $scope.loadAppPurchaseOrderDtlss[i].UnitID = row.UnitID;
                    $scope.loadAppPurchaseOrderDtlss[i].MRSReceiveQty = row.MRSReceiveQty;
                    $scope.loadAppPurchaseOrderDtlss[i].ReceiveQty = row.ReceiveQty;
                    $scope.loadAppPurchaseOrderDtlss[i].PODtlsID = row.PODtlsID;
                    $scope.loadAppPurchaseOrderDtlss[i].QCPassQty = row.QCPassQty;
                    $scope.loadAppPurchaseOrderDtlss[i].Remarks = row.Remarks;
                    $scope.loadAppPurchaseOrderDtlss[i].QCFailQty = row.QCFailQty;

                } 

            }

        }

    }
    $scope.loadAppPurchase = function () {

        $scope.appPurchaseOrderList = [];
        inventoryRepository.loadMRS().then(function (response) {
            if (response.data) {
                debugger

                $scope.appPurchaseOrderList = response.data;
            }
        })
    }
    //$scope.hideData = false;
    
    $scope.loadMRSFRQC = function () {
        debugger
        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMRSForQCDtls($scope.mRs.QCID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];
                    if ($scope.loadAppPurchaseOrderDtlss[i].POID) {

                        $scope.mRs.POID = $scope.loadAppPurchaseOrderDtlss[i].POID;

                    }


                }
                
            }
        })
    }
    $scope.loadAppPurchaseOrderDtls = function () {
        debugger
        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMRSDtls($scope.mRs.MRRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                $scope.mRs.MaterialReceiveID = response.data[0].MaterialReceiveID;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];
                }
                //$scope.hideData = true;
            }
        })
    }

    $scope.loadStoreReceiveDtls = function () {
        debugger 
        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMRSDtls($scope.mRs.MaterialReceiveID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                //$scope.mRs.MaterialReceiveID = response.data[0].MaterialReceiveID;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];
                }
                //$scope.hideData = true;
            }
        })
    }

    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_QC";
        var fieldName = "QCNO";
        var prefix = "QCN";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.mRs.QCNO = response.data.message;
            }
        })
    }

});