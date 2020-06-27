angular.module('app').controller('srApprovalController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    $scope.disabledButton = false;
    $scope.storeReq = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.storeReq = $cookieStore.get('editStoreReq');
        $scope.loadStore();
      
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }

    // modal open
    $scope.isModal = false;
    $scope.modalClose = function () {
        debugger
        $scope.isModal = !$scope.isModal;

    }

    $scope.viewSRApprovalRow = function (row) {
        debugger
        if (row.Status == "2A" || row.Status == "F" || row.Status == "IF" || row.Status == "IP") {

            $scope.end3AppBtn = false;
        } else {
            $scope.end3AppBtn = true;
        }
     



        $scope.isModal = !$scope.isModal;
        $scope.storeReq = row;
        $scope.loadLoginBranchID();
        $cookieStore.put('editStoreReq', $scope.storeReq);
        $scope.loadStoreDtls();
    }
    $scope.loadStoreReq = function () {
        $scope.srID = 0;
        $scope.status = "";
        $scope.storeReqList = [];
        settingRepository.loadStoreReq($scope.srID, $scope.status).then(function (response) {
            if (response.data) {
                
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].RequisitionDate != null) {
                        var RequisitionDate = response.data[i].RequisitionDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequisitionDate = $filter('date')(RequisitionDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].RequiredDate != null) {
                        var RequiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequiredDate = $filter('date')(RequiredDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    debugger
                    response.data[i].SecondStatus = commonRepository.convertSecondStatusText(response.data[i].Status);
                }
                $scope.storeReqList = response.data;
            }
        })
    }
    $scope.loadStore2ndAppReq = function () {

        $scope.store2ndAppReqList = [];
        settingRepository.load2ndAppStoreReq().then(function (response) {
            if (response.data) {
                debugger

                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].RequisitionDate != null) {
                        var RequisitionDate = response.data[i].RequisitionDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequisitionDate = $filter('date')(RequisitionDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].RequiredDate != null) {
                        var RequiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequiredDate = $filter('date')(RequiredDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    response.data[i].SecondStatus = commonRepository.convertSecondStatusText(response.data[i].Status);
                }
                $scope.store2ndAppReqList = response.data;
            }
        })
    }
    $scope.storeReqDtlss = [];
    $scope.approveQty = function (row) {
        $scope.disabledButton = false;
        if (row.ApprovedQty > row.ReqQty) {
            $scope.disabledButton = true;
            toastr.error("Approved Quantity Must Be Less Than Or Equal REQ. Quantity");
            return;
        }
        for (var i = 0; i < $scope.storeReqDtls.length; i++) {
            if ($scope.storeReqDtls[i].ProductID == row.ProductID) {
                $scope.storeReqDtls[i].CategoryID = row.CategoryID;
                $scope.storeReqDtls[i].ProductID = row.ProductID;
                $scope.storeReqDtls[i].UnitID = row.UnitID;
                $scope.storeReqDtls[i].ApprovedQty = row.ApprovedQty;
                $scope.storeReqDtls[i].ReqQty = row.ReqQty;
            }
        }
    }

    $scope.saveStoreReq = function () {
        debugger
        var saveType = inventoryRepository.saveStoreReqApp($scope.storeReq, $scope.storeReqDtls, $scope.deleteStoreReqDtlsID).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
                    $scope.loadStoreReq();
                    $scope.modalClose();
                  

                  
                } else {
                    if (response.data.message == "LogOut") {
                        $location.path('#!/Login')
                    }
                    toastr.error(response.data.message);
                }
            })
        
    }

    $scope.SecondappsaveStoreReq = function () {
        debugger
        var saveType = inventoryRepository.secondAppStoreReqApp($scope.storeReq, $scope.storeReqDtls, $scope.deleteStoreReqDtlsID).then(function (response) {
            if (response.data.isSucess) {

                toastr.success(response.data.message);
                $scope.modalClose();
                $scope.loadStoreReq();


            } else {
                if (response.data.message == "LogOut") {
                    $location.path('#!/Login')
                }
                toastr.error(response.data.message);
            }
        })

    }



    $scope.loadStore = function () {
        debugger
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.storeList = response.data;
            }
        })
    }
    $scope.loadStoreDtls = function () {
        debugger
        settingRepository.loadStoreReqDtls($scope.storeReq.SRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.storeReqDtls = response.data;

                for (i = 0; i < $scope.storeReqDtls.length; i++) {

                    if ($scope.storeReqDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.storeReq.BranchID, $scope.storeReqDtls[i].CategoryID, $scope.storeReqDtls[i].ProductID, $scope.storeReqDtls[i].UnitID).then(function (response) {
                            if (response.data) {
                                debugger
                               $scope.Id= response.data.ProductID;
                               for (j = 0; j < $scope.storeReqDtls.length; j++) {
                                   if ($scope.Id == $scope.storeReqDtls[j].ProductID) {
                                       $scope.storeReqDtls[j].StockQty = response.data.Quantity;
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

    //$scope.loadStockQty = function (row) {
    //    debugger
    //    inventoryRepository.loadStockQty($scope.storeReq.BranchID, row.CategoryID, row.ProductID, row.UnitID).then(function (response) {
    //        if (response.data) {
    //            debugger
    //            $scope.details.Quantity = response.data.Quantity;
    //        }
    //    })


    //}
    $scope.loadLoginBranchID = function () {
        debugger

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                debugger
             
                $scope.storeReq.UserName = response.data.userName;
            }
        })
    }


});