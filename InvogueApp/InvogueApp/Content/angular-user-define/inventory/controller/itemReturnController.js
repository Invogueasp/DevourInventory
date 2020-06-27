angular.module('app').controller('itemReturnController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore,commonRepository, settingRepository, inventoryRepository) {

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
        $scope.itemReturn = {};
        $cookieStore.put('editReturn', $scope.itemReturn);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
    //End :: Common Function
    $scope.productTypeList = [];
    $scope.loadDropdowns = function () {
      //  $scope.transferApp = $cookieStore.get('edittransferApp');
        $scope.loadStore();
        $scope.loadCategory();
        $scope.loadProduct();
        $scope.loadUnit();
        $scope.loadWarehouse();
        $scope.loadSuppliers();
        $scope.loadPO2ndApp();
        debugger

        $scope.button = true;
        $scope.itemReturn = $cookieStore.get('editReturn');
        debugger
        $scope.productTypeList = commonRepository.getProductType();

        if ($scope.itemReturn.ToWarehouseID) {

            $scope.issueDataList();
            $scope.storeWiseReq();
            $scope.loadStore();
            //$scope.hideData = true;
        }
        if ($scope.itemReturn.ReturnID) {
            $scope.showReqWishData();
            $scope.storeWiseReq();
            $scope.loadReturnDtls();
           // $scope.getPono();

            $scope.transferDtlsbyId();

            $scope.storeWiseTransfer();
           
            //$scope.button = false;
        } else {
            $scope.getNewPersonnelCode();
        }
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
    $scope.loadSuppliers = function () {
        debugger
        $scope.supplierList = [];
        settingRepository.loadSupplier().then(function (response) {
            if (response.data) {
                debugger
                $scope.supplierList = response.data;
            }
        })
    }
    $scope.loadSupplier = function () {
        debugger
        settingRepository.loadSupplier($scope.itemReturn.POID).then(function (response) {
            if (response.data) {
                debugger
                $scope.itemReturn.ToSupplierID = response.data[0].SupplierID;
            }
        })
    }
    // Modal Open
    $scope.isShiftModal = false;
    $scope.shiftClose = function () {
        $scope.isShiftModal = !$scope.isShiftModal;
    }
    $scope.viewSPRApprovalRow = function (row) {
        $scope.isShiftModal = !$scope.isShiftModal;

    }
    // Purchase Order

    //$scope.cStock = {};
    $scope.showReqWishData = function () {
        debugger
        $scope.issueDtlsList = [];
        $scope.transferDtlsList = [];
        if ($scope.itemReturn.ReturnType == "1") {
            $scope.whToS = true;
            $scope.sToWh = false;
            $scope.sToS = false;
        } else if ($scope.itemReturn.ReturnType == "2") {
            $scope.sToWh = true;
            $scope.whToS = false;
            $scope.sToS = false;
           // $scope.itemReturn.FromStoreID=null;
            $scope.itemReturn.ToStoreID=null;
        } else if ($scope.itemReturn.ReturnType == "3") {
            $scope.itemReturn.ToWarehouseID = null;
            $scope.sToS = true;
            $scope.whToS = false;
            $scope.sToWh = false;
        } else {
            $scope.whToS = false;
            $scope.sToWh = false;
            $scope.sToS = false;
        }

    }
    //$scope.getPono = function () {
    //    debugger
    //    $scope.sprList = [];

    //    inventoryRepository.loadSpr().then(function (response) {
    //        if (response.data && response.data.length > 0) {
    //            debugger
    //            $scope.sprList = response.data;
    //        }
    //        debugger
    //        var list = $filter('filter')($scope.sprList, function (d) { return d.BranchID === $scope.itemReturn.FromWarehouseID })[0];
    //       $scope.itemReturn.SPRID = list.SPRID;

    //       inventoryRepository.loadMRR().then(function (response) {
    //            if (response.data) {
    //                debugger
    //                $scope.purchaseOrderList = response.data;
    //            }
    //            $scope.sprList = $filter('filter')($scope.purchaseOrderList, function (d) { return d.SPRID === $scope.itemReturn.SPRID });
    //        })

    //    })
        
    //}

  

    $scope.reqWisePODtls = function () {

        $scope.sprDtls = [];
        inventoryRepository.loadMRSForQCDtls($scope.itemReturn.QCID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sprDtls = response.data;
                $scope.itemRetunDtlss = $scope.sprDtls;
                for (i = 0; i < $scope.sprDtls.length; i++) {
                    $scope.sprDtls[i].Index = [i];
                    if ($scope.sprDtls[i].POID) {
                        
                        $scope.itemReturn.POID = $scope.sprDtls[i].POID;
                        $scope.loadSupplier();

                    }


                }

            }
        })
    }

    $scope.loadPO2ndApp = function () {
        debugger
        $scope.purchaseOrderLists = [];
        inventoryRepository.loadQC().then(function (response) {
            if (response.data && response.data.length > 0) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].QCDate != null) {
                        var QCDate = response.data[i].QCDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].QCDate = $filter('date')(QCDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                }
                $scope.purchaseOrderLists = response.data;
            }
        })
    }


    $scope.hideData = false;
    $scope.searchData = function () {
        debugger
        $scope.hideData = true;

        //var productTypeList = commonRepository.getProductType();
    }

    $scope.itemReturn = {};

    $scope.itemRetunDtlss = [];
    $scope.disableBtn = false;
    $scope.itemReturnRemerks = function (row) {
        debugger
        if (row.ReturnQty > row.TransferQty || row.ReturnQty > row.QCFailQty || row.ReturnQty > row.ApprovedQty) {
            $scope.disableBtn = true;
            toastr.error("Return Quantity can not bigger then Other Quantity");
        }
        else {
            // $scope.issueDtl.push(row);
            for (var i = 0; i < $scope.itemRetunDtlss.length; i++) {
                if ($scope.itemRetunDtlss[i].ProductID == row.ProductID && $scope.itemRetunDtlss[i].PODtlsID == row.PODtlsID) {
                    $scope.itemRetunDtlss[i].CategoryID = row.CategoryID;
                    $scope.itemRetunDtlss[i].ProductID = row.ProductID;
                    $scope.itemRetunDtlss[i].UnitID = row.UnitID;
                    $scope.itemRetunDtlss[i].ProductType = row.ProductType;
                    $scope.itemRetunDtlss[i].ReturnQty = row.ReturnQty;
                    $scope.itemRetunDtlss[i].QCPassQty = row.QCPassQty;
                    $scope.itemRetunDtlss[i].Remarks = row.Remarks;
                    $scope.itemRetunDtlss[i].QCFailQty = row.QCFailQty;
                    $scope.itemRetunDtlss[i].ReceiveQty = row.ReceiveQty;
                }
            }

        }
    }


    $scope.saveItemReturn = function () {
        debugger
        //if ($scope.itemReturnForm.$valid) {
        $scope.itemReturn.ReturnTypeID = $scope.itemReturn.ReturnType;
            var saveType = inventoryRepository.saveIetmReturn($scope.itemReturn, $scope.itemRetunDtlss).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.reportView(response.data.lastInsertedID);
                    $scope.loadReturn();
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
    //===================================== MRR Report ==================================================
    $scope.parameters = {};
    $scope.reportView = function (row) {

        var url = '/Inventory/ItemReturn/ReturnMemoReport';
        $scope.parameters.ReturnID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    //==================================================================================================
    $scope.getPodtls = function () {
            debugger
            $scope.sprDtls = [];
            inventoryRepository.loadMRSDtlsFrReturn($scope.itemReturn.POID).then(function (response) {
                if (response.data) {
                    debugger
                    $scope.sprDtls = response.data;
                    $scope.itemRetunDtlss = response.data;
                    $scope.transferDtlsList = [];
                }
            })
    }
    $scope.loadReturnDtls = function () {
        debugger
        $scope.itemRetunDtlss = [];
        inventoryRepository.loadReturnDtls($scope.itemReturn.ReturnID).then(function (response) {
            if (response.data) {
                debugger
                $scope.itemRetunDtlss = response.data;
                
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].POID) {
                        $scope.itemReturn.POID = response.data[i].POID;
                    }
                }
                $scope.sprDtls = response.data;
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
                    if (response.data[i].ReturnTypeID == 1) {
                       
                        response.data[i].ReturnTypes = "Warehouse to Supplier";
                    }
                    if (response.data[i].ReturnTypeID == 2) {

                        response.data[i].ReturnTypes = "Store to Warehouse";
                    }
                    if (response.data[i].ReturnTypeID == 3) {

                        response.data[i].ReturnTypes = "Store to Store";
                    }
                    debugger
                    if (response.data[i].Status == "A") {

                        response.data[i].end3AppBtn =true ;
                    } else {
                        response.data[i].end3AppBtn = false;
                    }
                }

                $scope.returnList = response.data;
            }
        })
    }

    $scope.loadStore = function () {
        debugger
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.storeList = response.data;
            }
        })
    }
    //$scope.itemReturn.FromStoreID
    $scope.storeWiseReq = function () {
        debugger
        $scope.storeWiseReqList = [];
        settingRepository.loadStoreReq().then(function (response) {
            if (response.data) {
                debugger
                $scope.storeWiseReqList = response.data;

                $scope.storeWiseReqList = $filter('filter')($scope.storeWiseReqList, function (d) { return d.StoreID === $scope.itemReturn.FromStoreID });


            }
        })
    }
    $scope.issueDataList = function () {
        debugger
        $scope.issueIDList = [];
        $scope.transferDtlsList = [];
        if ($scope.itemReturn.SRID) {
            $scope.issueIDList = [];
            inventoryRepository.loadIssueID($scope.itemReturn.SRID).then(function (response) {
                if (response.data) {
                    debugger
                    $scope.issueIDList = response.data;
               
                }
                for (i = 0; i < $scope.issueIDList.length; i++) {
                    var issueID = $scope.issueIDList[i].IssueID;
                    $scope.issueDtlsList = [];
                    inventoryRepository.loadIssueDtls(issueID).then(function (response) {
                        if (response.data) {
                            debugger
                            $scope.issueDtlsList = response.data;
                            $scope.itemRetunDtlss = response.data;
                            //$scope.transferDtls = false;
                            //$scope.returnDtls = false;
                            $scope.transferDtlsList = [];
                            //$scope.issueDtls = true;
                        }
                        
                    })
                }
            })
        }
      
    }


    $scope.transferDtlsbyId = function () {

        if ($scope.itemReturn.TransferID) {
            $scope.transferDtlsList = [];
            inventoryRepository.loadRtnTransferDtls($scope.itemReturn.TransferID).then(function (response) {
                if (response.data) {
                    debugger
                    $scope.transferDtlsList = response.data;
                    $scope.itemRetunDtlss = response.data;
                    $scope.issueDtlsList = [];
                
                }
            })

        }
    }
    $scope.storeWiseTransfer = function () {
        debugger
        $scope.issueDtlsList = [];
        $scope.transferList = [];
        inventoryRepository.loadTransferNo($scope.itemReturn.FromStoreID, $scope.itemReturn.ToStoreID).then(function (response) {
            if (response.data) {
                debugger
                $scope.transferList = response.data;
            }
        })


        //.catch(function (response) {
        //    $scope.issueDtlsList = [];
        //    toastr.error('Data Not Found');
        //});
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
    $scope.editRow = function (row) {
        debugger
       
        $scope.itemReturn = row;
        $cookieStore.put('editReturn', $scope.itemReturn);
        $location.path("/ItemReturnCreate");
    }
    $scope.clearData = function () {
        debugger
        $scope.itemReturn = {};
        $scope.itemRetunDtlss = [];
        $scope.transferDtlsList = [];
        $scope.issueDtlsList = [];
        $scope.sprDtls = [];
        $cookieStore.put('editReturn', $scope.itemReturn);
        $scope.Reload();
     
    }

    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_Return";
        var fieldName = "ReturnNO";
        var prefix = "RTN";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.itemReturn.ReturnNO = response.data.message;
            }
        })
    }
});
