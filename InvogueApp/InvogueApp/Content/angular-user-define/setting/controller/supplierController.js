angular.module('app').controller('supplierController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, settingRepository) {

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
        $scope.supplier = {};
        $cookieStore.put('companyBranch', $scope.supplier);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
    //End :: Common Function
    //Tab Section
    $scope.tab = 1;
    $scope.setTab = function (tabId) {
        $scope.tab = tabId;
    };
    $scope.isSet = function (tabId) {
        return $scope.tab === tabId;
    };
    $scope.supplier = {};
    $scope.loadDropdowns = function () {
        $scope.loadCountry();
        $scope.loadProduct();
        $scope.loadCategory();
       
        $scope.LoadCurrency();
      debugger
        $scope.supplier = $cookieStore.get('editsupplier');
        if ($scope.supplier.SupplierID) {
            $scope.loadSupplierProduct();
            $scope.loadSubCategorys();
        }

    }
    $scope.removeCookies = function () {
        $scope.supplier = {};
        $cookieStore.put('editsupplier', $scope.supplier);
    }
    $scope.loadCountry = function () {
        debugger
        $scope.countryList = [];
        settingRepository.loadCountry().then(function (response) {
            if (response.data) {
                debugger
                $scope.countryList = response.data;
            }
        })
    }
    $scope.LoadCurrency = function () {

        $scope.currencyList = [];
        settingRepository.LoadCurrency().then(function (response) {
            if (response.data) {
                $scope.currencyList = response.data;
            }
        })
    }



    $scope.loadProduct = function () {

        $scope.productList = [];
        settingRepository.loadProduct().then(function (response) {
            if (response.data) {
                $scope.productList = response.data;
            }
        })
    }
    $scope.loadCategory = function () {

        $scope.categoryList = [];
        settingRepository.loadCategory().then(function (response) {
            if (response.data) {
                $scope.categoryList = response.data;
            }
        })
    }
    $scope.loadSubCategorys = function (id) {
        debugger
        $scope.subCategoryLists = [];
        settingRepository.loadSubCategory(id).then(function (response) {
            if (response.data) {
                $scope.subCategoryLists = response.data;
            }
        })
    }

    // Supplier Add
    $scope.supplierProductDtls = [];
    $scope.deleteSupplierProductRowID = [];
    var indexID = 0;
    $scope.addSupplierProductRow = function (details) {
        debugger
        if (details && details.CategoryID != null && details.ProductID != null) {
            $scope.supplierProductDtls.push({
                CategoryID: details.CategoryID,
                ProductID: details.ProductID,
                SubCategoryID: details.SubCategoryID,
                IndexID: indexID
            });
            //$scope.loadSubCategory(details.SubCategoryID);
            details.ProductID = null;
            details.CategoryID = null;
            details.SubCategoryID = null;
            ++indexID;
            

        }
        else {
            toastr.error("Category and Product is required field !!!");
        }


    }
    $scope.updateDetailsData = function (details) {
        debugger
        for (var i = 0; i < $scope.supplierProductDtls.length; i++) {
            if ($scope.supplierProductDtls[i].IndexID == details.IndexID) {
                $scope.supplierProductDtls[i].CategoryID = details.CategoryID;
                $scope.supplierProductDtls[i].ProductID = details.ProductID;
                $scope.supplierProductDtls[i].SubCategoryID = details.SubCategoryID;
            }
        }
    }
    $scope.removeSupplierProductRow = function (index, details) {
        debugger
        $scope.supplierProductDtls.splice(index, 1);
        if (details.SupplierProductID > 0) {
            $scope.deleteSupplierProductRowID.push(details.SupplierProductID);
        }

    }

    $scope.clearData = function () {
        debugger
        $scope.supplier = {};
        $scope.Reload();
        $cookieStore.put('editsupplier', $scope.supplier);
    }
    $scope.loadSubCategory = function (id) {
        
        $scope.subCategoryList = [];
        settingRepository.loadIdwiseSubCategory(id).then(function (response) {
            if (response.data) {
                debugger
                $scope.subCategoryList = response.data;
            }
        })
    }
    //========================== Save Supplier ======================
    $scope.saveSupplier = function () {
        debugger
        if ($scope.supplierForm.$valid) {

            $scope.supplier.Name = $scope.supplier.SupplierName;
            var saveType = settingRepository.saveSupplier($scope.supplier, $scope.supplierProductDtls, $scope.deleteSupplierProductRowID).then(function (response) {
                if (response.data.isSucess) {

                    toastr.success(response.data.message);
                    $scope.clearData();
                    $scope.Reload();
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

    $scope.loadSupplier = function () {
        
        $scope.supplierList = [];
        settingRepository.loadSupplier().then(function (response) {
            if (response.data) {
                debugger
                $scope.supplierList = response.data;
            }
        })
    }
    $scope.loadSupplierProduct = function () {

        $scope.supplierProductDtls = [];
        settingRepository.loadSupplierProduct($scope.supplier.SupplierID).then(function (response) {
            if (response.data) {
                debugger
                $scope.supplierProductDtls = response.data;
            }
        })
    }

    
    $scope.editRow = function (row) {
        debugger
       
        $scope.supplier = row;
       
        $cookieStore.put('editsupplier', $scope.supplier);
        $location.path("/SupplierCreate");

    }

});
