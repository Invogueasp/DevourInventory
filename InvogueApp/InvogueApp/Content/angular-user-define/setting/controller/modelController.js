angular.module('app').controller("modelController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.model = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();
        $scope.loadMachine();
        $scope.model = $cookieStore.get('model');

    }
    $scope.removeCookies = function () {
        $scope.model = {};
        $cookieStore.put('model', $scope.model);
    }
    $scope.clearData = function () {
        $scope.model = {};        
        $cookieStore.put('machine', $scope.model);
    }


    // Save Warehouse
    $scope.saveModel = function () {
        debugger
        if ($scope.modelForm.$valid) {
            var saveType = settingRepository.saveModel($scope.model).then(function (response) {
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

    $scope.loadMachine = function () {
        debugger
        $scope.machineList = [];
        settingRepository.loadMachine().then(function (response) {
            if (response.data) {
                $scope.machineList = response.data;
            }
        })
    }

    $scope.loadModel = function () {
        debugger
        $scope.modelList = [];
        settingRepository.loadModel().then(function (response) {
            if (response.data) {
                $scope.modelList = response.data;
            }
        })
    }
    $scope.editRow = function (row) {
        debugger
        $scope.model = row;
        $cookieStore.put('model', $scope.model);
        $location.path("/ModelCreate");

    }

})