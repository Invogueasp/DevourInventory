angular.module('app').controller('stockController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, settingRepository, commonRepository, inventoryRepository) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel
    //Start :: Common Function
    $scope.removeCookies = function () {
        $scope.companyBranch = {};
        $cookieStore.put('companyBranch', $scope.companyBranch);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }

    $scope.stock = {};
    $scope.loadDropdowns = function () {
        debugger
        $scope.loadStore();
        $scope.loadCategory();
        $scope.loadStock();
        //$scope.loadWarehouse();
   
    }
    $scope.loadCategory = function () {
        $scope.categoryList = [];
        settingRepository.loadCategory().then(function (response) {
            if (response.data) {
                $scope.categoryList = response.data;
            }
        })
    }
    $scope.loadWarehouse = function () {
        debugger
        $scope.warehouseList = [];
        settingRepository.loadWarehouse().then(function (response) {
            if (response.data) {
                $scope.warehouseList = response.data;
            }
        })
    }
    $scope.loadStore = function () {
        debugger
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
            debugger
                $scope.storeList = response.data;
            }
        })
    }

    //$scope.loadStocks = function () {
    //    debugger
    
    //    $scope.stockLists = [];
    //    inventoryRepository.loadStock($scope.stock.BranchID, $scope.stock.CategoryID).then(function (response) {
    //        if (response.data) {
    //            $scope.stockLists = response.data;
    //        }
    //    })
    //}
    $scope.maxSize = 5;
    $scope.totalItemCount = 0;
    $scope.Pageindex = 1;
    $scope.Pagesize = 10;
    $scope.Namevalue = "";
    $scope.loadStock = function () {
        debugger
        $scope.pageValueList = commonRepository.getPageValue();
        $scope.stockLists = [];
        
        inventoryRepository.loadStock($scope.stock.BranchID, $scope.stock.CategoryID, $scope.Namevalue, $scope.Pageindex, $scope.Pagesize).then(function (response) {
            if (response.data) {
                debugger
                $scope.stockLists = response.data;
                $scope.totalCount = response.data[0].TotalStock;
            }
        })   
       
    }

    $scope.nameWiseSearch = function () {
        debugger
        if ($scope.Namevalue != "") {
            if ($scope.Namevalue.length > 2) {
                $scope.loadStock();
            }
        } else {
            $scope.loadStock();
        }
    }

    $scope.pageChanged = function () {
        $scope.loadStock();
    }
    $scope.changePageSize = function () {
        $scope.pageIndex = 1;
        $scope.loadStock();
        
    }

 
});