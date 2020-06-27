angular.module('app').controller("departmentController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.dept = {};
    $scope.statusList = [];
    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();

        $scope.dept = $cookieStore.get('editdept');

    }
    $scope.removeCookies = function () {
        $scope.dept = {};
        $cookieStore.put('editdept', $scope.dept);
    }
    $scope.clearData = function () {
        $scope.dept = {};
        $cookieStore.put('editdept', $scope.dept);
    }


    // save Department
    $scope.saveDept = function () {
        debugger
        if ($scope.deptForm.$valid) {
            var saveColor = settingRepository.saveDept($scope.dept).then(function (response) {
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

    $scope.loadDepartment = function () {
        debugger
        $scope.deptList = [];
        settingRepository.loadDepartment().then(function (response) {
            if (response.data) {
                $scope.deptList = response.data;
            }
        })
    }


    $scope.editRow = function (row) {
        debugger
        $scope.dept = row;
        $cookieStore.put('editdept', $scope.dept);
        $location.path("/DepartmentCreate");

    }

})