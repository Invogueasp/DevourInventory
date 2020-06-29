angular.module('app').controller('sprController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

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

        $scope.storePR = {};
        $scope.storePR.SPRDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.storePR.RequiredDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.storePR.UserFullName = $scope.UserFullName;
        $cookieStore.put('editstorePR', $scope.storePR);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }


    $scope.parameters = {};
    $scope.storePR = {};
    $scope.statusList = [];
    $scope.storePR.RequiredDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
    $scope.loadParameters = function () {
        //$scope.parameters.FormDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.parameters.FormDate = null;
        $scope.parameters.ToDate = null;
        $scope.loadStore();
        $scope.loadDepartment();
    }

    $scope.loadDropdowns = function () {        
        $scope.loadStore();
        $scope.loadCategory();
        $scope.loadProducts();
        $scope.loadUnit();
        //$scope.loadStore();
        $scope.statusList = commonRepository.getAStatus();
        $scope.loadLoginBranchID();
        debugger
        $scope.storePR.SPRDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.storePR.RequiredDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
       

        $scope.storePR = $cookieStore.get('editstorePR');
        if ($scope.storePR.SPRID) {
            $scope.loadSprDtls();
        } else {

            $scope.getNewPersonnelCode();
        }
    }
    $scope.clearData = function () {
        debugger
        $scope.storePR = {};
        $scope.Reload();
        $cookieStore.put('editstorePR', $scope.storePR);
    }

    //  store REQ Add
    $scope.sprDtls = [];
    $scope.deleteTransferDtlsID = [];
    var indexID = 0;
    $scope.addStoreReqRow = function (details) {
        debugger
        if (details && details.CategoryID != null && details.ProductID != null && details.ReqQty !="" && details.UnitID) {

            for (i = 0; i < $scope.sprDtls.length; i++) {
                if (details.CategoryID == $scope.sprDtls[i].CategoryID && details.ProductID == $scope.sprDtls[i].ProductID) {
                    toastr.error("Category and Product are already exist !!!");
                    return;
                }
            }
           
                $scope.sprDtls.push({
                    CategoryID: details.CategoryID,
                    ProductID: details.ProductID,
                    UnitID: details.UnitID,
                    ReqQty: details.ReqQty,
                    Remarks: details.Remarks,
                    Quantity:details.Quantity,
                    IndexID: indexID
                });
                details.ProductID = null;
                details.CategoryID = null;
                details.UnitID = null;
                details.ReqQty = "";
                details.Remarks = "";
                ++indexID;
           

        }
        else {
            toastr.error("Category,Product and Quantity,Unit are required !!!");
        }
        $scope.unitName = "";

    }
    $scope.updateDetailsData = function (details) {
        for (var i = 0; i < $scope.sprDtls.length; i++) {
            if ($scope.sprDtls[i].IndexID == details.IndexID) {
                $scope.sprDtls[i].CategoryID = details.CategoryID;
                $scope.sprDtls[i].ProductID = details.ProductID;
                $scope.sprDtls[i].UnitID = details.UnitID;
                $scope.sprDtls[i].ReqQty = details.ReqQty;
                $scope.sprDtls[i].Remarks = details.Remarks;
                $scope.sprDtls[i].Quantity = details.Quantity;
            }
        }
    }
    $scope.deleteSPRDtlsID = [];
    $scope.removeStoreReqRow = function (index, details) {
        debugger
        $scope.sprDtls.splice(index, 1);
        if (details.SPRDtlsID > 0) {
            $scope.deleteSPRDtlsID.push(details.SPRDtlsID);
        }

    }
    $scope.loadStore = function () {
        debugger
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    debugger
                }
                $scope.storeList = response.data;
            }
        })
    }
    $scope.loadStockQty = function (row) {
        debugger
        $scope.details.Quantity = 0;
        inventoryRepository.loadStockQty($scope.storePR.BranchID, row.CategoryID, row.ProductID, row.UnitID).then(function (response) {
            if (response.data) {
                debugger
                $scope.details.Quantity = response.data.Quantity;
            }
        })


    }
    $scope.loadLoginBranchID = function () {
        debugger

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                debugger
                $scope.storePR.BranchID = response.data.branchsID;
                $scope.storePR.UserFullName = response.data.userName;
                $scope.storePR.Department = response.data.department;
            }
        })
    }
    $scope.loadTransfer = function () {

        $scope.transferList = [];
        inventoryRepository.loadTransfer().then(function (response) {
            if (response.data) {
                debugger

                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].TransferDate != null) {
                        var TransferDate = response.data[i].TransferDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].TransferDate = $filter('date')(TransferDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                }
                $scope.transferList = response.data;
            }
        })
    }
    $scope.editRow = function (row) {
        debugger
        $scope.storePR = row;
        $cookieStore.put('editstorePR', $scope.storePR);
        $location.path("/PurchaseRequisitionCreate");

    }
    $scope.loadDepartment = function () {

        $scope.deptList = [];
        settingRepository.loadDepartment().then(function (response) {
            if (response.data) {
                $scope.deptList = response.data;
            }
        })
    }
    //$scope.loadStore = function () {
    //    debugger
    //    $scope.warehouseList = [];
    //    settingRepository.loadWarehouse().then(function (response) {
    //        if (response.data) {
    //            $scope.warehouseList = response.data;
    //        }
    //    })
    //}
    $scope.sprID = 0;
    $scope.loadSpr = function () {
        debugger
        $scope.sprList = [];
        inventoryRepository.loadSpr($scope.sprID, $scope.parameters).then(function (response) {
            if (response.data) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].SPRDate != null) {
                        var SPRDate = response.data[i].SPRDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].SPRDate = $filter('date')(SPRDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].RequiredDate != null) {
                        var RequiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].RequiredDate = $filter('date')(RequiredDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].UserFullName != null) {
                        $scope.UserFullName = response.data[i].UserFullName;
                        $scope.Department = response.data[i].Department;
                    }
                    if (response.data[i].FirstApproveStatus == "A") {
                        response.data[i].endAppBtn = true;
                    } else {
                        response.data[i].endAppBtn = false;
                    }


                }
                $scope.sprList = response.data;
            }
        })
    }
    $scope.saveSpr = function () {
        debugger
        $scope.storePR.WarehouseID = 1;
        if ($scope.sprForm.$valid) {
            $scope.storePR.RequiredDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
            if ($scope.details.CategoryID == null && $scope.details.ProductID == null && $scope.details.UnitID == null && $scope.details.ReqQty == "" || $scope.details.CategoryID == undefined) {

                var saveType = inventoryRepository.saveSpr($scope.storePR, $scope.sprDtls, $scope.deleteSPRDtlsID).then(function (response) {
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
    //===================================== MRR Report ==================================================
    $scope.parameters = {};
    $scope.reportView = function (row) {

        var url = '/Inventory/PurchaseRequisition/IndentPrReport';
        $scope.parameters.SPRID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    //==================================================================================================
    $scope.clearLastData = function () {
        $scope.unitName = "";
        $scope.details.CategoryID = null;
        $scope.details.ProductID = null;
        $scope.details.UnitID = null;
        $scope.details.ReqQty = "";

    }
    $scope.loadSprDtls = function () {
        debugger
        //$scope.storeDtlsList = [];
        inventoryRepository.loadSprDtls($scope.storePR.SPRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sprDtls = response.data;
                for (i = 0; i < $scope.sprDtls.length; i++) {

                    if ($scope.sprDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.storePR.BranchID, $scope.sprDtls[i].CategoryID, $scope.sprDtls[i].ProductID, $scope.sprDtls[i].UnitID).then(function (response) {
                            if (response.data) {
                                debugger
                                $scope.Id = response.data.ProductID;
                                for (j = 0; j < $scope.sprDtls.length; j++) {
                                    if ($scope.Id == $scope.sprDtls[j].ProductID) {
                                        $scope.sprDtls[j].Quantity = response.data.Quantity;
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

    $scope.loadCategoryWiseProduct = function (categoryID) {
        $scope.productList = [];
        settingRepository.loadCategoryWiseProduct(categoryID).then(function (response) {
            if (response.data) {
                debugger
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
        var tableName = "INV_SPR";
        var fieldName = "SPRNO";
        var prefix = "SPR";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.storePR.SPRNO = response.data.message;
            }
        })
    }
    $scope.details={};
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
});