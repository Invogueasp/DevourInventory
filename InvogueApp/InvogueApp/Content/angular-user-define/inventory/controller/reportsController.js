angular.module('app').controller('reportsController', function ($scope, $filter, $location, $route, commonRepository, settingRepository, inventoryRepository, $templateCache, $cookieStore) {

    //start :: ng-idel => when user idel specific time then it will be auto logout
    $scope.$on('IdleStart', function () {
        toastr.warning('your session will be end soon!!');
    });

    $scope.$on('IdleTimeout', function () {
        window.location = "/Security/LogOff#!/";
    });

    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }

    //===================================== Inventory Reports ==================================================
    //------------------------------ Store Requisition And Issue Note ------------------------------------------ 
    $scope.parameters = {};
    $scope.reportView = function (row) {
        debugger
        var url = '/Reports/TechnoReports/SRandIssueReportView';
        $scope.parameters.SRID = row;
        commonRepository.viewMultiParameterDocument($scope.parameters, 'Do you want to view this Report ?', url);
    }

    $scope.details = {};
    $scope.searchUnitDropDown = function () {
        debugger
        if ($scope.unitName != null && $scope.unitName.length >= 3) {
            var searchInput = $scope.unitName.trim().length;
            if (searchInput > 0) {

              
                inventoryRepository.loadSRIDBySRNO($scope.unitName).then(function (response) {
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

    
        $scope.reportView(unit.SRID);
    }




});