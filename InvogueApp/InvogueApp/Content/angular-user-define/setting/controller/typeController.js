angular.module('app').controller("typeController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.type = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();

        $scope.type = $cookieStore.get('edittype');

    }
    $scope.removeCookies = function () {
        $scope.type = {};
        $cookieStore.put('edittype', $scope.type);
    }
    $scope.clearData = function () {
        $scope.type = {};
        $cookieStore.put('edittype', $scope.type);
    }


    // Save Unit
    $scope.saveType = function () {
        debugger
        if ($scope.typeForm.$valid) {
            var saveType = settingRepository.saveType($scope.type).then(function (response) {
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

    $scope.loadType = function () {
        debugger
        $scope.typeList = [];
        settingRepository.loadType().then(function (response) {
            if (response.data) {
                $scope.typeList = response.data;
            }
        })
    }


    $scope.editRow = function (row) {
        debugger
        $scope.type = row;
        $cookieStore.put('edittype', $scope.type);
        $location.path("/TypeCreate");

    }

})