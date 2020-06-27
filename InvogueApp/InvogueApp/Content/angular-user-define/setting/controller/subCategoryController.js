angular.module('app').controller("subCategoryController", function ($scope, $location, $cookieStore, settingRepository, commonRepository) {
    $scope.subCategory = {};
    $scope.statusList = [];

    $scope.loadDropdowns = function () {
        $scope.loadCategory();
        $scope.statusList = commonRepository.getAStatus();
        debugger
       
        $scope.subCategory = $cookieStore.get('editsubCategory');

    }
    $scope.removeCookies = function () {
        $scope.subCategory = {};
        $cookieStore.put('editsubCategory', $scope.subCategory);
    }
    $scope.clearData = function () {
        $scope.subCategory = {};
        $cookieStore.put('editsubCategory', $scope.subCategory);
    }


    // saveSubCategory
    $scope.saveSubCategory = function () {
        debugger
        if ($scope.subCategoryForm.$valid) {
            var saveCategory = settingRepository.saveSubCategory($scope.subCategory).then(function (response) {
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
    $scope.loadSubCategory = function () {
        debugger
        $scope.subCategoryList = [];
        settingRepository.loadSubCategory().then(function (response) {
            if (response.data) {
                $scope.subCategoryList = response.data;
            }
        })
    }



    $scope.editRow = function (row) {
        debugger
        $scope.subCategory = row;
        $cookieStore.put('editsubCategory', $scope.subCategory);
        $location.path("/SubCategoryCreate");

    }

})