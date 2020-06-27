angular.module('app').controller("categoryController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.category = {};
    $scope.statusList = [];

    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();
        $scope.category = $cookieStore.get('editcategory');

    }
    $scope.removeCookies = function () {
        $scope.category = {};
        $cookieStore.put('editcategory', $scope.category);
    }
    $scope.clearData = function () {
        $scope.category = {};
        $cookieStore.put('editcategory', $scope.category);
    }


    // saveCategory
    $scope.saveCategory = function () {
        debugger
        if ($scope.categoryForm.$valid) {
            var saveCategory = settingRepository.saveCategory($scope.category).then(function (response) {
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

    $scope.loadCategory = function () {
        debugger
        $scope.categoryList = [];
        settingRepository.loadCategory().then(function (response) {
            if (response.data) {
                $scope.categoryList = response.data;
            }
        })
    }
    $scope.editRow = function (row) {
        debugger
        $scope.category = row;
        $cookieStore.put('editcategory', $scope.category);
        $location.path("/CategoryCreate");
    
    }

})