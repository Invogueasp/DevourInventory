angular.module('app').controller('scrapReturnController', function ($scope, $filter, $location, $route, inventoryRepository, commonRepository, settingRepository, $templateCache, $cookieStore) {

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

        $scope.sreturn = {};
        $scope.sreturn.ReturnDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.sreturn.UserFullName = $scope.UserFullName;
        $cookieStore.put('editsreturn', $scope.sreturn);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }

    $scope.sreturn = {};

    $scope.statusList = [];
    $scope.loadDropdowns = function () {

        $scope.sreturn.ReturnDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.loadLoginBranchID();
        $scope.loadStore();
        $scope.loadCategory();
        $scope.loadUnit();
        $scope.loadProducts();
        $scope.statusList = commonRepository.getAStatus();
      
        debugger
        $scope.sreturn = $cookieStore.get('editsreturn');
        if ($scope.sreturn.ScrapReturnID) {
            $scope.loadScrapDtls();
        } else {
            $scope.getNewPersonnelCode();
        }
    }
    $scope.clearData = function () {
        debugger
        $scope.sreturn = {};
        $scope.Reload();
        $cookieStore.put('editsreturn', $scope.sreturn);
    }

    //  store REQ Add
    $scope.sreturnDtls = [];
    $scope.deletesreturnDtlsID = [];
    var indexID = 0;
    $scope.addsreturnRow = function (details) {
        debugger
        if (details.CategoryID != null && details.ProductID != null && details.Quantity != "" && details.UnitID) {
            if (details.Quantity > details.sQuantity) {
                toastr.error('Scrap quantity can not greater then stock quantity !!');
                return;
            }

            $scope.sreturnDtls.push({
                CategoryID: details.CategoryID,
                ProductID: details.ProductID,
                UnitID: details.UnitID,
                Quantity: details.Quantity,
                Remarks: details.Remarks,
                sQuantity: details.sQuantity,
                Location: details.Location,
                IndexID: indexID
            });
            details.ProductID = null;
            details.CategoryID = null;
            details.UnitID = null;
            details.Quantity = "";
            details.Remarks = "";
            details.sQuantity = null;
            details.Location = " ";
            ++indexID;
        }

        $scope.unitName = " ";
    }
    $scope.updateDetailsData = function (details) {
        for (var i = 0; i < $scope.sreturnDtls.length; i++) {
            if ($scope.sreturnDtls[i].IndexID == details.IndexID) {
                $scope.sreturnDtls[i].CategoryID = details.CategoryID;
                $scope.sreturnDtls[i].ProductID = details.ProductID;
                $scope.sreturnDtls[i].UnitID = details.UnitID;
                $scope.sreturnDtls[i].Quantity = details.Quantity;
                $scope.sreturnDtls[i].Remarks = details.Remarks;
                $scope.sreturnDtls[i].Location = details.Location;

                $scope.sreturnDtls[i].sQuantity = details.sQuantity;
            }
        }
    }
    $scope.removesreturnRow = function (index, details) {
        $scope.sreturnDtls.splice(index, 1);
        if (details.ScrapReturnDtlsID > 0) {
            $scope.deletesreturnDtlsID.push(details.ScrapReturnDtlsID);
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

    $scope.loadStockQty = function (row) {
        debugger
        $scope.details.sQuantity = 0;
        inventoryRepository.loadStockQty($scope.sreturn.BranchID, row.CategoryID, row.ProductID, row.UnitID).then(function (response) {
            if (response.data) {
                debugger
                $scope.details.sQuantity = response.data.Quantity;
            }
        })


    }



    $scope.loadLoginBranchID = function () {
        debugger

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                debugger
                $scope.sreturn.BranchID = response.data.branchsID;
                $scope.sreturn.UserFullName = response.data.userName;

               
            }
        })
    }
    $scope.loadsreturn = function () {

        $scope.sreturnList = [];
        inventoryRepository.loadScrapReturn().then(function (response) {
            if (response.data) {
                debugger

                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].ReturnDate != null) {
                        var RequisitionDate = response.data[i].ReturnDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].ReturnDate = $filter('date')(RequisitionDate, "dd-MMM-yyyy");
                    }
                
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].UserFullName != null) {
                        $scope.UserFullName = response.data[i].UserFullName;
                    }
                }
                $scope.sreturnList = response.data;
            }
        })
    }
    $scope.editRow = function (row) {
        debugger
        $scope.sreturn = row;
        $scope.loadLoginBranchID();
        $cookieStore.put('editsreturn', $scope.sreturn);
        $location.path("/ScrapReturnCreate");

    }
    //===================================== Scrap Return ==================================================
    $scope.parameters = {};
    $scope.reportView = function (row) {

        var url = '/Inventory/ScrapReturn/ScrapReturnView';
        $scope.parameters.ScrapReturnID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    //==================================================================================================
    $scope.saveSreturn = function () {
        debugger
        if ($scope.sreturnForm.$valid) {
            if ($scope.details.CategoryID == null && $scope.details.ProductID == null && $scope.details.UnitID == null && $scope.details.ReqQty == "" || $scope.details.CategoryID == undefined) {
                var saveType = inventoryRepository.saveScrapReturn($scope.sreturn, $scope.sreturnDtls, $scope.deletesreturnDtlsID).then(function (response) {
                    if (response.data.isSucess) {

                        toastr.success(response.data.message);
                        $scope.reportView(response.data.lastInsertedID);

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
    $scope.loadScrapDtls = function () {
        debugger
        //$scope.storeDtlsList = [];
        inventoryRepository.loadScrapDtls($scope.sreturn.ScrapReturnID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sreturnDtls = response.data;

                for (i = 0; i < $scope.sreturnDtls.length; i++) {

                    if ($scope.sreturnDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.sreturn.BranchID, $scope.sreturnDtls[i].CategoryID, $scope.sreturnDtls[i].ProductID, $scope.sreturnDtls[i].UnitID).then(function (response) {
                            if (response.data) {
                                debugger
                                $scope.Id = response.data.ProductID;
                                for (j = 0; j < $scope.sreturnDtls.length; j++) {
                                    if ($scope.Id == $scope.sreturnDtls[j].ProductID) {
                                        $scope.sreturnDtls[j].Quantity = response.data.Quantity;
                                    }

                                }

                                //$scope.storeReqDtls[i].StockQty = 5;
                            }
                        })



                    }
                }




            }
        })
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
        debugger
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
        var tableName = "INV_ScrapReturn";
        var fieldName = "ReturnNO";
        var prefix = "SRN";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.sreturn.ReturnNO = response.data.message;
            }
        })
    }

});