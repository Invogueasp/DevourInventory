angular.module('app').controller("storeBinController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.storeBin = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        debugger
        $scope.statusList = commonRepository.getAStatus();
        $scope.storeBin.Status = $scope.statusList[0].id;
        $scope.loadStore();
        $scope.loadStoreRack();
        $scope.storeBin = $cookieStore.get('editstoreBin');

    }
    $scope.removeCookies = function () {
        $scope.storeBin = {};
        $cookieStore.put('editstoreBin', $scope.storeBin);
    }
    $scope.clearData = function () {
        $scope.storeBin = {};
        $cookieStore.put('editstoreBin', $scope.storeBin);
    }


    // saveStoreBin
    $scope.saveStoreBin = function () {
        debugger
        if ($scope.storeBinForm.$valid) {
            var saveType = settingRepository.saveStoreBin($scope.storeBin).then(function (response) {
                if (response.data.isSucess) {
                    debugger
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

    $scope.loadStoreRack = function () {
        debugger
        $scope.storeRackList = [];
        settingRepository.loadStoreRack().then(function (response) {
            if (response.data) {
                $scope.storeRackList = response.data;
            }
        })
    }
    $scope.loadStoreBin = function () {
        debugger
        $scope.storeBinList = [];
        settingRepository.loadStoreBin().then(function (response) {
            if (response.data) {
                $scope.storeBinList = response.data;
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
    $scope.editRow = function (row) {
        debugger
        $scope.storeBin = row;
        $cookieStore.put('editstoreBin', $scope.storeBin);
        $location.path("/StoreBinCreate");

    }

})