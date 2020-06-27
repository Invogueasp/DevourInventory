angular.module('app').controller('mrInStoreController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

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
        if ($scope.mRs.MaterialReceiveID) {
            $scope.loadAppPurchaseOrderDtlsList();
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
        $location.path("/MRSCreate");

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
    $scope.mRRID = 0;
    $scope.loadmRRList = function () {
        debugger
        $scope.mRRList = [];
        inventoryRepository.loadMRS($scope.mRRID, $scope.parameters).then(function (response) {
            if (response.data && response.data.length > 0) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].ReceiveDate != null) {
                        var ReceiveDate = response.data[i].ReceiveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].ReceiveDate = $filter('date')(ReceiveDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].Status == "F" || response.data[i].Status=="A") {

                        response.data[i].end3AppBtn = true;
                    } else {
                        response.data[i].end3AppBtn = false;
                    }



                }
                $scope.mRRList = response.data;
            }
        })
    }


    $scope.saveMRS = function () {
        debugger
        if ($scope.MRRForm.$valid) {
            $scope.mRs.POID = $scope.loadAppPurchaseOrderDtlss[0].POID;
            var saveType = inventoryRepository.saveMRS($scope.mRs, $scope.loadAppPurchaseOrderDtlss, $scope.deleteDtlsID).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
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
    $scope.deleteDtlsID = [];
    $scope.removeRow = function (index, details) {
        debugger
        $scope.loadAppPurchaseOrderDtlss.splice(index, 1);
        if (details.ReceiveDtlsID > 0) {
            $scope.deleteDtlsID.push(details.ReceiveDtlsID);
        }

    }



    $scope.total = {};

    $scope.checkRemainQty = function (row) {        
        if (row.MRSReceiveQty > row.SentQty) {
            toastr.error("Receive quantity can not bigger then Sent quantity");
            row.MRSReceiveQty = null;
        } else {

            for (var i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                if ($scope.loadAppPurchaseOrderDtlss[i].Index == row.Index) {
                    $scope.loadAppPurchaseOrderDtlss[i].CategoryID = row.CategoryID;
                    $scope.loadAppPurchaseOrderDtlss[i].ProductID = row.ProductID;
                    $scope.loadAppPurchaseOrderDtlss[i].UnitID = row.UnitID;
                    $scope.loadAppPurchaseOrderDtlss[i].ReceiveQty = row.MRSReceiveQty;
                    $scope.loadAppPurchaseOrderDtlss[i].PODtlsID = row.PODtlsID;

                }

            }

        }

    }
    $scope.loadAppPurchase = function () {

        $scope.appPurchaseOrderList = [];
        inventoryRepository.loadMRRHeadOffice().then(function (response) {
            if (response.data) {
                $scope.appPurchaseOrderList = response.data;
            }
        })
    }
    //$scope.hideData = false;
    $scope.loadAppPurchaseOrderDtls = function () {
        debugger
        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMrrHODtlsForMRS($scope.mRs.MRRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];
                }
                //$scope.hideData = true;
            }
        })
    }
    $scope.loadAppPurchaseOrderDtlsList = function () {

        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMRSDtlsList($scope.mRs.MaterialReceiveID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];
                }
                //$scope.hideData = true;
            }
        })
    }
    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_MaterialReceive";
        var fieldName = "ReceiveNO";
        var prefix = "MRS";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.mRs.ReceiveNO = response.data.message;
            }
        })
    }

});