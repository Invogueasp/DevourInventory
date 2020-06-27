angular.module('app').controller('transferAppController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {


    $scope.transferApp = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.transferApp = $cookieStore.get('edittransferApp');
        $scope.loadStore();
        $scope.loadCategory();
        $scope.loadProduct();
        $scope.loadUnit();

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
    $scope.viewTransApprovalRow = function (row) {
        debugger
        if (row.Status == "P") {
            $scope.button = "Pending";
        }

        if (row.Status == "A") {
            $scope.button = "Approved";
        }

        $scope.isModal = !$scope.isModal;
        $scope.transferApp = row;
        $cookieStore.put('edittransferApp', $scope.transferApp);
        $scope.loadTransferDtls();
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

    $scope.loadTransferDtls = function () {
        debugger
        //$scope.storeDtlsList = [];
        inventoryRepository.loadTransferDtls($scope.transferApp.TransferID).then(function (response) {
            if (response.data) {
                debugger
                $scope.transferDtls = response.data;
            }
        })
    }



    $scope.transferAppDtlss = [];
    $scope.apptransferRemerks = function (row) {
        debugger
        $scope.transferAppDtlss.push(row);
    }

    $scope.saveTransferApp = function () {
        debugger
        if ($scope.transferdtlsForm.$valid) {
            var saveType = inventoryRepository.saveTransferApp($scope.transferApp, $scope.transferDtls, $scope.deleteTransferDtlsID).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
                    $scope.modalClose();
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
    $scope.loadStore = function () {
        debugger
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.storeList = response.data;
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



});