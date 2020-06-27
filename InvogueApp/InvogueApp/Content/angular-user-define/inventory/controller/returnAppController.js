angular.module('app').controller('returnAppController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, settingRepository, inventoryRepository) {


    $scope.itemReturn = {};

    $scope.isModal = false;
    $scope.modalClose = function () {
        debugger
        $scope.isModal = !$scope.isModal;

    }
    $scope.loadDropdowns = function () {
       
        $scope.loadReturn();
    }



    $scope.saveItemReturn = function () {
        debugger
        //if ($scope.itemReturnForm.$valid) {
        var saveType = inventoryRepository.saveIetmReturnDtls($scope.itemReturn, $scope.returnDtlsList).then(function (response) {
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
        //} else {
        //    toastr.error("Please fill-up all required field !!!");
        //}

    }

    $scope.loadReturnDtls = function () {
        debugger
        $scope.returnDtlsList = [];
        inventoryRepository.loadReturnDtls($scope.itemReturn.ReturnID).then(function (response) {
            if (response.data) {
                debugger
                $scope.returnDtlsList = response.data;
            }
        })
    }

    $scope.loadReturn = function () {
        debugger
        $scope.returnList = [];
        inventoryRepository.loadReturn().then(function (response) {
            if (response.data) {

                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].ReturnDate) {
                        var ReturnDate = response.data[i].ReturnDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].ReturnDate = $filter('date')(ReturnDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].ReturnType == 1) {

                        response.data[i].ReturnTypes = "Warehouse to Supplier";
                    }
                    if (response.data[i].ReturnType == 2) {

                        response.data[i].ReturnTypes = "Store to Warehouse";
                    }
                    if (response.data[i].ReturnType == 3) {

                        response.data[i].ReturnTypes = "Store to Store";
                    }
                }

                $scope.returnList = response.data;
            }
        })
    }
    $scope.returnDtlsLists = [];
    $scope.returnAppRemerks = function (row) {
        debugger
        $scope.returnDtlsLists.push(row);
    }
    $scope.returnDtlsView = function (row) {
        debugger

        if (row.Status == "A" && row.Status != null) {
            $scope.endAppBtn = false;
        } else {
            $scope.endAppBtn = true;
        }

        $scope.isModal = !$scope.isModal;
        $scope.itemReturn = row;
        $cookieStore.put('editReturnApp', $scope.itemReturn);
        $scope.loadReturnDtls();
    }

});