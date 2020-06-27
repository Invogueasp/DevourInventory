angular.module('app').controller('itemIssueController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, settingRepository,commonRepository, inventoryRepository) {

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
    //End :: Common Function
    //Tab Section
    $scope.tab = 1;
    $scope.setTab = function (tabId) {
        $scope.tab = tabId;
    };
    $scope.isSet = function (tabId) {
        return $scope.tab === tabId;
    };
    $scope.purchaseReq = {};
    $scope.purchaseReq.RequisitionNO = "PR001";

    $scope.loadDropdowns = function () {
        $scope.loadDepartment();
        $scope.loadStore();
        $scope.loadLoginBranchID();
    }
    $scope.clearData = function () {
        debugger
        $scope.companyBranch = {};
        $scope.Reload();
        $cookieStore.put('companyBranch', $scope.companyBranch);
    }

    // Purchase Requisition approval

    // Modal Open

    $scope.issueItem = {};

    $scope.isShiftModal = false;
    $scope.shiftClose = function () {
        $scope.isShiftModal = !$scope.isShiftModal;
    }
    $scope.viewIssueApprovalRow = function (row) {
        debugger
        $scope.issueItem = row;
        $scope.loadStoreDtls();
        $scope.loadIssue();
        $scope.isShiftModal = !$scope.isShiftModal;
    }
    
    $scope.readonly = false;
    $scope.disableButton = true;
    $scope.loadStoreDtls = function () {
        debugger
        $scope.storeReqDtls = [];
       
        inventoryRepository.loadStoreReqDtls($scope.issueItem.SRID).then(function (response) {
            if (response.data) {
                debugger
                $scope.storeReqDtls = response.data;
                $scope.issueDtl = response.data;

                //$scope.disableButton = false;
                for (i = 0; i < $scope.storeReqDtls.length; i++) {


                    if ($scope.storeReqDtls[i].ProductID != null) {
                        debugger
                        inventoryRepository.loadStockQty($scope.issue.BranchID, $scope.storeReqDtls[i].CategoryID, $scope.storeReqDtls[i].ProductID, $scope.storeReqDtls[i].UnitID).then(function (response) {
                            if (response.data) {
                                debugger
                                $scope.Id = response.data.ProductID;
                                for (j = 0; j < $scope.storeReqDtls.length; j++) {
                                    if ($scope.Id == $scope.storeReqDtls[j].ProductID) {
                                        $scope.storeReqDtls[j].StockQty = response.data.Quantity;
                                        
                                        if ($scope.storeReqDtls[j].StockQty > 0) {

                                            $scope.disableButton = false;
                                        }
                                    

                                    }

                                }

                                //$scope.storeReqDtls[i].StockQty = 5;
                            }
                         
                        })
                   
                    }
                    $scope.storeReqDtls[i].StockQty = 0;
                    if ($scope.storeReqDtls[i].StockQty == 0) {

                        $scope.disableButton = true;
                    }

                    if ($scope.storeReqDtls[i].IssueQty == $scope.storeReqDtls[i].ApprovedQty) {
                        $scope.readonly = true;
                        


                    } else {
                        $scope.readonly = false;
                       
                    }
                  
                  
                }


            }
        })
    }
    $scope.loadDepartment = function () {

        $scope.deptList = [];
        settingRepository.loadDepartment().then(function (response) {
            if (response.data) {
                $scope.deptList = response.data;
            }
        })
    }
    $scope.loadIssue = function () {
        debugger
       
        inventoryRepository.loadIssue($scope.issueItem.SRID).then(function (response) {
            if (response.data) {
                debugger
               
                    if (response.data.IssueDate != null) {
                        var IssueDate = response.data.IssueDate.replace('/Date(', '').replace(')/', '');
                        $scope.issueItem.IssueDate = $filter('date')(IssueDate, "dd-MMM-yyyy");
                    }
                    if (response.data.IssueNO != null) {
                        $scope.issueItem.IssueNO = response.data.IssueNO;
                    }
                    if (response.data.IssueID != null) {
                        $scope.issueItem.IssueID = response.data.IssueID;

                    } else {
                        $scope.getNewPersonnelCode();
                    }
                    if (response.data.SRID != null) {
                        $scope.issueItem.SRID = response.data.SRID;
                    }
                 
                //$scope.issueList = response.data;

            }
            else {
                $scope.getNewPersonnelCode();
            }
        })
    }
    $scope.issueDtl = [];
    $scope.disableButton = false;
    $scope.issueDtls = function (row) {
        debugger
        $scope.disableButton = false;
        if (row.IssueQty > row.ApprovedQty) {
            $scope.disableButton = true;
            toastr.error("Issue Quantity can not bigger then ApprovedQty Quantity");
            return;
        }
        else {
            for (var i = 0; i < $scope.issueDtl.length; i++) {
                if ($scope.issueDtl[i].ProductID == row.ProductID) {
                    $scope.issueDtl[i].CategoryID = row.CategoryID;
                    $scope.issueDtl[i].ProductID = row.ProductID;
                    $scope.issueDtl[i].UnitID = row.UnitID;
                    $scope.issueDtl[i].ReqQty = row.ReqQty;
                    $scope.issueDtl[i].IssueQty = row.IssueQty;
                    $scope.issueDtl[i].Remarks = row.Remarks;
                }
            }

        }
        
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
    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_Issue";
        var fieldName = "IssueNO";
        var prefix = "ISN";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.issueItem.IssueNO = response.data.message;
            }
        })
    }

    $scope.saveIssueItem = function () {
        debugger
        $scope.issueItem.Status = "IF";
        for (var i = 0; i < $scope.issueDtl.length; i++) {
            if ($scope.issueDtl[i].ApprovedQty != $scope.issueDtl[i].IssueQty) {
                $scope.issueItem.Status = "IP";
            }
        }
        var saveType = inventoryRepository.saveIssue($scope.issueItem, $scope.issueDtl).then(function (response) {
            if (response.data.isSucess) {

                toastr.success(response.data.message);
                $scope.shiftClose();
                $scope.searchSR();

            } else {
                if (response.data.message == "LogOut") {
                    $location.path('#!/Login')
                }
                toastr.error(response.data.message);
            }
        })

    }
    $scope.issue = {};
    $scope.hideData = false;
    $scope.searchSR = function (id) {
        debugger
        $scope.storeReqList = [];
        if ($scope.issue.BranchID) {
            inventoryRepository.loadStoreReq($scope.issue.BranchID, $scope.issue.DepartmentID, $scope.issue.fromDate, $scope.issue.toDate).then(function (response) {
                if (response.data) {
                    debugger

                    for (i = 0; i < response.data.length; i++) {
                        if (response.data[i].RequisitionDate != null) {
                            var RequisitionDate = response.data[i].RequisitionDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].RequisitionDate = $filter('date')(RequisitionDate, "dd-MMM-yyyy");
                        }
                        if (response.data[i].RequiredDate != null) {
                            var RequiredDate = response.data[i].RequiredDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].RequiredDate = $filter('date')(RequiredDate, "dd-MMM-yyyy");
                        }
                        if (response.data[i].CreatedDate != null) {
                            var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                        }

                        if (response.data[i].Status == "2A" || response.data[i].Status == "IP") {

                            response.data[i].IssueStatus = "Pending";
                        }
                        
                    }
                    $scope.hideData = true;
                    $scope.storeReqList = response.data;
                }
            })
        }
        
    }




    $scope.loadLoginBranchID = function () {
        debugger

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                debugger
                $scope.issue.BranchID = response.data.branchsID;

                $scope.issue.UserName = response.data.userName;
                if ($scope.issue.BranchID) {
                    $scope.searchSR();
                }
            }
        })
    }






});
