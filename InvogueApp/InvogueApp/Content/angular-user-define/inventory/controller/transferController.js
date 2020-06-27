angular.module('app').controller('transferController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

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
        debugger

        $scope.transfer = {};
        $cookieStore.put('edittransfer', $scope.transfer);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }


    $scope.transfer = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        debugger
        $scope.loadStore();
        $scope.loadCategory();
        $scope.loadUnit();
        $scope.loadProducts();
        $scope.statusList = commonRepository.getAStatus();
        


        $scope.transfer = $cookieStore.get('edittransfer');
        if ($scope.transfer.TransferID) {
            $scope.loadTransferDtls();
        }
        else {
            $scope.getNewPersonnelCode();
        }
    }
    $scope.clearData = function () {
        debugger
        $scope.transfer = {};
        $scope.Reload();
        $cookieStore.put('edittransfer', $scope.transfer);
    }

    //  store REQ Add
    $scope.transferDtls = [];
    $scope.deleteTransferDtlsID = [];
    var indexID = 0;
    $scope.addStoreReqRow = function (details) {
        debugger
        if (details.CategoryID != null && details.ProductID != null) {
            $scope.transferDtls.push({
                CategoryID: details.CategoryID,
                ProductID: details.ProductID,
                UnitID: details.UnitID,
                TransferQty: details.TransferQty,
                Remarks: details.Remarks,
                IndexID: indexID
            });
            details.ProductID = null;
            details.CategoryID = null;
            details.UnitID = null;
            details.TransferQty = "";
            details.Remarks = "";
            ++indexID;
        }


    }
    $scope.updateDetailsData = function (details) {
        for (var i = 0; i < $scope.transferDtls.length; i++) {
            if ($scope.transferDtls[i].IndexID == details.IndexID) {
                $scope.transferDtls[i].CategoryID = details.CategoryID;
                $scope.transferDtls[i].ProductID = details.ProductID;
                $scope.transferDtls[i].UnitID = details.UnitID;
                $scope.transferDtls[i].TransferQty = details.TransferQty;
                $scope.transferDtls[i].Remarks = details.Remarks;
            }
        }
    }
    $scope.removeStoreReqRow = function (index, details) {
        debugger
        $scope.transferDtls.splice(index, 1);
        if (details.TransferDtlsID > 0) {
            $scope.deleteTransferDtlsID.push(details.TransferDtlsID);
        }

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
    $scope.loadTransfer = function () {

        $scope.transferList = [];
        inventoryRepository.loadTransfer().then(function (response) {
            if (response.data) {
                debugger

                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].TransferDate != null) {
                        var TransferDate = response.data[i].TransferDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].TransferDate = $filter('date')(TransferDate, "dd-MMM-yyyy");
                    }
                
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                }
                $scope.transferList = response.data;
            }
        })
    }
    $scope.editRow = function (row) {
        debugger
        $scope.transfer = row;
        $cookieStore.put('edittransfer', $scope.transfer);
        $location.path("/TransferCreate");

    }

    $scope.saveTransfer = function () {
        debugger
        if ($scope.transferForm.$valid) {
            var saveType = inventoryRepository.saveTransfer($scope.transfer, $scope.transferDtls, $scope.deleteTransferDtlsID).then(function (response) {
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

    $scope.loadTransferDtls = function () {
        debugger
        //$scope.storeDtlsList = [];
        inventoryRepository.loadTransferDtls($scope.transfer.TransferID).then(function (response) {
            if (response.data) {
                debugger
                $scope.transferDtls = response.data;
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


    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_Transfer";
        var fieldName = "TransferNO";
        var prefix = "TRN";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.transfer.TransferNO = response.data.message;
            }
        })
    }

});