angular.module('app').controller('mrReportController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    $scope.removeCookies = function () {
        debugger
        $scope.mMr = {};
        $cookieStore.put('editmMr', $scope.mMr);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }


    $scope.parameters = {};
    $scope.mMr = {};
    $scope.loadParameters = function () {
        $scope.parameters.FormDate = null;
        $scope.parameters.ToDate = null;
        $scope.loadStore();
        $scope.loadDepartment();
    }
    $scope.loadDropdowns = function () {
        debugger
        $scope.loadSpr();
        $scope.mMr.MRRDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.mMr = $cookieStore.get('editmMr');
        if ($scope.mMr.MRRID) {
            $scope.loadSprDtls();
        } else {
            $scope.getNewPersonnelCode();
            $scope.mMr.MRRDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        }
    }

    $scope.loadSpr = function () {
        debugger
        $scope.sprList = [];
        inventoryRepository.loadSpr().then(function (response) {
            if (response.data) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].SPRDate != null) {
                        var SPRDate = response.data[i].SPRDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].SPRDate = $filter('date')(SPRDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].RequiredDate != null) {
                        var RequiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequiredDate = $filter('date')(RequiredDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].FirstApproveDate != null) {
                        var FirstApproveDate = response.data[i].FirstApproveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].FirstApproveDate = $filter('date')(FirstApproveDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].ThirdApproveDate != null) {
                        var ThirdApproveDate = response.data[i].ThirdApproveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].ThirdApproveDate = $filter('date')(ThirdApproveDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].SecondApproveDate != null) {
                        var SecondApproveDate = response.data[i].SecondApproveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].SecondApproveDate = $filter('date')(SecondApproveDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].SecondApproveStatus == "2A") {
                        response.data[i].endAppBtn = true;
                    } else {
                        response.data[i].endAppBtn = false;
                    }
                }
                $scope.sprList = response.data;
            }
        })
    }

    $scope.loadSprDtls = function () {
        debugger
        $scope.sprDtls = [];
        inventoryRepository.loadSprDtls($scope.mMr.SPRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sprDtls = response.data;
                $scope.Index = 1;
                for (i = 0; i < $scope.sprDtls.length; i++) {
                    $scope.sprDtls.Index = $scope.Index;
                    $scope.Index++;
                }
            }
        })
    }

    $scope.clearData = function () {
        debugger
        $scope.mMr = {};
        $scope.Reload();
        $cookieStore.put('editmMr', $scope.mMr);
    }
    $scope.editRow = function (row) {
        debugger
        $scope.mMr = row;

        //$scope.reqWisePODtls();
        $cookieStore.put('editmMr', $scope.mMr);
        $location.path("/MRRCreate");

    }
    $scope.loadLoginBranchID = function () {
        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                $scope.mMr.BranchID = response.data.branchsID;
                $scope.mMr.UserFullName = response.data.userName;
                $scope.mMr.Department = response.data.department;
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
    $scope.mRRID = 0;
    $scope.loadmRRLists = function () {
        debugger
        $scope.mRRList = [];
        inventoryRepository.loadMRR($scope.mRRID, $scope.parameters).then(function (response) {
            if (response.data && response.data.length > 0) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].MRRDate != null) {
                        var MRRDate = response.data[i].MRRDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].MRRDate = $filter('date')(MRRDate, "dd-MMM-yyyy");
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
    

    $scope.saveMRR = function () {
        debugger
        $scope.mMr.SupplierID = 0; // by kamrul -> science no need to store about supplier
        $scope.mMr.POID = 0;
        $scope.mMr.QCID = 0;
        
        if ($scope.MRRForm.$valid) {
            var saveType = inventoryRepository.saveMRR($scope.mMr, $scope.sprDtls).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
                    debugger
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

        var url = '/Inventory/MRR/MaterialReceiveReport';
        $scope.parameters.MRRID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    //==================================================================================================
    $scope.deleteDtlsID = [];
    $scope.removeRow = function (index, details) {
        debugger
        $scope.loadAppPurchaseOrderDtlss.splice(index, 1);
        if (details.MRRDtlsID > 0) {
            $scope.deleteDtlsID.push(details.MRRDtlsID);
        }

    }

    $scope.supplierSelectByOrder = function () {
        debugger
       var list = $filter('filter')($scope.appPurchaseOrderList, function (d) { return d.POID === $scope.mMr.POID })[0];
       return  $scope.mMr.SupplierID =  list.SupplierID;

    }

    $scope.total = {};
    $scope.disabled = false;
    $scope.checkRemainQty = function (row) {
        debugger 
        $scope.total.GrandTotal = 0;
        $scope.mMr.SubTotal = 0;
        if ((row.ReceiveQtys > row.ReqQty)) {
            $scope.disabled = true;
            row.ReceiveQtys = " ";
            toastr.error("Order Quantity can not bigger then ReqQty Quantity");
            return;
        }
        if (row.UnitRate && row.ReceiveQtys) {
            debugger
            $scope.disabled = false;
            var LineTotal = (row.ReceiveQtys) * (row.UnitRate);

            for (var i = 0; i < $scope.sprDtls.length; i++) {
                if ($scope.sprDtls[i].Index == row.Index) {
                    $scope.sprDtls[i].ReceiveQty = row.ReceiveQtys;
                    $scope.sprDtls[i].UnitRate = row.UnitRate;
                    $scope.sprDtls[i].LineTotal = LineTotal;

                    //$scope.loadAppPurchaseOrderDtlss[i].GrandTotal = $scope.loadAppPurchaseOrderDtlss[i].GrandTotal + LineTotal;
                }


                if ($scope.sprDtls[i].LineTotal) {
                    $scope.total.GrandTotal = parseInt($scope.total.GrandTotal) + $scope.sprDtls[i].LineTotal;
                }

            }
            $scope.mMr.SubTotal = $scope.total.GrandTotal;
        }
            

    }
    $scope.loadmRRList = function () {
        debugger
        $scope.mRRList = [];
        inventoryRepository.loadQC().then(function (response) {
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

    //$scope.loadAppPurchase = function () {

    //    $scope.appPurchaseOrderList = [];
    //    inventoryRepository.loadAppPurchaseOrder().then(function (response) {
    //        if (response.data) {
    //            debugger
            
    //            $scope.appPurchaseOrderList = response.data;
    //        }
    //    })
    //}
    $scope.hideData = false;
    $scope.loadMRSFRQC = function () {
        debugger
        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMRSForQCDtls($scope.mMr.QCID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
      

                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];
                    if ($scope.loadAppPurchaseOrderDtlss[i].POID) {

                        $scope.mMr.POID = $scope.loadAppPurchaseOrderDtlss[i].POID;

                    }
                }

                $scope.hideData = true;
            }
        })
    }




    $scope.hideData = false;
    $scope.loadMrrDtlsForEdit = function () {

        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMrrDtls($scope.mMr.MRRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];
                    if ($scope.loadAppPurchaseOrderDtlss[i].ReceiveQty) {

                        $scope.loadAppPurchaseOrderDtlss[i].ReceiveQtys = $scope.loadAppPurchaseOrderDtlss[i].ReceiveQty;

                    }
                }
                $scope.hideData = true;
            }
        })
    }
    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_MRR";
        var fieldName = "MRRNO";
        var prefix = "MRR";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.mMr.MRRNO = response.data.message;
            }
        })
    }

});