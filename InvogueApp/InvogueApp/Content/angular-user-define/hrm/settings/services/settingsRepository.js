angular.module('app').factory('settingsRepository', function ($http, $location) {
    return {
        // Shift
        loadShift: function (id) {
            return $http.post('/Shift/LoadShift', { "id": id });
        },
        loadShiftDtls: function (id) {
            return $http.post('/Shift/LoadShiftDtls', { "id": id });
        },
        // Company
        loadCompany: function (id) {
            return $http.post('/Company/LoadCompany', { "companyID": id });
        },
        saveCompany: function (company) {
            debugger
            return $http.post('/Company/SaveCompany', company);
        },
        saveShift: function (shift, dayList) {
            debugger
            return $http.post('/Shift/SaveShift', { 'shift': shift, 'weekDayList': dayList });
        },
        loadShiftWithTime: function (shiftID) {
            return $http.post('/Shift/LoadShiftDtls', { "id": shiftID });
        },
        loadBranch: function (id) {
            return $http.post('/Branch/LoadBranch', { "branchID": id, "branchCode": 'STORE' });
        },
        // Company Branch
        saveCompanyBranch: function (branch, branchDept, editBranchDeptRowID, branchShift, editBranchShiftRowID) {
            return $http.post('/CompanyBranch/SaveCompanyBranch', { 'companyBranch': branch, 'branchDeptList': branchDept, 'editDeleteBranchDeptRowID': editBranchDeptRowID, 'branchShiftList': branchShift, 'editDeleteBranchShiftRowID': editBranchShiftRowID });
        },
        loadCompanyBranch: function (id) {
            return $http.post('/CompanyBranch/LoadCompanyBranch', { "id": id });
        },
        loadBranchDeptDtls: function (id) {
            return $http.post('/CompanyBranch/LoadBranchDeptDtls', { "branchID": id });
        },
        loadBranchShiftDtls: function (id) {
            return $http.post('/CompanyBranch/LoadBranchShiftDtls', { "branchID": id });
        },
    }
});