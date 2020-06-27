angular.module('app').controller('openingStockController', function ($scope, $filter, $location, $route, inventoryRepository, commonRepository, settingRepository, $templateCache, $cookieStore) {

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
        debugger

        $scope.openingStock = {};
        $scope.openingStock.CreatedDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.openingStock.UserFullName = $scope.UserFullName;
        $cookieStore.put('editopeningStock', $scope.openingStock);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }

    $scope.openingStock = {};

    $scope.statusList = [];
    $scope.loadDropdowns = function () {

        $scope.openingStock.CreatedDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.loadLoginBranchID();
        $scope.loadStore();
        $scope.loadCategory();
        $scope.loadUnit();
        $scope.loadProducts();
        $scope.statusList = commonRepository.getAStatus();
        debugger

    }
    $scope.clearData = function () {
        debugger
        $scope.openingStock = {};
        $scope.Reload();
        $cookieStore.put('editopeningStock', $scope.openingStock);
    }

    //  store REQ Add
    $scope.storeReqDtls = [];
    $scope.deleteStoreReqDtlsID = [];
    var indexID = 0;
    $scope.addStoreReqRow = function (details) {
        debugger
        if (details.CategoryID != null && details.ProductID != null && details.Quantity != "" && details.UnitID) {
            $scope.storeReqDtls.push({
                CategoryID: details.CategoryID,
                ProductID: details.ProductID,
                UnitID: details.UnitID,
               
                Remarks: details.Remarks,
                Quantity: details.Quantity,
                IndexID: indexID
            });
            details.ProductID = null;
            details.CategoryID = null;
            details.UnitID = null;
           
            details.Remarks = "";
            details.Quantity = null;
            ++indexID;
        }

        $scope.unitName = " ";
    }
    $scope.updateDetailsData = function (details) {
        for (var i = 0; i < $scope.storeReqDtls.length; i++) {
            if ($scope.storeReqDtls[i].IndexID == details.IndexID) {
                $scope.storeReqDtls[i].CategoryID = details.CategoryID;
                $scope.storeReqDtls[i].ProductID = details.ProductID;
                $scope.storeReqDtls[i].UnitID = details.UnitID;
               
                $scope.storeReqDtls[i].Remarks = details.Remarks;

                $scope.storeReqDtls[i].Quantity = details.Quantity;
            }
        }
    }
    $scope.removeStoreReqRow = function (index, details) {
        $scope.storeReqDtls.splice(index, 1);
        if (details.SRDtlsID > 0) {
            $scope.deleteStoreReqDtlsID.push(details.SRDtlsID);
        }

    }
    $scope.loadStore = function () {
        debugger
        $scope.branchList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.branchList = response.data;
            }
        })
    }

    //$scope.loadStockQty = function (row) {
    //    debugger
    //    $scope.details.Quantity = 0;
    //    inventoryRepository.loadStockQty($scope.openingStock.BranchID, row.CategoryID, row.ProductID, row.UnitID).then(function (response) {
    //        if (response.data) {
    //            debugger
    //            $scope.details.Quantity = response.data.Quantity;
    //        }
    //    })


    //}



    $scope.loadLoginBranchID = function () {
        debugger

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                debugger
                $scope.openingStock.BranchID = response.data.branchsID;
            }
        })
    }

    $scope.editRow = function (row) {
        debugger
        $scope.openingStock = row;
        $scope.loadLoginBranchID();
        $cookieStore.put('editopeningStock', $scope.openingStock);
        //$location.path("/openingStockuisitionCreate");

    }

    $scope.saveOpeningStock = function () {
        debugger
        if ($scope.openingStockForm.$valid) {
            if ($scope.details.CategoryID == null && $scope.details.ProductID == null && $scope.details.UnitID == null && $scope.details.ReqQty == "" || $scope.details.CategoryID == undefined) {
                var saveType = inventoryRepository.saveOpeningStock($scope.storeReqDtls, $scope.openingStock).then(function (response) {
                    if (response.data.isSucess) {

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
                toastr.error("Please add new data or clear !!!");
            }




        } else {
            toastr.error("Please fill-up all required field !!!");
        }
    }
    $scope.clearLastData = function () {
        $scope.unitName = "";
        $scope.details.CategoryID = null;
        $scope.details.ProductID = null;
        $scope.details.UnitID = null;
        $scope.details.ReqQty = "";

    }



    $scope.loadProducts = function () {
        debugger
        $scope.productLists = [];
        settingRepository.loadProduct().then(function (response) {
            if (response.data) {
                $scope.productLists = response.data;
            }
        })
    }


    $scope.loadProduct = function (id) {
        debugger
        $scope.productList = [];
        settingRepository.loadProduct(id).then(function (response) {
            if (response.data) {
                $scope.productList = response.data;
            }
        })
    }

    //$scope.SelectedCustomer = function (selected) { //event fires when click on textbox    
    //    if (selected) {
    //        debugger
    //        $scope.sell.CustomerID = selected.originalObject.CustomerID;
    //    }
    //}
    //$scope.loadCategoryWiseProduct = function (row) {
    //    debugger



    //    if (row.SearchName.length >= 3 && row.CategoryID != undefined) {
    //        $scope.productList = [];

    //        settingRepository.loadCategoryWiseProduct(row.CategoryID, row.SearchName).then(function (response) {
    //            if (response.data) {
    //                debugger
    //                $scope.productList = response.data;
    //            }
    //        })
    //    }

    //}
    $scope.details = {};
    $scope.searchUnitDropDown = function () {
        if ($scope.unitName == "") {
            $scope.details.CategoryID = null;
            $scope.details.UnitID = null;
        }
        if ($scope.unitName != null && $scope.unitName.length >= 3) {
            var searchInput = $scope.unitName.trim().length;
            if (searchInput > 0) {

                if ($scope.details.CategoryID == undefined) {
                    $scope.details.CategoryID = 0;
                }
                settingRepository.loadCategoryWiseProduct($scope.details.CategoryID, $scope.unitName).then(function (response) {
                    if (response.data) {
                        $scope.cproductList = response.data;
                    }
                })



            } else {
                $scope.cproductList = [];
            }
        } else {
            $scope.cproductList = [];
        }

    }

    $scope.searchboxClicked = function ($event) {
        $event.stopPropagation();
    }
    // Set value to search box
    $scope.setValue = function (index, $event, unit) {
        debugger
        $scope.unitName = $scope.cproductList[index].Name;
        $scope.UnitID = $scope.cproductList[index].UnitID;


        $scope.cproductList = [];
        $event.stopPropagation();
        $scope.details.UnitID = unit.UnitID;
        $scope.details.CategoryID = unit.CategoryID;

        $scope.details.ProductID = unit.ProductID;
        $scope.loadStockQty(unit);
    }




    $scope.loadCategory = function () {
        $scope.categoryList = [];
        settingRepository.loadCategory().then(function (response) {
            if (response.data) {
                $scope.categoryList = response.data;
            }
        })
    }

    $scope.loadUnit = function () {
        $scope.unitList = [];
        settingRepository.loadUnit().then(function (response) {
            if (response.data) {
                $scope.unitList = response.data;
            }
        })
    }

    $scope.loadProductWiseUnit = function (productID) {
        var product = $filter('filter')($scope.productList, function (d) { return d.ProductID === productID })[0];
        $scope.details.UnitID = product.UnitID;
    }



    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_SR";
        var fieldName = "SRNO";
        var prefix = "SRT";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.storeReq.SRNO = response.data.message;
            }
        })
    }

});