angular.module('app').controller('pOrderApprovalController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    $scope.removeCookies = function () {
        debugger
        $scope.purchaseOrder = {};
        $cookieStore.put('editpurchaseOrder', $scope.purchaseOrder);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }


    $scope.purchaseOrder = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        debugger
        $scope.loadStore();
        $scope.purchaseOrder = $cookieStore.get('editpurchaseOrder');
        $scope.loadLoginBranchID();
        if ($scope.purchaseOrder.POID) {
            $scope.reqWisePODtls();
        }
        
        $scope.loadSupplier();
    }
    $scope.isModal = false;
    $scope.modalClose = function () {
        debugger
        $scope.isModal = !$scope.isModal;

    }
    $scope.viewPOApprovalRow = function (row) {
        debugger
        if (row.SecondApproveStatus == "2A" || row.SecondApproveStatus == "F") {
            $scope.endAppBtn = false;

        } else {
            $scope.endAppBtn = true;
        }
        $scope.loadLoginBranchID();
        $scope.isModal = !$scope.isModal;
        $scope.purchaseOrder = row;
        $cookieStore.put('editpurchaseOrder', $scope.purchaseOrder);
        $scope.reqWisePODtls();
    }
    $scope.clearData = function () {
        debugger
        $scope.purchaseOrder = {};
        $scope.Reload();
        $cookieStore.put('editpurchaseOrder', $scope.purchaseOrder);
    }


    $scope.loadStore = function () {
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.storeList = response.data;
            }
        })
    }

    $scope.editRow = function (row) {
        debugger
        $scope.purchaseOrder = row;
        $cookieStore.put('editpurchaseOrder', $scope.purchaseOrder);
        $location.path("/PurchaseOrderCreate");

    }
    $scope.loadLoginBranchID = function () {
        debugger

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                debugger
                $scope.purchaseOrder.BranchID = response.data.branchsID;
                $scope.purchaseOrder.UserFullName = response.data.userName;
            }
        })
    }
    $scope.savePurchasOrder = function () {
        debugger
        //if ($scope.PoForm.$valid) {
        var saveType = inventoryRepository.saveAppPurchase($scope.purchaseOrder, $scope.sprDtls, $scope.deleteSPRDtlsID).then(function (response) {
                if (response.data.isSucess) {
                    
                    toastr.success(response.data.message);
                    $scope.loadPurchase();
                    $scope.modalClose();
                } else {
                    if (response.data.message == "LogOut") {
                        $location.path('#!/Login')
                    }
                    toastr.error(response.data.message);
                }
            })
        //} else {
        //    toastr.error("Please fill-up all required field !!!");
        //}
    }

    $scope.loadSupplier = function () {

        $scope.supplierList = [];
        settingRepository.loadSupplier().then(function (response) {
            if (response.data) {
                debugger
                $scope.supplierList = response.data;
            }
        })
    }

    $scope.loadPurchase = function () {

        $scope.purchaseOrderList = [];
        inventoryRepository.loadPurchaseOrderApp().then(function (response) {
            if (response.data) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].PODate != null) {
                        var PODate = response.data[i].PODate.replace('/Date(', '').replace(')/', '');
                        response.data[i].PODate = $filter('date')(PODate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].DueDate != null) {
                        var DueDate = response.data[i].DueDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].DueDate = $filter('date')(DueDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].SecondApproveDate != null) {
                        var SecondApproveDate = response.data[i].SecondApproveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].SecondApproveDate = $filter('date')(SecondApproveDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].FirstApproveDate != null) {
                        var FirstApproveDate = response.data[i].FirstApproveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].FirstApproveDate = $filter('date')(FirstApproveDate, "dd-MMM-yyyy");
                    }

                }
                $scope.purchaseOrderList = response.data;
            }
        })
    }
    $scope.reqWisePODtls = function () {
        debugger
        $scope.sprDtls = [];
        inventoryRepository.loadPurchaseOrderDtls($scope.purchaseOrder.POID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sprDtls = response.data;


                for (i = 0; i < $scope.sprDtls.length; i++) {

                    if ($scope.sprDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.purchaseOrder.BranchID, $scope.sprDtls[i].CategoryID, $scope.sprDtls[i].ProductID, $scope.sprDtls[i].UnitID).then(function (response) {
                            if (response.data) {
                                debugger
                                $scope.Id = response.data.ProductID;
                                for (j = 0; j < $scope.sprDtls.length; j++) {
                                    if ($scope.Id == $scope.sprDtls[j].ProductID) {
                                        $scope.sprDtls[j].StockQty = response.data.Quantity;
                                    }

                                }

                                //$scope.storeReqDtls[i].StockQty = 5;
                            }
                        })



                    }
                }

            }
        })
    }

    $scope.approveQty = function (row) {
        $scope.disabledButton = false;
        if (row.ApprovedQty > row.OrderQty) {
            $scope.disabledButton = true;
            toastr.error("Approved Quantity Must Be Less Than Or Equal REQ. Quantity");
            return;
        }
        for (var i = 0; i < $scope.sprDtls.length; i++) {
            if ($scope.sprDtls[i].ProductID == row.ProductID) {
                $scope.sprDtls[i].CategoryID = row.CategoryID;
                $scope.sprDtls[i].ProductID = row.ProductID;
                $scope.sprDtls[i].UnitID = row.UnitID;
                $scope.sprDtls[i].ApprovedQty = row.ApprovedQty;
                $scope.sprDtls[i].OrderQty = row.OrderQty;
            }
        }

    }
    //========================================== 2nd Approval =================================================

    $scope.savePO2ndApp= function () {
        debugger
        //if ($scope.PoForm.$valid) {
        var saveType = inventoryRepository.savePO2ndApp($scope.purchaseOrder, $scope.sprDtls, $scope.deleteSPRDtlsID).then(function (response) {
            if (response.data.isSucess) {

                toastr.success(response.data.message);
                $scope.loadPurchase();
                $scope.modalClose();
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('#!/Login')
                }
                toastr.error(response.data.message);
            }
        })
        //} else {
        //    toastr.error("Please fill-up all required field !!!");
        //}
    }
    $scope.loadPO2ndApp = function () {

        $scope.purchaseOrderLists = [];
        inventoryRepository.loadPO2ndApp().then(function (response) {
            if (response.data) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].PODate != null) {
                        var PODate = response.data[i].PODate.replace('/Date(', '').replace(')/', '');
                        response.data[i].PODate = $filter('date')(PODate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].DueDate != null) {
                        var DueDate = response.data[i].DueDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].DueDate = $filter('date')(DueDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].SecondApproveDate != null) {
                        var SecondApproveDate = response.data[i].SecondApproveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].SecondApproveDate = $filter('date')(SecondApproveDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].FirstApproveDate != null) {
                        var FirstApproveDate = response.data[i].FirstApproveDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].FirstApproveDate = $filter('date')(FirstApproveDate, "dd-MMM-yyyy");
                    }
                

                }
                $scope.purchaseOrderLists = response.data;
            }
        })
    }
});