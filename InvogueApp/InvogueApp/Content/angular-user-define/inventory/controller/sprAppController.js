angular.module('app').controller('sprAppController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel
    //Start :: Common Function

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
  
    $scope.viewTransApprovalRow = function (row) {
        debugger
        if (row.SecondApproveStatus == "2A" && row.SecondApproveStatus!=null) {
            $scope.endAppBtn = false;
            $scope.endAppDBtn = true;
        } else {
            $scope.endAppBtn = true;
            $scope.endAppDBtn = false;
        }

        if (row.ThirdApproveStatus == "3A" && row.ThirdApproveStatus != null || row.ThirdApproveStatus == "F") {
            $scope.end3AppBtn = false;
            
        } else {
            $scope.end3AppBtn = true;
        }

        $scope.isModal = !$scope.isModal;
        $scope.storePR = row;
        $cookieStore.put('editstorePRApp', $scope.storePR);
        $scope.loadSprDtls();
    }
    $scope.storePR = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        
        $scope.loadStore();
       
    }
    $scope.clearData = function () {
        debugger
        $scope.storePR = {};
        $scope.Reload();
        $cookieStore.put('editstorePR', $scope.storePR);
    }


    $scope.deleteSPRDtlsID = [];
    $scope.removeStoreReqRow = function (index, details) {
        debugger
        $scope.sprDtls.splice(index, 1);
        if (details.SPRDtlsID > 0) {
            $scope.deleteSPRDtlsID.push(details.SPRDtlsID);
        }

    }

    $scope.menuCount = {};
    $scope.checkCountData = function() {
        $http.get('/Security/GetAllCountData').then(function (response) {
            debugger;
            $scope.menuCount.sprCountData = response.data.sprApprovalCountData;
        });
    }
    

    $scope.loadStore = function () {        
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    debugger
                }
                $scope.storeList = response.data;
            }
        })
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
                    //if (response.data[i].FirstApproveStatus == null) {
                    //    $scope.myObj = {
                    //        "font-weight": "bold !important"
                    //    }
                    //}
                }
                $scope.sprList = response.data;
            }
        })
    }
    $scope.saveSprApp = function () {
        
        if ($scope.sprAppForm.$valid) {
            var saveType = inventoryRepository.saveSPRApp($scope.storePR, $scope.sprDtls, $scope.deleteSPRDtlsID).then(function (response) {
                if (response.data.isSucess) {
                    debugger
                    $scope.loadSpr();
                    toastr.success(response.data.message);
                    $scope.modalClose();
                    //$scope.checkCountData();
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

    $scope.loadSprDtls = function () {
        debugger
        $scope.sprDtls = [];
        inventoryRepository.loadSprDtls($scope.storePR.SPRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sprDtls = response.data;
                for (i = 0; i < $scope.sprDtls.length; i++) {

                    if ($scope.sprDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.storePR.BranchID, $scope.sprDtls[i].CategoryID, $scope.sprDtls[i].ProductID, $scope.sprDtls[i].UnitID).then(function (response) {
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


    $scope.loadProducts = function () {
        debugger
        $scope.productLists = [];
        settingRepository.loadProduct().then(function (response) {
            if (response.data) {
                $scope.productLists = response.data;
            }
        })
    }


    $scope.loadProduct = function (id) {
        debugger
        $scope.productList = [];
        settingRepository.loadProduct(id).then(function (response) {
            if (response.data) {
                $scope.productList = response.data;
            }
        })
    }
    $scope.loadCategory = function () {

        $scope.categoryList = [];
        settingRepository.loadCategory().then(function (response) {
            if (response.data) {
                $scope.categoryList = response.data;
            }
        })
    }

    $scope.loadUnit = function () {

        $scope.unitList = [];
        settingRepository.loadUnit().then(function (response) {
            if (response.data) {
                $scope.unitList = response.data;
            }
        })
    }
    $scope.disabledButton = false;
    $scope.approveQty = function (row) {
        $scope.disabledButton = false;
        if (row.ApprovedQty > row.ReqQty) {
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
                $scope.sprDtls[i].ReqQty = row.ReqQty;
            }
        }


    }

    //======================================================== 2nd Approval SPR ===============================================
    $scope.loadSpr2ndApp = function () {
        debugger
        $scope.sprLists = [];
        inventoryRepository.loadSpr2ndApp().then(function (response) {
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
                    if (response.data[i].FirstApproveStatus == "2A") {
                        response.data[i].endAppBtn = true;
                    } else {
                        response.data[i].endAppBtn = false;
                    }
                }
                $scope.sprLists = response.data;
            }
        })
    }
    $scope.saveSpr2ndApp = function () {
        debugger
        if ($scope.sprAppForm.$valid) {
            var saveType = inventoryRepository.saveSPR2ndApp($scope.storePR, $scope.sprDtls, $scope.deleteSPRDtlsID).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
                    $scope.loadSpr();
                    $scope.modalClose();
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
    //======================================================== 3rd Approval SPR ===============================================
    $scope.loadSpr3rdApp = function () {
        debugger
        $scope.sprList3rd = [];
        inventoryRepository.loadSpr3rdApp().then(function (response) {
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
                }
                $scope.sprList3rd = response.data;
            }
        })
    }
    $scope.saveSpr3rdApp = function () {
        debugger
        if ($scope.sprAppForm.$valid) {
            var saveType = inventoryRepository.saveSPR3rdApp($scope.storePR, $scope.sprDtls, $scope.deleteSPRDtlsID).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
                    $scope.loadSpr();
                    $scope.modalClose();
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
});