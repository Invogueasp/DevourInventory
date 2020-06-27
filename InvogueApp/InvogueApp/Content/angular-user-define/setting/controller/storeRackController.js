angular.module('app').controller("storeRackController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.storeRack = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();
        $scope.loadStore();
        $scope.storeRack = $cookieStore.get('editstoreRack');

    }
    $scope.removeCookies = function () {
        $scope.storeRack = {};
        $cookieStore.put('editstoreRack', $scope.storeRack);
    }
    $scope.clearData = function () {
        $scope.storeRack = {};
        $cookieStore.put('editstoreRack', $scope.storeRack);
    }


    // Save Warehouse
    $scope.saveStoreRack = function () {
        debugger
        if ($scope.storeRackForm.$valid) {
            var saveType = settingRepository.saveStoreRack($scope.storeRack).then(function (response) {
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
        $scope.storeRack = row;
        $cookieStore.put('editstoreRack', $scope.storeRack);
        $location.path("/StoreRackCreate");

    }

})