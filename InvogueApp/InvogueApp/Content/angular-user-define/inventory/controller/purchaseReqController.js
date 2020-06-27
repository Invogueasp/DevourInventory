angular.module('app').controller('purchaseReqController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel
    //Start :: Common Function
    $scope.removeCookies = function () {
        $scope.companyBranch = {};
        $cookieStore.put('companyBranch', $scope.companyBranch);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
    //End :: Common Function
    //Tab Section
    $scope.tab = 1;
    $scope.setTab = function (tabId) {
        $scope.tab = tabId;
    };
    $scope.isSet = function (tabId) {
        return $scope.tab === tabId;
    };
    $scope.purchaseReq = {};
    $scope.purchaseReq.RequisitionNO = "PR001";
    $scope.loadDropdowns = function () {

    }

    // pf Structure Add
    $scope.pfStructureDtls = [];
    $scope.deletePFStRowDtlsID = [];
    var indexID = 0;
    $scope.addPFStructureRow = function (details) {
        debugger
        if (details.Category != null && details.Product != null) {
            $scope.pfStructureDtls.push({
                Category: details.Category,
                Product: details.Product,
                Unit: details.Unit,
                Quantity: details.Quantity,
                Remarks: details.Remarks,
                IndexID: indexID
            });
            details.Product = null;
            details.Category = null;
            details.Unit = null;
            details.Quantity = "";
            details.Remarks = "";
            ++indexID;
        }


    }
    $scope.updateDetailsData = function (details) {
        for (var i = 0; i < $scope.pfStructureDtls.length; i++) {
            if ($scope.pfStructureDtls[i].IndexID == details.IndexID) {
                $scope.pfStructureDtls[i].Category = details.Category;
                $scope.pfStructureDtls[i].Product = details.Product;
                $scope.pfStructureDtls[i].Unit = details.Unit;
                $scope.pfStructureDtls[i].Quantity = details.Quantity;
                $scope.pfStructureDtls[i].Remarks = details.Remarks;
            }
        }
    }
    $scope.removePFStructureRow = function (index, details) {
        $scope.pfStructureDtls.splice(index, 1);
        if (details.PFStructureDtlsID > 0) {
            $scope.deletePFStRowDtlsID.push(details.PFStructureDtlsID);
        }

    }

    $scope.clearData = function () {
        debugger
        $scope.companyBranch = {};
        $scope.Reload();
        $cookieStore.put('companyBranch', $scope.companyBranch);
    }

    // Purchase Requisition approval

    // Modal Open
    $scope.isShiftModal = false;
    $scope.shiftClose = function () {
        $scope.isShiftModal = !$scope.isShiftModal;
    }
    $scope.viewSPRApprovalRow = function (row) {
        $scope.isShiftModal = !$scope.isShiftModal;
    }
    // Purchase Order

    $scope.orderReq = {};
    $scope.showReqWishData = function () {
        debugger 
        if ($scope.orderReq.RequisitionNo != "") {
            $scope.hideData = true;
        } else {
            $scope.hideData = false;
        }
        
    }

    //Current Stock
    $scope.cStock = {};
    $scope.showStockWishData = function () {
        debugger 
        if ($scope.cStock.Category > 0) {
            $scope.hideData = true;
        }
        else if ($scope.cStock.Type > 0) {
            $scope.hideData = true;
        }
        else if ($scope.cStock.WireHouse > 0) {
            $scope.hideData = true;
        }
        else if ($scope.cStock.Store > 0) {
            $scope.hideData = true;
        }
        else {
            $scope.hideData = false;
        }
    }


    

});
