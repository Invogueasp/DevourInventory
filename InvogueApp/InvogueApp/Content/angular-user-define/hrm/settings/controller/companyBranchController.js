angular.module('app').controller('companyBranchController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, settingsRepository, hrmRepository, commonRepository) {

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
    $scope.companyBranch = {};
    $scope.branchDeptList = [];
    $scope.branchShiftList = [];
    $scope.editBranchDeptRowID = [];
    $scope.editBranchShiftRowID = [];
    $scope.loadDropdowns = function () {
        $scope.loadCompany();
        $scope.loadDepartment();
        $scope.loadShift();
        $scope.statusList = commonRepository.getAStatus();
        $scope.companyBranch = $cookieStore.get('companyBranch');
        if ($scope.companyBranch.BranchID != null) {
            $scope.loadBranchDeptDtls($scope.companyBranch.BranchID);
            $scope.loadBranchShiftDtls($scope.companyBranch.BranchID);
        }
    }
    $scope.loadCompany = function () {
        settingsRepository.loadCompany().then(function (response) {
            if (response.data) {
                $scope.companyList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.loadDepartment = function () {
        hrmRepository.loadDepartment().then(function (response) {
            if (response.data) {
                $scope.departmentList = response.data;
                //change Branch Department
                if ($scope.editBranchDeptList != null) {
                    for (var i = 0; i < $scope.departmentList.length; i++) {
                        var dept = $filter('filter')($scope.editBranchDeptList, function (d) { return d.DepartmentID === $scope.departmentList[i].DepartmentId; })[0];
                        if (dept != null && dept.DepartmentID > 0) {
                            $scope.departmentList[i].selected = true;
                            $scope.changeDepartmentID(dept.DepartmentID, dept.BranchDeptID);
                        }
                    }
                }
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.loadShift = function () {
        settingsRepository.loadShift().then(function (response) {
            if (response.data) {
                $scope.shiftList = response.data;
                //change Branch shift
                if ($scope.editBranchShiftList != null) {
                    for (var i = 0; i < $scope.shiftList.length; i++) {
                        var shift = $filter('filter')($scope.editBranchShiftList, function (d) { return d.ShiftID === $scope.shiftList[i].ShiftID; })[0];
                        if (shift != null && shift.ShiftID > 0) {
                            $scope.shiftList[i].selected = true;
                            $scope.changeShiftID(shift.ShiftID, shift.BranchShiftID);
                        }
                    }
                }
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    


    // Modal Open
    $scope.isShiftModal = false;
    $scope.shiftClose = function () {
        $scope.isShiftModal = !$scope.isShiftModal;
    }
    $scope.viewShiftRow = function (row) {
        $scope.isShiftModal = !$scope.isShiftModal;
        $scope.loadShiftDtls(row.ShiftID);
    }
    $scope.loadShiftDtls = function (shiftID) {
        settingsRepository.loadShiftDtls(shiftID).then(function (response) {
            if (response.data) {
                debugger 
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].Day) {
                        response.data[i].Day = commonRepository.convertWeekDay(response.data[i].Day);
                    }
                    if (response.data[i].StartTime) {
                        response.data[i].StartTime = commonRepository.convertTwelveHours(response.data[i].StartTime);
                    }
                    if (response.data[i].EndTime) {
                        response.data[i].EndTime = commonRepository.convertTwelveHours(response.data[i].EndTime);
                    }
                    
                }
                $scope.shiftDtlsList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.createDepartmentID = function (dept) {
        debugger 
        if (dept.selected == true) {
            $scope.branchDeptList.push({
                BranchDeptID: 0,
                BranchID: $scope.companyBranch.BranchID,
                DepartmentID: dept.DepartmentId
            });
        } else {
            for (var i = 0; i < $scope.branchDeptList.length; i++) {
                if ($scope.branchDeptList[i].DepartmentID == dept.DepartmentId) {
                    $scope.branchDeptList.splice(i, 1);
                    $scope.editBranchDeptRowID.push(dept.DepartmentId);
                }

            }
            
        }
    }
    $scope.createShiftID = function (shift) {
        if (shift.selected == true) {
            $scope.branchShiftList.push({
                BranchShiftID: 0,
                BranchID: $scope.companyBranch.BranchID,
                ShiftID: shift.ShiftID
            });
        } else {
            for (var i = 0; i < $scope.branchShiftList.length; i++) {
                
                if ($scope.branchShiftList[i].ShiftID == shift.ShiftID) {
                    $scope.branchShiftList.splice(i, 1);
                    $scope.editBranchShiftRowID.push(shift.ShiftID);
                }

            }
        }
    }
    //Change Edit Branch Department and Branch Shift List
    $scope.changeDepartmentID = function (dept, branchDeptID) {
        for (var i = 0; i < $scope.branchDeptList.length; i++) {
            if ($scope.branchDeptList[i] != null && $scope.branchDeptList[i].DepartmentID == dept) {
                $scope.branchDeptList[i].DepartmentID = dept;
                $scope.branchDeptList[i].BranchDeptID = branchDeptID;
                $scope.branchDeptList[i].BranchID = $scope.companyBranch.BranchID;
                return;
            }
        }
        $scope.branchDeptList.push({
            BranchDeptID: branchDeptID,
            BranchID: $scope.companyBranch.BranchID,
            DepartmentID: dept
        });
    }
    $scope.changeShiftID = function (shift, branchShiftID) {
        for (var i = 0; i < $scope.branchShiftList.length; i++) {
            if ($scope.branchShiftList[i] != null && $scope.branchShiftList[i].ShiftID == shift) {
                $scope.branchShiftList[i].ShiftID = shift;
                $scope.branchShiftList[i].BranchShiftID = branchShiftID;
                $scope.branchShiftList[i].BranchID = $scope.companyBranch.BranchID;
                return;
            }
        }
        $scope.branchShiftList.push({
            BranchShiftID: branchShiftID,
            BranchID: $scope.companyBranch.BranchID,
            ShiftID: shift
        });
        
    }
    $scope.saveCompanyBranch = function () {
        debugger
        if ($scope.CreateBranchForm.$valid) {
            if ($scope.companyBranch.BranchID < 1) {
                if ($scope.branchDeptList.length < 1 && $scope.branchShiftList.length < 1) {
                    toastr.error("Must be Branch Department Data And Branch Shift Data valid!!");
                    return;
                }
            }
            var createpage = settingsRepository.saveCompanyBranch($scope.companyBranch, $scope.branchDeptList, $scope.editBranchDeptRowID, $scope.branchShiftList, $scope.editBranchShiftRowID);
            createpage.then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    $scope.clearData();
                } else {
                    if (response.data.message === "LogOut") {
                        $location.path('#!/LogIn');
                    }
                    toastr.error(response.data.message);
                }
            }).catch(function (response) {
                toastr.error($scope.name += response.data + '!!');
            });
        } else {
            toastr.error("All input field are not valid");
        }
    }
    $scope.editRow = function (row) {
        $cookieStore.put('companyBranch', row);
        $location.path('/CreateCompanyBranch');
    }

    $scope.loadCompanyBranch = function () {
        settingsRepository.loadCompanyBranch().then(function (response) {
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].CreatedDate) {
                        var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                }
                $scope.companyBranchList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.loadBranchShiftDtls = function (branchID) {
        settingsRepository.loadBranchShiftDtls(branchID).then(function (response) {
            if (response.data) {
                $scope.editBranchShiftList = response.data;
                if ($scope.shiftList != null) {
                    for (var i = 0; i < $scope.shiftList.length; i++) {
                        var shift = $filter('filter')($scope.editBranchShiftList, function (d) { return d.ShiftID === $scope.shiftList[i].ShiftID; })[0];
                        if (shift != null && shift.ShiftID > 0) {
                            $scope.shiftList[i].selected = true;
                            $scope.changeShiftID(shift.ShiftID, shift.BranchShiftID);
                        }
                    }
                }
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.loadBranchDeptDtls = function (branchID) {
        settingsRepository.loadBranchDeptDtls(branchID).then(function (response) {
            if (response.data) {
                $scope.editBranchDeptList = response.data;
                debugger 
                if ($scope.departmentList != null) {
                    for (var i = 0; i < $scope.departmentList.length; i++) {
                        var dept = $filter('filter')($scope.editBranchDeptList, function (d) { return d.DepartmentID === $scope.departmentList[i].DepartmentId; })[0];
                        if (dept != null && dept.DepartmentID > 0) {
                            $scope.departmentList[i].selected = true;
                            $scope.changeDepartmentID(dept.DepartmentID, dept.BranchDeptID);
                        }
                    }
                }
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.clearData = function () {
        debugger 
        $scope.companyBranch = {};
        $scope.Reload();
        $cookieStore.put('companyBranch', $scope.companyBranch);
    }

});
