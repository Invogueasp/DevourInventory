angular.module('app').controller("companyController", function ($scope, $location, $cookieStore, settingsRepository, commonRepository) {
    $scope.company = {};
    $scope.statusList = [];

    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();
        $scope.company = $cookieStore.get('editCompany');
       
    }
    $scope.removeCookies = function () {
        $scope.company = {};
        $cookieStore.put('editCompany', $scope.company);
    }
    $scope.clearData = function () {
        $scope.company = {};
        $cookieStore.put('editCompany', $scope.company);
    }

    
    // Save company
    $scope.saveCompany = function () {
        debugger
        if ($scope.CompanyForm.$valid) {
            var saveCustomer = settingsRepository.saveCompany($scope.company).then(function (response) {
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

    $scope.loadCompany = function () {
        debugger
        $scope.companyList = [];
        settingsRepository.loadCompany().then(function (response) {
            if (response.data) {
                $scope.companyList = response.data;
            }
        })
    }
    $scope.editRow = function (row) {
        debugger
        $location.path("/CreateCompany");
        $scope.company = row;
        $cookieStore.put('editCompany', $scope.company);
    }

})