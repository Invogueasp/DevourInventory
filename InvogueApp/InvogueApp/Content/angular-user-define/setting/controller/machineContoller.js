angular.module('app').controller("machineContoller", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.machine = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();
        $scope.machine = $cookieStore.get('machine');

    }
    $scope.removeCookies = function () {
        $scope.machine = {};
        $cookieStore.put('machine', $scope.machine);
    }
    $scope.clearData = function () {
        $scope.machine = {};
        $cookieStore.put('machine', $scope.machine);
    }


    // Save Warehouse
    $scope.saveMachine = function () {
        debugger
        if ($scope.machineForm.$valid) {
            var saveType = settingRepository.saveMachine($scope.machine).then(function (response) {
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

    $scope.editRow = function (row) {
        debugger
        $scope.machine = row;
        $cookieStore.put('machine', $scope.machine);
        $location.path("/MachineCreate");

    }

})