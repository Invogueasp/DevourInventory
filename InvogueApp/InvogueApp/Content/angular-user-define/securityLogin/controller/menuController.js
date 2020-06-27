
angular.module('app').controller('menuController', function ($scope, $http, $rootScope) {
    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    //End :: ng-idel

    ////Hide all menu on first time page load     
    //All Module
    $scope.groupID = 0;
    $scope.Security = false;
    $scope.Commercial = false;
    $scope.Setup = false;
    $scope.Reports = false;
    $scope.Inventory = false;
    $scope.CommonUser = true;

    //Sub Module
    $scope.SecurityMaster = false;
    $scope.Dashboard = false;
    $scope.User = false;
    $scope.Page = false;
    $scope.PagePermission = false;
    $scope.ChangePasswordByAdmin = false;
    $scope.ChangeOwnPassword = false;

    //commercial
    $scope.PurchaseRequisitionList = false;
    $scope.SPRApprovalList = false;
    $scope.SPR2ndApprovalList = false;
    $scope.SPR3rdApprovalList = false;
    $scope.PurchaseOrderList = false;
    $scope.POApprovalList = false;
    $scope.PO2ndApprovalList = false;

    $scope.MRRHeadOfficeList = false;
    $scope.MRSList = false;
    $scope.QCList = false;
    $scope.MRRList = false;

    $scope.ItemReturnList = false;
    $scope.IRApprovalList = false;
    //Inventory

    $scope.StoreRequisitionList = false;
    $scope.SRApprovalList = false;
    $scope.SR2ndApprovalList = false;
    $scope.IssueList = false;
    $scope.ScrapReturnList = false;
    $scope.CurrentStockList = false;


    //Settings
    $scope.CategoryList = false;
    $scope.SubCategoryList = false;
    $scope.DepartmentList = false;
    $scope.UnitList = false;
    $scope.ProductList = false;
    $scope.UploadProduct = false;
    $scope.StoreRackList = false;
    $scope.StoreBinList = false;
    $scope.SupplierList = false;
    $scope.Machine = false;
    $scope.Model = false;
    $scope.Company = false;
    //Reports
    $scope.SRandIssueReport = false;
    //Attendance
    $scope.DeviceAttendance = true;
    $scope.MenualAttendance = true;


    //Reports
    $scope.StockWithoutValue = false;
    $scope.StockWithValue = false;

    $scope.menuCount = {};
    $http.get('/Security/GetAllCountData').then(function (response) {
        debugger;     
        $scope.menuCount.sprCountData = response.data.sprApprovalCountData;
        $scope.menuCount.pOCountData = response.data.PurchaseOrderCountData;
        $scope.menuCount.pOFirstAppCountData = response.data.PurchaseOrderFirstAppCountData;
        $scope.menuCount.pOSecAppCountData = response.data.PurchaseOrderSecAppCountData;
        $scope.menuCount.mrrCountData = response.data.MaterialReceiveCountData;
        $scope.menuCount.qcCountData = response.data.QualityCertificateCountData;
        $scope.menuCount.mrrCountData = response.data.MaterialReceiveReportCountData;
        $scope.menuCount.irCountData = response.data.ItemReturnCountData;
        $scope.menuCount.stFirstCountData = response.data.StoreRequisitionFirstAppCountData;
        $scope.menuCount.stSecCountData = response.data.StoreRequisitionSecAppCountData;
        $scope.menuCount.itemIssueCountData = response.data.ItemIssueCountData;
    });
    $http.get('/Security/GetSiteMenu').then(function (response) {
        //When login SuperAdmin        
        $scope.groupID = response.data.userGroupID;
        if ($scope.groupID === 2) { // groupID 2 = SuperAdmin
            //Main Module Show
            $scope.Security = true;
            $scope.SetUp = true;
            //Sub Module Show
            $scope.User = true;
            $scope.CommonUser = true;
            $scope.Page = true;
            $scope.PagePermission = true;
            $scope.ChangePasswordByAdmin = true;
            $scope.ChangeOwnPassword = true;
        }
        else {
            $scope.menu = response.data.menu;
            ////Module and Sub Module Show
            for (var i = 0; i < $scope.menu.length; i++) {
                if ($scope.menu[i].Module == "Security")
                    $scope.Security = true;
                if ($scope.menu[i].Module == "HR")
                    $scope.HR = true;
                if ($scope.menu[i].Module == "Setup")
                    $scope.Setup = true;
                if ($scope.menu[i].Module == "Reports")
                    $scope.Reports = true;
                if ($scope.menu[i].Module == "Inventory")
                    $scope.Inventory = true;
                if ($scope.menu[i].Module == "Commercial")
                    $scope.Commercial = true;
                if ($scope.menu[i].Module == "CommonUser")
                    $scope.CommonUser = true;
            }

            ////Page Show
            for (var i = 0; i < $scope.menu.length; i++) {
                if ($scope.menu[i].Page == "SecurityMaster")
                    $scope.SecurityMaster = true;
                if ($scope.menu[i].Page == "Dashboard")
                    $scope.Dashboard = true;
                if ($scope.menu[i].Page == "User")
                    $scope.User = true;
                if ($scope.menu[i].Page == "Page")
                    $scope.Page = true;
                if ($scope.menu[i].Page == "PagePermission")
                    $scope.PagePermission = true;
                if ($scope.menu[i].Page == "ChangePasswordByAdmin")
                    $scope.ChangePasswordByAdmin = true;
                if ($scope.menu[i].Page == "ChangeOwnPassword")
                    $scope.ChangeOwnPassword = true;
                //Reports
                if ($scope.menu[i].Page == "SRandIssueReport")
                    $scope.SRandIssueReport = true;

                //Inventory
                if ($scope.menu[i].Page == "StoreRequisitionList")
                    $scope.StoreRequisitionList = true;
                if ($scope.menu[i].Page == "SRApprovalList")
                    $scope.SRApprovalList = true;
                if ($scope.menu[i].Page == "SR2ndApprovalList")
                    $scope.SR2ndApprovalList = true;
                if ($scope.menu[i].Page == "IssueList")
                    $scope.IssueList = true;
                if ($scope.menu[i].Page == "ScrapReturnList")
                    $scope.ScrapReturnList = true;
                if ($scope.menu[i].Page == "CurrentStockList")
                    $scope.CurrentStockList = true;
                //Settings
                if ($scope.menu[i].Page == "CategoryList")
                    $scope.CategoryList = true;
                if ($scope.menu[i].Page == "SubCategoryList")
                    $scope.SubCategoryList = true;
                if ($scope.menu[i].Page == "DepartmentList")
                    $scope.DepartmentList = true;
                if ($scope.menu[i].Page == "UnitList")
                    $scope.UnitList = true;
                if ($scope.menu[i].Page == "ProductList")
                    $scope.ProductList = true;
                if ($scope.menu[i].Page == "UploadProduct")
                    $scope.UploadProduct = true;

                if ($scope.menu[i].Page == "StoreRackList")
                    $scope.StoreRackList = true;
                if ($scope.menu[i].Page == "StoreBinList")
                    $scope.StoreBinList = true;
                if ($scope.menu[i].Page == "SupplierList")
                    $scope.SupplierList = true;
                if ($scope.menu[i].Page == "Company")
                    $scope.Company = true;
                if ($scope.menu[i].Page == "Machine")
                    $scope.Machine = true;
                if ($scope.menu[i].Page == "Model")
                    $scope.Model = true;



                //Commercial
                if ($scope.menu[i].Page == "PurchaseRequisitionList")
                    $scope.PurchaseRequisitionList = true;
                if ($scope.menu[i].Page == "SPRApprovalList")
                    $scope.SPRApprovalList = true;
                if ($scope.menu[i].Page == "SPR2ndApprovalList")
                    $scope.SPR2ndApprovalList = true;
                if ($scope.menu[i].Page == "SPR3rdApprovalList")
                    $scope.SPR3rdApprovalList = true;
                if ($scope.menu[i].Page == "PurchaseOrderList")
                    $scope.PurchaseOrderList = true;

                if ($scope.menu[i].Page == "POApprovalList")
                    $scope.POApprovalList = true;
                if ($scope.menu[i].Page == "PO2ndApprovalList")
                    $scope.PO2ndApprovalList = true;
                if ($scope.menu[i].Page == "MRRHeadOfficeList")
                    $scope.MRRHeadOfficeList = true;
                if ($scope.menu[i].Page == "MRSList")
                    $scope.MRSList = true;
                if ($scope.menu[i].Page == "QCList")
                    $scope.QCList = true;
                if ($scope.menu[i].Page == "MRRList")
                    $scope.MRRList = true;
                if ($scope.menu[i].Page == "ItemReturnList")
                    $scope.ItemReturnList = true;
                if ($scope.menu[i].Page == "IRApprovalList")
                    $scope.IRApprovalList = true;
                
               

                //Attendance
                if ($scope.menu[i].Page == "Attendance")
                    $scope.Attendance = true;
                
            }
        }
    });

    //Reports function
    //start code:: Print Customer List
    $scope.customerPrintDocument = function (id) {
        var reportUrl = '../Reports/SetupReports/CustomerList/';
        var transactionID = id;
        printDocument(reportUrl, transactionID); //function location asset/custom.js
    }

    //start code:: Print Supplier List
    $scope.supplierPrintDocument = function (id) {
        var reportUrl = '../Reports/SetupReports/SupplierList/';
        var transactionID = id;
        printDocument(reportUrl, transactionID); //function location asset/custom.js
    }

    //start code:: Print Product List
    $scope.productPrintDocument = function (id) {
        var reportUrl = '../Reports/SetupReports/ProductList/';
        var transactionID = id;
        printDocument(reportUrl, transactionID); //function location asset/custom.js
    }

});


