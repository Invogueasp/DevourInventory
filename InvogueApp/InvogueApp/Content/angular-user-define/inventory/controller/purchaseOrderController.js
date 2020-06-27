angular.module('app').controller('purchaseOrderController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    $scope.removeCookies = function () {
        debugger
        $scope.purchaseOrder = {};
        $scope.purchaseOrder.PODate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $cookieStore.put('editpurchaseOrder', $scope.purchaseOrder);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }


    $scope.parameters = {};
   $scope.purchaseOrder = {};
    $scope.statusList = [];
    $scope.loadParameters = function () {
        $scope.parameters.FormDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.parameters.ToDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.loadStore();
        $scope.loadDepartment();
    }
    $scope.loadDropdowns = function () {
        debugger
        $scope.loadStore();
        $scope.purchaseOrder.PODate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        
        $scope.loadLoginBranchID();
        
        $scope.purchaseOrder = $cookieStore.get('editpurchaseOrder');
        if ($scope.purchaseOrder.POID) {
            $scope.loadPODtlsList();


        } else {
            $scope.getNewPersonnelCode();
        }
        $scope.loadSprList();
        $scope.loadSprFrPendingPO();
        $scope.loadSpr();
        $scope.loadSupplier();
    
        $scope.statusList = commonRepository.getAStatus();
        
        //$scope.purchasOrder = $cookieStore.get('edipurchasOrder');
        //if ($scope.purchasOrder.SPRID) {
        //    $scope.loadSprDtls();
        //}
    }
    $scope.clearData = function () {
        debugger
        $scope.purchaseOrder = {};
        $scope.Reload();
        $cookieStore.put('editpurchaseOrder', $scope.purchaseOrder);
    }

    $scope.loadDepartment = function () {

        $scope.deptList = [];
        settingRepository.loadDepartment().then(function (response) {
            if (response.data) {
                $scope.deptList = response.data;
            }
        })
    }
    $scope.loadStore = function () {
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.storeList = response.data;
            }
        })
    }

    $scope.editRow = function (row) {
        debugger
        $scope.purchaseOrder = row;
        //$scope.reqWisePODtls();
        $cookieStore.put('editpurchaseOrder', $scope.purchaseOrder);
        $location.path("/PurchaseOrderCreate");

    }


    $scope.loadSprList = function () {
        debugger
        $scope.sprList = [];
     
        inventoryRepository.loadSprBy3rdApp().then(function (response) {
                if (response.data && response.data.length > 0) {
                    debugger
                    $scope.sprList = response.data;
                }
            })
    }


    $scope.loadSpr = function () {
        debugger
        $scope.sprList = [];
        if ($scope.purchaseOrder.FrmRequiredDate && $scope.purchaseOrder.ToRequiredDate) {
            inventoryRepository.loadSprByDate($scope.purchaseOrder.sPRID, $scope.purchaseOrder.FrmRequiredDate, $scope.purchaseOrder.ToRequiredDate).then(function (response) {
                if (response.data && response.data.length >0) {
                    debugger
                    $scope.sprList = response.data;
                    $scope.hideData = true;
                }
               else{
                    $scope.hideData = false;
                    //$scope.sprDtls = [];
                }
                
            })
        }
    
    }
    $scope.savePurchasOrder = function () {
        debugger
        if ($scope.PoForm.$valid) {
            var saveType = inventoryRepository.savePurchase($scope.purchaseOrder, $scope.sprDtlss, $scope.deletePODtlsID).then(function (response) {
                if (response.data.isSucess) {
                    debugger
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
            toastr.error("Please fill-up all required field !!!");
        }
    }


    
    $scope.savePendingPurchase = function () {
        debugger
        if ($scope.PoForm.$valid) {
            $scope.check = 1;
            var saveType = inventoryRepository.savePendingPurchase($scope.purchaseOrder, $scope.sprDtlss, $scope.deletePODtlsID, $scope.check).then(function (response) {
                if (response.data.isSucess) {
                    debugger
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
            toastr.error("Please fill-up all required field !!!");
        }
    }

    //===================================== PO Report ==================================================
    $scope.parameters = {};
    $scope.reportView = function (row) {

        var url = '/Inventory/PurchaseOrder/POReport';
        $scope.parameters.POID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    //==================================================================================================
   
    $scope.removePORow = function (index, details) {
        debugger
        $scope.sprDtls.splice(index, 1);
      

    }
    $scope.deletePODtlsID = [];
    $scope.removePORow2 = function (index, details) {
        debugger
        $scope.sprDtlss.splice(index, 1);
        if (details.PODtlsID > 0) {
            $scope.deletePODtlsID.push(details.PODtlsID);
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

    $scope.loadSprFrPendingPO = function () {

        $scope.pendingSprList = [];
        inventoryRepository.loadSprFrPendingPO().then(function (response) {
            if (response.data) {
                debugger
                $scope.pendingSprList = response.data;
            }
        })
    }


    $scope.total = {};
 
    $scope.checkRemainQty = function (row) {
        debugger
        $scope.total.GrandTotal = 0;
        if (row.OrderQty > row.ApprovedQty) {

            $scope.disableBtn = true;
            row.OrderQty = " ";
            toastr.error("Order Quantity can not bigger then ApprovedQty Quantity");

        } else {
            debugger
            $scope.disableBtn = false;
            var ReaminQty = (row.ApprovedQty) - (row.OrderQty);
            var LineTotal = (row.OrderQty) * (row.UnitRate);
           

            for (var i = 0; i < $scope.sprDtls.length; i++) {
                if ($scope.sprDtls[i].Index == row.Index) {
                    $scope.sprDtls[i].CategoryID = row.CategoryID;
                    $scope.sprDtls[i].ProductID = row.ProductID;
                    $scope.sprDtls[i].UnitID = row.UnitID;
                    //$scope.sprDtls[i].ApprovedQty = 0;

                    $scope.sprDtls[i].OrderQty = row.OrderQty;
                    $scope.sprDtls[i].ReaminQty = ReaminQty;
                    $scope.sprDtls[i].LineTotal = LineTotal;
                    
                    $scope.sprDtls[i].Remarks = row.Remarks;
                }
                if ($scope.sprDtls[i].LineTotal) {
                    $scope.total.GrandTotal = parseInt($scope.total.GrandTotal) + $scope.sprDtls[i].LineTotal;
                }
                
            }
            
           
        }

    }

    $scope.checkRemainQtyFrPending = function (row) {
        debugger
        $scope.total.GrandTotal = 0;
        if (row.OrderQty > row.ReaminQty) {

            $scope.disableBtn = true;
            row.OrderQty = " ";
            toastr.error("Order Quantity can not bigger then ReaminQty Quantity");

        } else {
            debugger
            $scope.disableBtn = false;
            //var ReaminQty = (row.ReaminQty) - (row.OrderQty);
            var LineTotal = (row.OrderQty) * (row.UnitRate);


            for (var i = 0; i < $scope.sprDtls.length; i++) {
                if ($scope.sprDtls[i].Index == row.Index) {
                    $scope.sprDtls[i].CategoryID = row.CategoryID;
                    $scope.sprDtls[i].ProductID = row.ProductID;
                    $scope.sprDtls[i].UnitID = row.UnitID;
                    //$scope.sprDtls[i].ApprovedQty = 0;

                    $scope.sprDtls[i].OrderQty = row.OrderQty;
                    $scope.sprDtls[i].ApprovedQty = row.ReaminQty;
                    $scope.sprDtls[i].LineTotal = LineTotal;

                    $scope.sprDtls[i].Remarks = row.Remarks;
                }
                if ($scope.sprDtls[i].LineTotal) {
                    $scope.total.GrandTotal = parseInt($scope.total.GrandTotal) + $scope.sprDtls[i].LineTotal;
                }

            }


        }

    }

    $scope.checkRemainQty2 = function (row) {
        debugger
          $scope.GrandTotal = 0;
        if (row.OrderQty > row.ApprovedQty) {
            toastr.error("Order Quantity can not bigger then ApprovedQty Quantity");
            $scope.disableBtn = true;
            row.OrderQty = " ";
        } else {
            debugger
            $scope.disableBtn = false;
            var ReaminQty = (row.ApprovedQty) - (row.OrderQty);
            var LineTotal = (row.OrderQty) * (row.UnitRate);


            for (var i = 0; i < $scope.sprDtlss.length; i++) {
                if ($scope.sprDtlss[i].Index == row.Index) {
                    $scope.sprDtlss[i].CategoryID = row.CategoryID;
                    $scope.sprDtlss[i].ProductID = row.ProductID;
                    $scope.sprDtlss[i].UnitID = row.UnitID;
                    //$scope.sprDtls[i].ApprovedQty = 0;

                    $scope.sprDtlss[i].OrderQty = row.OrderQty;
                    $scope.sprDtlss[i].ReaminQty = ReaminQty;
                    $scope.sprDtlss[i].LineTotal = LineTotal;

                    $scope.sprDtlss[i].Remarks = row.Remarks;
                }
                if ($scope.sprDtlss[i].LineTotal) {
                    $scope.GrandTotal = parseInt($scope.GrandTotal) + $scope.sprDtlss[i].LineTotal;
                }

            }


        }

    }

    $scope.checkRemainQtyFrPending2 = function (row) {
        debugger
        $scope.GrandTotal = 0;
        if (row.OrderQty > row.ReaminQty) {
            toastr.error("Order Quantity can not bigger then ReaminQty Quantity");
            $scope.disableBtn = true;
            row.OrderQty = " ";
        } else {
            debugger
            $scope.disableBtn = false;
            //var ReaminQty = (row.ReaminQty) - (row.OrderQty);
            var LineTotal = (row.OrderQty) * (row.UnitRate);


            for (var i = 0; i < $scope.sprDtlss.length; i++) {
                if ($scope.sprDtlss[i].Index == row.Index) {
                    $scope.sprDtlss[i].CategoryID = row.CategoryID;
                    $scope.sprDtlss[i].ProductID = row.ProductID;
                    $scope.sprDtlss[i].UnitID = row.UnitID;
                    //$scope.sprDtls[i].ApprovedQty = 0;

                    $scope.sprDtlss[i].OrderQty = row.OrderQty;
                    $scope.sprDtlss[i].ReaminQty = row.ReaminQty;
                    $scope.sprDtlss[i].LineTotal = LineTotal;

                    $scope.sprDtlss[i].Remarks = row.Remarks;
                }
                if ($scope.sprDtlss[i].LineTotal) {
                    $scope.GrandTotal = parseInt($scope.GrandTotal) + $scope.sprDtlss[i].LineTotal;
                }

            }


        }

    }


    $scope.sprDtlss = [];
    $scope.GrandTotal = 0;
    $scope.addRow = function (row) {
        debugger
        if (row.OrderQty <=0 || row.UnitRate <=0) {
            return;
        }
        var list = $filter('filter')($scope.sprList, function (d) { return d.SPRID === $scope.purchaseOrder.SPRID })[0];
        $scope.purchaseOrder.SPRNO = list.SPRNO;

        for (i = 0; i < $scope.sprDtlss.length;i++){
            if ($scope.sprDtlss[i].ProductID == row.ProductID && $scope.sprDtlss[i].UnitID == row.UnitID && $scope.sprDtlss[i].CategoryID) {
                return;
            }
        }
        if(row.ApprovedQtys){
            row.ApprovedQty = row.ApprovedQtys
        }
        $scope.GrandTotal += $scope.total.GrandTotal;
        $scope.sprDtlss.push({
            CategoryID:row.CategoryID,
            ProductID : row.ProductID,
            UnitID: row.UnitID,
            StockQty: row.StockQty,
            CategoryName:row.CategoryName,
            UnitName: row.UnitName,
            //ApprovedQty: row.ApprovedQty,
            ApprovedQty: 0,
            ProductName:row.ProductName,
            OrderQty : row.OrderQty,
            ReaminQty: row.ReaminQty,
            UnitRate:row.UnitRate,
            LineTotal: row.LineTotal,
            Remarks: row.Remarks,
            SPRDtlsID: row.SPRDtlsID,
            PODtlsID:row.PODtlsID,
            SPRNO: $scope.purchaseOrder.SPRNO

        });
        //$scope.hideData = false;

    }
    $scope.pOID = 0;
    $scope.loadPurchase = function () {

        $scope.purchaseOrderList = [];
        inventoryRepository.loadPurchaseOrder($scope.pOID, $scope.parameters).then(function (response) {
            if (response.data) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].PODate != null) {
                        var PODate = response.data[i].PODate.replace('/Date(', '').replace(')/', '');
                        response.data[i].PODate = $filter('date')(PODate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].DueDate != null) {
                        var DueDate = response.data[i].DueDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].DueDate = $filter('date')(DueDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].FirstApproveStatus == "A") {
                        response.data[i].endAppBtn = true;
                    } else {
                        response.data[i].endAppBtn = false;
                    }
                }
                $scope.purchaseOrderList = response.data;
            }
        })
    }
    $scope.loadLoginBranchID = function () {
        debugger

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                debugger
                $scope.purchaseOrder.BranchID = response.data.branchsID;
                $scope.purchaseOrder.UserFullName = response.data.userName;
                $scope.purchaseOrder.Department = response.data.department;
            }
        })
    }




    $scope.loadPODtlsList = function () {
        debugger

    
        $scope.sprDtls = [];
        inventoryRepository.loadPODtlsList($scope.purchaseOrder.POID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sprDtls = response.data;
                $scope.sprDtlss = response.data;
                
                for (i = 0; i < $scope.sprDtls.length; i++) {
                    $scope.sprDtls[i].Index = [i];
                }
                $scope.hideData = true;
                for (i = 0; i < $scope.sprDtls.length; i++) {

                    if ($scope.sprDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.purchaseOrder.BranchID, $scope.sprDtls[i].CategoryID, $scope.sprDtls[i].ProductID, $scope.sprDtls[i].UnitID).then(function (response) {
                            if (response.data) {
                                debugger
                                $scope.Id = response.data.ProductID;
                                for (j = 0; j < $scope.sprDtls.length; j++) {
                                    if ($scope.Id == $scope.sprDtls[j].ProductID) {
                                        $scope.sprDtls[j].StockQty = response.data.Quantity;
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

    $scope.reqWisePODtls = function () {
        debugger
        if ($scope.purchaseOrder.POID) {

            $scope.sprDtlss = [];
        }

            $scope.sprDtls = [];
      
            inventoryRepository.loadSprDtlsList($scope.purchaseOrder.SPRID).then(function (response) {
                if (response.data) {
                    debugger
                    $scope.sprDtls = response.data;
                    for (i = 0; i < $scope.sprDtls.length; i++) {
                        $scope.sprDtls[i].Index = [i];
                    }
                    $scope.hideData = true;




                    for (i = 0; i < $scope.sprDtls.length; i++) {

                        if ($scope.sprDtls[i].ProductID != null) {
                            debugger
                            inventoryRepository.loadStockQty($scope.purchaseOrder.BranchID, $scope.sprDtls[i].CategoryID, $scope.sprDtls[i].ProductID, $scope.sprDtls[i].UnitID).then(function (response) {
                                if (response.data) {
                                    debugger
                                    $scope.Id = response.data.ProductID;
                                    for (j = 0; j < $scope.sprDtls.length; j++) {
                                        if ($scope.Id == $scope.sprDtls[j].ProductID) {
                                            $scope.sprDtls[j].StockQty = response.data.Quantity;
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



    $scope.reqWisePendingPODtls = function () {
        debugger
        if ($scope.purchaseOrder.POID) {

            $scope.sprDtlss = [];
        }

        $scope.sprDtls = [];

        inventoryRepository.loadSprDtlsListFrPendingPO($scope.purchaseOrder.SPRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.sprDtls = response.data;
                for (i = 0; i < $scope.sprDtls.length; i++) {
                    $scope.sprDtls[i].Index = [i];
                }
                $scope.hideData = true;




                for (i = 0; i < $scope.sprDtls.length; i++) {

                    if ($scope.sprDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.purchaseOrder.BranchID, $scope.sprDtls[i].CategoryID, $scope.sprDtls[i].ProductID, $scope.sprDtls[i].UnitID).then(function (response) {
                            if (response.data) {
                                debugger
                                $scope.Id = response.data.ProductID;
                                for (j = 0; j < $scope.sprDtls.length; j++) {
                                    if ($scope.Id == $scope.sprDtls[j].ProductID) {
                                        $scope.sprDtls[j].StockQty = response.data.Quantity;
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


    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_PO";
        var fieldName = "PONO";
        var prefix = "PON";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.purchaseOrder.PONO = response.data.message;
            }
        })
    }
});