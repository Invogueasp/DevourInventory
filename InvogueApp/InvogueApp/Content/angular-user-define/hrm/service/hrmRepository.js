angular.module('app').factory('hrmRepository', function ($http, $location) {
    return {
        // Department
        loadDepartment: function (id) {
            return $http.post('/Department/LoadDepartment/', { "departmentID": id });
        },
        loadDeptDesignationDtls: function (id) {
            return $http.post('/Department/LoadDeptDesignationDtls/', { "departmentID": id });
        },

        saveDepartment: function (data, deptDesignation, editDesignationRowID) {
            return $http.post('/Department/SaveDepartment', { "department": data, "deptDesignation": deptDesignation, "editDesignationRowID": editDesignationRowID });
        },
        //Section
        loadSection: function (id) {
            debugger
            return $http.post('/Section/LoadSection/', { "sectionID": id });
        },
        saveSection: function (data) {
            return $http.post('/Section/SaveSection', data);
        },
        //Grade
        saveGrade: function (data) {
            return $http.post('/Grade/SaveGrade', data);
        },
        loadGrade: function (id) {
            return $http.post('/Grade/LoadGrade/', { Params: { "gradeID": id } });
        },
        // Position
        savePosition: function (data) {
            return $http.post('/Position/SavePosition', data);
        },
        updateDesignation: function (data) {
            debugger
            return $http.post('/Position/UpdateDesignation', { "position": data });
        },
        loadPostion: function (id) {
            return $http.post('/Position/LoadPosition/', { "positionID": id });
        },
        // Employee type
        saveEmpType: function (data) {
            return $http.post('/EmployeeType/SaveEmpType', data);
        },
        loadEmloyeeType: function (empTypeID) {
            return $http.post('/EmployeeType/LoadEmloyeeType/', { "empTypeID": empTypeID });
        },
        //Exam
        saveExam: function (data) {
            return $http.post('/Exam/SaveExam', data);
        },
        loadExam: function (id) {
            return $http.post('/Exam/LoadExam', { "examID": id });
        },
        //Attachment
        saveAttachment: function (data) {
            return $http.post('/Attachment/SaveAttachment', data);
        },
        loadAttachment: function (id) {
            return $http.post('/Attachment/LoadAttachment', { "attachmentID": id });
        },
        // Personnel
        getLoadDepartment: function (id) {
            return $http.post('/PersonnelInfo/LoadDepartment/', { "departmentID": id });
        },
        getLoadSection: function (id) {
            return $http.post('/PersonnelInfo/LoadSection/', { "sectionID": id });
        },
        getLoadEmpType: function (id) {
            return $http.post('/PersonnelInfo/LoadEmpType/', { "empTypeID": id });
        },
        getLoadGrade: function (id) {
            return $http.post('/PersonnelInfo/LoadGrade/', { "gradeID": id });
        },
        getLoadBank: function (id) {
            return $http.post('/PersonnelInfo/LoadBank/', { "bankID": id });
        },
        getLoadBankBranch: function (id) {
            return $http.post('/PersonnelInfo/LoadBankBranch/', { "bankID": id });
        },
        loadPersonnelInfo: function (id) {
            return $http.post('/PersonnelInfo/LoadpersonnelInfo/', { "PersonnelID": id });
        },
        getDepartmentWiseEmp: function (id) {
            return $http.post('/PersonnelInfo/GetDepartmentWiseEmp/', { "departmentID": id });
        },
        personnelWiseDepartmentID: function (id) {
            return $http.post('/PersonnelInfo/PersonnelWiseDepartmentID/', { "personnelID": id });
        },
        deptWiseHeadOfDepartmentData: function (id) {
            return $http.post('/PersonnelInfo/DeptWiseHeadOfDepartmentData/', { "departmentID": id });
        },
        designationWiseHeadOfEmployeeData: function (id) {
            return $http.post('/PersonnelInfo/DesignationWiseHeadOfEmployeeData/', { "designationID": id });
        },
        savePersonnelInfo: function (data) {
            return $http.post('/PersonnelInfo/SavePersonnelInfo', data);
        },
        savePersonnelBank: function (data, deleteRowDtlsID) {
            return $http.post('/PersonnelInfo/SavePersonnelBank', { "bank": data, "editDeleteRowDtlsID": deleteRowDtlsID });
        },
        savePersonalOfficial: function (official, increment) {
            return $http.post('/PersonnelInfo/SavePersonalOfficial', { "official": official, "increment": increment });
        },
        saveApprovalProcess: function (approval) {
            return $http.post('/PersonnelInfo/SaveApprovalProcess', { "approval": approval });
        },
        savePersonnelTraining: function (data, deleteRowDtlsID) {
            return $http.post('/PersonnelInfo/SavePersonnelTraining', { "training": data, "editDeleteRowDtlsID": deleteRowDtlsID });
        },
        savePersonalEducation: function (data, deleteRowDtlsID) {
            return $http.post('/PersonnelInfo/SavePersonalEducation', { "education": data, "editDeleteRowDtlsID": deleteRowDtlsID });
        },
        savePersonalExperience: function (data, deleteRowDtlsID) {
            return $http.post('/PersonnelInfo/SavePersonalExperience', { "experience": data, "editDeleteRowDtlsID": deleteRowDtlsID });
        },
        saveAttachFile: function (file, data) {
            var formData = new FormData();
            formData.append("file", file);
            formData.append("data", angular.toJson(data));

            var config = {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }
            return $http.post('/PersonnelInfo/SaveAttachFile', formData, config);
        },
        // Personnel Offficial

        searchcompanyWiseBranch: function (companyID) {
            return $http.post('/PersonnelInfo/SearchCompanyWiseBranch', { "companyID": companyID });
        },

        searchBranchWiseShift: function (branchID) {
            return $http.post('/PersonnelInfo/SearchBranchWiseShift', { "branchID": branchID });
        },

        searchbranchWiseDepartment: function (branchID) {
            return $http.post('/PersonnelInfo/SearchbranchWiseDepartment', { "branchID": branchID });
        },

        searchDepartmentWiseSection: function (departmentID) {
            return $http.post('/PersonnelInfo/SearchDepartmentWiseSection', { "departmentID": departmentID });
        },

        searchDepartmentWiseDesignation: function (departmentID) {
            return $http.post('/PersonnelInfo/SearchDepartmentWiseDesignation', { "departmentID": departmentID });
        },
       
        //Get Edit Data
        getEditOfficial: function (personnelID) {
            return $http.post('/PersonnelInfo/GetEditOfficial', { "personnelID": personnelID });
        },
        getEditApprovalProcess: function (personnelID) {
            return $http.post('/PersonnelInfo/GetEditApprovalProcess', { "personnelID": personnelID });
        },
        getEditTraining: function (personnelID) {
            return $http.post('/PersonnelInfo/GetEditTraining', { "personnelID": personnelID });
        },
        getEditEducation: function (personnelID) {
            return $http.post('/PersonnelInfo/GetEditEducation', { "personnelID": personnelID });
        },
        getEditExperience: function (personnelID) {
            return $http.post('/PersonnelInfo/GetEditExperience', { "personnelID": personnelID });
        },
        getEditBank: function (personnelID) {
            return $http.post('/PersonnelInfo/GetEditBank', { "personnelID": personnelID });
        },
        getFile: function (code) {
            return $http.post('/PersonnelInfo/GetFile', { "code": code });
        },
        //Employment History
        getLoadPersonnel: function (id) {
            return $http.post('/EmploymentHistory/GetLoadPersonnel', { "sectionID": id });
        },
        getLoadPosition: function (id) {
            return $http.post('/EmploymentHistory/GetLoadPosition', { "positionID": id });
        },
        getEmpType: function () {
            return $http.post('/EmploymentHistory/GetEmpType');
        },
        getEmpPhoto: function (code) {
            return $http.post('/PersonnelInfo/GetEmpPhoto', { "code": code });
        },
        getPersonnel: function () {
            return $http.post('/EmploymentHistory/GetPersonnel');
        },
        getEditPromotion: function (id) {
            return $http.post('/EmploymentHistory/GetEditPromotion', { "personnelID": id });
        },
        getEditTransfer: function (id) {
            return $http.post('/EmploymentHistory/GetEditTransfer', { "personnelID": id });
        },
        SearchEmployeeData: function (depID, secID, empTID, perID) {
            return $http.post('/EmploymentHistory/SearchEmployeeData', { "departmentID": depID, "sectionID": secID, "empTypeID": empTID, "personnelID": perID });
        },
        loadEmployee: function () {
            return $http.post('/EmploymentHistory/LoadEmployee');
        },
        savePromotion: function (data, deleteRowDtlsID) {
            return $http.post('/EmploymentHistory/SavePromotion', { "promotion": data, "deleteRowID": deleteRowDtlsID });
        },
        saveTransfer: function (data, deleteRowDtlsID) {
            return $http.post('/EmploymentHistory/SaveTransfer', { "transfer": data, "deleteTransferRowID": deleteRowDtlsID });
        },
        //=====================increment section====================
        saveIncrement: function (increment) {
            debugger
            return $http.post('/EmploymentHistory/SaveIncrement', { "increment": increment });
        },
        getEditIncrement: function (id) {
            return $http.post('/EmploymentHistory/SearchIncrementSalary', { "personnelID": id });
        },

    }
})