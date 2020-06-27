angular.module('app').controller("unitController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.unit = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();

        $scope.unit = $cookieStore.get('editunit');

    }
    $scope.removeCookies = function () {
        $scope.unit = {};
        $cookieStore.put('editunit', $scope.unit);
    }
    $scope.clearData = function () {
        $scope.unit = {};
        $cookieStore.put('editunit', $scope.unit);
    }


    // Save Unit
    $scope.saveUnit = function () {
        debugger
        if ($scope.unitForm.$valid) {
            var saveUnit = settingRepository.saveUnit($scope.unit).then(function (response) {
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

    $scope.loadUnit = function () {
        debugger
        $scope.unitList = [];
        settingRepository.loadUnit().then(function (response) {
            if (response.data) {
                $scope.unitList = response.data;
            }
        })
    }


    $scope.editRow = function (row) {
        debugger
        $scope.unit = row;
        $cookieStore.put('editunit', $scope.unit);
        $location.path("/UnitCreate");

    }

})