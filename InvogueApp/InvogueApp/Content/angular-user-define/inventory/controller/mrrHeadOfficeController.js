angular.module('app').controller('mrrHeadOfficeController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });
    $scope.removeCookies = function () {
        debugger
        $scope.mMr = {};
        $cookieStore.put('editmMr', $scope.mMr);
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }


    $scope.parameters = {};
    $scope.mMr = {};
    $scope.loadParameters = function () {
        $scope.parameters.FormDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.parameters.ToDate = $filter('date')(Date.now(), 'dd-MMM-yyyy');
        $scope.loadStore();
        $scope.loadDepartment();
    }

    $scope.loadDropdowns = function () {
        debugger

        $scope.loadLoginBranchID();
        $scope.loadStore();

        $scope.loadAppPurchase();
        $scope.mMr = $cookieStore.get('editmMr');
        if ($scope.mMr.MRRID) {
            $scope.getMrrImgFiles($scope.mMr.MRRID);
            $scope.loadAppPurchaseOrderDtlsForEdit();
        }
        if ($scope.mMr.POID) {
            $scope.loadAppPurchaseOrderDtls();
            $scope.supplierSelectByOrder();
        } else {
            $scope.getNewPersonnelCode();
        }
    }
    $scope.clearData = function () {
        debugger
        $scope.mMr = {};
        $scope.Reload();
        $cookieStore.put('editmMr', $scope.mMr);
    }
    $scope.editRow = function (row) {
        debugger
        if (row.Status != null || row.Status == "F") {
            $scope.end3AppBtn = false;

        } else {
            $scope.end3AppBtn = true;
        }

        $scope.mMr = row;

        //$scope.reqWisePODtls();
        $cookieStore.put('editmMr', $scope.mMr);
       
        $location.path("/MRRHeadOfficeCreate");

    }
    $scope.loadLoginBranchID = function () {

        inventoryRepository.loadLoginBranchID().then(function (response) {
            if (response.data) {
                $scope.mMr.BranchID = response.data.branchsID;
                $scope.mMr.UserFullName = response.data.userName;
                $scope.mMr.Department = response.data.department;
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
    $scope.loadStore = function () {
        $scope.storeList = [];
        settingRepository.loadStore().then(function (response) {
            if (response.data) {
                $scope.storeList = response.data;
            }
        })
    }
    $scope.mRRID = 0;
    $scope.loadmRRList = function () {
        debugger
        $scope.mRRList = [];
        inventoryRepository.loadMRRHeadOffice($scope.mRRID, $scope.parameters).then(function (response) {
            if (response.data && response.data.length > 0) {
                debugger
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].MRRDate != null) {
                        var MRRDate = response.data[i].MRRDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].MRRDate = $filter('date')(MRRDate, "dd-MMM-yyyy");
                    }

                    if (response.data[i].CreatedDate != null) {
                        var CreatedDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(CreatedDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].Status == "F") {
                        
                        response.data[i].end3AppBtn = false;
                    } else {
                        response.data[i].end3AppBtn = true;
                    }

                    if (response.data[i].ImageURL == "N") {
                      
                        response.data[i].viewAttachment = false;

                    }
                    else {
                        response.data[i].viewAttachment = true;
                    }
                }
                $scope.mRRList = response.data;
            }
        })
    }
    $scope.allData = {};
 

    $scope.saveMRR = function () {
        debugger
        $scope.allData = {
            mRrDtls: $scope.loadAppPurchaseOrderDtlss,
            mRr: $scope.mMr,
            deletepDtlsID: $scope.deleteDtlsID
        };
        if ($scope.MRRForm.$valid) {
            var saveType = inventoryRepository.saveMRRHeadOffice($scope.selectFileUpload, $scope.allData).then(function (response) {
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
            toastr.error("Please fill-up all required field !!!");
        }
    }
    //===================================== MRR Report ==================================================
    $scope.parameters = {};
    $scope.reportView = function (row) {

        var url = '/Inventory/MRRHeadOffice/MaterialReceiveHOReport';
        $scope.parameters.MRRID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    //==================================================================================================
    $scope.deleteDtlsID = [];
    $scope.removeRow = function (index, details) {
        debugger
        $scope.total.GrandTotal -= $scope.loadAppPurchaseOrderDtlss[index].LineTotal;
        $scope.loadAppPurchaseOrderDtlss.splice(index, 1);
        if (details.MRRDtlsID > 0) {
            $scope.deleteDtlsID.push(details.MRRDtlsID);
        }

        // calculate grand total
        


    }

    $scope.supplierSelectByOrder = function () {
        debugger
        var list = $filter('filter')($scope.appPurchaseOrderList, function (d) { return d.POID === $scope.mMr.POID })[0];
        return $scope.mMr.SupplierID = list.SupplierID;

    }


 


    $scope.selectFileUpload = "";
    $scope.stepsModel = "";
    $scope.singleImageUpload = function (event) {
        debugger
        var files = event.target.files; //FileList object
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var fileName = file.name;
            var index = fileName.lastIndexOf(".");
            var ext = fileName.substring(index, fileName.length).toUpperCase();
            if (ext == '.PNG' || ext == '.JPEG' || ext == '.JPG' || ext == '.GIF' || ext == '.PDF') {
                if (file.size > 307200 || file.fileSize > 307200) {
                    toastr.error("Allowed file size exceeded. (Max. 300KB)");
                    angular.element("input[type='file']").val(null);
                } else {
                    $scope.dbImage = false;
                    $scope.createImage = true;
                    $scope.selectFileUpload = file;

                    var reader = new FileReader();

                    reader.onload = function (event) {
                        $scope.stepsModel = event.target.result;
                        $scope.$apply();
                    }
                    // when the file is read it triggers the onload event above.
                    reader.readAsDataURL(file);
                }

            } else {
                toastr.error("Allowed image formats are: jpg, jpeg, png, pdf");
                $scope.personnelInfo.Photo = '';
            }
        }
    }
  
    $scope.getMrrImgFiles = function (mrrID) {
        
        $scope.productImg = [];
        inventoryRepository.getMrrImgFile(mrrID).then(function (response) {
            if (response.data) {
                debugger
                for (var i = 0; i < response.data.length; i++) {
                    response.data[i].FullName = $scope.mMr.MRRID;
                }
                $scope.dbImage = true;
                $scope.productImg = response.data;
  
            }
        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }






    $scope.total = {};

    $scope.checkRemainQty = function (row) {
        $scope.total.GrandTotal = 0;
        if (row.ReceiveQty > row.ApprovedQty) {
            toastr.error("Order Quantity can not bigger then ApprovedQty Quantity");
        } else {
            debugger

            var LineTotal = (row.ReceiveQty) * (row.UnitRate);
            //var Discount = (LineTotal) - (row.Discount);

            for (var i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                if ($scope.loadAppPurchaseOrderDtlss[i].Index == row.Index) {
                    $scope.loadAppPurchaseOrderDtlss[i].CategoryID = row.CategoryID;
                    $scope.loadAppPurchaseOrderDtlss[i].ProductID = row.ProductID;
                    $scope.loadAppPurchaseOrderDtlss[i].UnitID = row.UnitID;
                    $scope.loadAppPurchaseOrderDtlss[i].ReceiveQty = row.ReceiveQty;
                    $scope.loadAppPurchaseOrderDtlss[i].UnitRate = row.UnitRate;

                    $scope.loadAppPurchaseOrderDtlss[i].OrderQty = row.OrderQty;
                    $scope.loadAppPurchaseOrderDtlss[i].PODtlsID = row.PODtlsID;
                    $scope.loadAppPurchaseOrderDtlss[i].LineTotal = LineTotal;

                    //$scope.loadAppPurchaseOrderDtlss[i].GrandTotal = $scope.loadAppPurchaseOrderDtlss[i].GrandTotal + LineTotal;
                }


                if ($scope.loadAppPurchaseOrderDtlss[i].LineTotal) {
                    $scope.total.GrandTotal = parseInt($scope.total.GrandTotal) + $scope.loadAppPurchaseOrderDtlss[i].LineTotal;
                }

            }

        }

    }
    $scope.loadAppPurchase = function () {

        $scope.appPurchaseOrderList = [];
        inventoryRepository.loadAppPurchaseOrder().then(function (response) {
            if (response.data) {
                debugger

                $scope.appPurchaseOrderList = response.data;
            }
        })
    }
    $scope.hideData = false;
    $scope.loadAppPurchaseOrderDtls = function () {

        $scope.loadAppPurchaseOrderDtlss = [];
        $scope.total.GrandTotal = 0;

        inventoryRepository.loadMrrHODtls($scope.mMr.POID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];


                    if ($scope.loadAppPurchaseOrderDtlss[i].MRRDtlsID) {
                        $scope.loadAppPurchaseOrderDtlss[i].MRRDtlsID = 0;
                    }

                    $scope.total.GrandTotal += $scope.loadAppPurchaseOrderDtlss[i].LineTotal;
                }                
                $scope.hideData = true;
            }
        })
    }

    $scope.loadAppPurchaseOrderDtlsForEdit = function () {

        $scope.loadAppPurchaseOrderDtlss = [];
        inventoryRepository.loadMrrHODtls($scope.mMr.POID).then(function (response) {
            if (response.data) {
                debugger
                $scope.loadAppPurchaseOrderDtlss = response.data;
                for (i = 0; i < $scope.loadAppPurchaseOrderDtlss.length; i++) {
                    $scope.loadAppPurchaseOrderDtlss[i].Index = [i];

                }

                $scope.hideData = true;
            }
        })
    }
    $scope.getNewPersonnelCode = function () {
        var tableName = "INV_MRR_HO";
        var fieldName = "MRRNO";
        var prefix = "MRH";
        debugger
        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                debugger
                $scope.mMr.MRRNO = response.data.message;
            }
        })
    }
    //========================================== GatePass Report =======================================================
    $scope.GatePassView = function (row) {
        debugger
        $scope.reportPrint(row);
    }
    $scope.isModal = false;
    $scope.modalClose = function () {
        debugger
        $scope.isModal = !$scope.isModal;

    }
    $scope.report = {};
    $scope.Report = function (row) {
        $scope.reportDtlsList = [];
        $scope.isModal = !$scope.isModal;
        inventoryRepository.loadReportList(row.POID).then(function (response) {
            if (response.data) {
                debugger
                $scope.reportDtlsList = response.data.mrRDtlsLists;
                $scope.report.PONO = response.data.PONO;
                $scope.report.MRRNO = response.data.MRRNO;

                
            }
        });

    }

    $scope.DeliveryChallanView = function (row) {

        var url = '/Inventory/MRRHeadOffice/DeliveryChallanReportView';
        $scope.parameters.POID = row.POID;
        $scope.parameters.BranchID = row.BranchID;
        $scope.parameters.SPRID = row.SPRID;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
    $scope.parameters = {};
    $scope.reportPrint = function (row) {
        debugger
        var url = '/Inventory/MRRHeadOffice/GatePassReportView';
        $scope.parameters.POID = row.POID;
        $scope.parameters.SPRID = row.SPRID;
        $scope.parameters.BranchID = row.BranchID;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }
  
});