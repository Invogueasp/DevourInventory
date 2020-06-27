angular.module('app').controller("personnelController", function ($scope, $filter, $location, $cookieStore, hrmRepository, commonRepository, settingsRepository) {
    $scope.personnelInfo = {};
    $scope.personnelOfficial = {};
    $scope.increment = {};
    $scope.filesDtls = [];
    $scope.empInfo = {};
    // Start Tab
    $scope.tab = 1;
    $scope.setTab = function (tabId) {
        $scope.tab = tabId;
    };
    $scope.isSet = function (tabId) {
        return $scope.tab === tabId;
    };
    // End Tab

    //Approval Process
    $scope.approval = {};
    $scope.hasApproval = {};
    $scope.disabled = true;
    //Training
    $scope.training = {};
    $scope.trainingDtls = [];
    $scope.selectedTrainingView = [];
    $scope.deleteTrainingRowDtlsID = [];
    //Education
    $scope.education = {};
    $scope.educationDtls = [];
    $scope.selectedEducationView = [];
    $scope.deleteEducationRowDtlsID = [];
    // Experience
    $scope.experience = {};
    $scope.experienceDtls = [];
    $scope.selectedExperienceView = [];
    $scope.deleteExperienceRowDtlsID = [];
    // Bank
    $scope.personnelBank = {};
    $scope.bankDtls = [];
    $scope.selectedBankView = [];
    $scope.deleteBankRowDtlsID = {};
    //emp level
    $scope.empLevelList = [];
    $scope.BankName = '';
    $scope.BranchName = '';
    $scope.label = ''
    $scope.label = '';

    $scope.selectedImage = true;
    $scope.loadDropdowns = function () {
        $scope.loadpersonnelInfo();
        $scope.loadReligion();
        $scope.loadBGroup();
        $scope.loadGender();
        $scope.loadMStatus();
        $scope.loadAcType();
        $scope.loadAcMode();
        $scope.loadSection();
        $scope.getLoadExam();
        $scope.loadAppDepartment();
        $scope.loadDesignation();
        $scope.loadEmpType();
        $scope.loadGrade();
        $scope.loadBank();
        $scope.loadAttachmentTitle();
        $scope.loadCompany();
        $scope.empLevelList = commonRepository.getEmployeeLevel();
        $scope.getYears = commonRepository.getYear();
        debugger 
        $scope.leaveApplicableAfter = commonRepository.getAfterMonth(6);
        $scope.incrementApplicableAfter = commonRepository.getAfterMonth(12);
        $scope.personnelInfo = $cookieStore.get('editDataPersonnel');

        if ($scope.personnelInfo.PersonnelId) {
            $scope.empDiv = true;
            $scope.empInfo.PersonnelCode = $scope.personnelInfo.PersonnelCode;
            $scope.empInfo.PersonnelName = $scope.personnelInfo.PersonnelName;
            $scope.empInfo.DesignationName = $scope.personnelInfo.DesignationName;
            $scope.empInfo.GrossSalary = $scope.personnelInfo.GrossSalary;
            $scope.empInfo.Name = $scope.personnelInfo.Name;
            $scope.empInfo.DepartmentHead = $scope.personnelInfo.DepartmentHead;
        }
        if (!$scope.personnelInfo.PersonnelId) {
            $scope.getNewPersonnelCode();
        }
    }
    $scope.removeCookies = function () {
        $scope.personnelInfo = {};
        $cookieStore.put('editDataPersonnel', $scope.personnelInfo);
    }
    $scope.getNewPersonnelCode = function () {
        var tableName = "Hr_PersonnelBasics";
        var fieldName = "PersonnelCode";
        var prefix = "PER";

        var personnelCode = commonRepository.generateCode(tableName, fieldName, prefix).then(function (response) {
            if (response.data) {
                $scope.personnelInfo.PersonnelCode = response.data.message;
            }
        })
    }
    $scope.getEditOfficial = function () {
        debugger
        $scope.getOfficialList = {};
        if ($scope.personnelInfo.PersonnelId != null) {
            hrmRepository.getEditOfficial($scope.personnelInfo.PersonnelId).then(function (response) {
                if (response.data) {
                    for (i = 0; i < response.data.length; i++) {
                        if (response.data[i].DateOfJoin) {
                            var dobDate = response.data[i].DateOfJoin.replace('/Date(', '').replace(')/', '');
                            response.data[i].DateOfJoin = $filter('date')(dobDate, "dd-MMM-yyyy");
                        }
                        if (response.data[i].CreatedDate) {
                            var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                        }
                    }
                    debugger
                    $scope.getOfficialList = response.data;
                    $scope.personnelOfficial.PersonnelOfficeID = $scope.getOfficialList[0].PersonnelOfficeID;
                    $scope.personnelOfficial.CompanyID = $scope.getOfficialList[0].CompanyID;
                    if ($scope.personnelOfficial.CompanyID) {
                        $scope.companyWiseBranch();
                    }
                    $scope.personnelOfficial.BranchID = $scope.getOfficialList[0].BranchID;
                    if ($scope.personnelOfficial.BranchID) {
                        $scope.branchWiseDepartment();
                    }

                    $scope.personnelOfficial.ShiftID = $scope.getOfficialList[0].ShiftID;
                    if ($scope.personnelOfficial.ShiftID) {
                        $scope.branchWiseShift();
                    }
                    $scope.personnelOfficial.DepartmentID = $scope.getOfficialList[0].DepartmentID;
                    if ($scope.personnelOfficial.DepartmentID) {
                        $scope.departmentWiseDesignation($scope.personnelOfficial.DepartmentID);
                        $scope.personnelOfficial.DesignationID = $scope.getOfficialList[0].DesignationID;
                    }

                    $scope.personnelOfficial.LastIncrementID = $scope.getOfficialList[0].LastIncrementID;
                    debugger
                    $scope.personnelOfficial.SectionID = $scope.getOfficialList[0].SectionID;
                    $scope.personnelOfficial.EmpTypeID = $scope.getOfficialList[0].EmpTypeID;
                    $scope.personnelOfficial.EmpLevelID = $scope.getOfficialList[0].EmpLevelID;
                    $scope.personnelOfficial.DateOfJoin = $scope.getOfficialList[0].DateOfJoin;
                    $scope.personnelOfficial.GradeID = $scope.getOfficialList[0].GradeID;
                    $scope.personnelOfficial.GrossSalary = $scope.getOfficialList[0].GrossSalary;
                    $scope.personnelOfficial.PersonnelID = $scope.getOfficialList[0].PersonnelID;
                    $scope.personnelOfficial.PersonnelStatus = $scope.getOfficialList[0].PersonnelStatus;
                    $scope.personnelOfficial.Reference = $scope.getOfficialList[0].Reference;
                    $scope.personnelOfficial.CreatedBy = $scope.getOfficialList[0].CreatedBy;
                    $scope.personnelOfficial.CreatedDate = $scope.getOfficialList[0].CreatedDate;




                   
                    var branch = $filter('filter')($scope.empTypeList, function (d) { return d.EmpTypeID == $scope.personnelOfficial.EmpTypeID })[0];
                    $scope.EmpType = branch.Name;


                    var branch = $filter('filter')($scope.empLevelList, function (d) { return d.EmpLevelID == $scope.personnelOfficial.EmpLevelID })[0];
                    $scope.EmpLevel = branch.label;


                    var branch = $filter('filter')($scope.acModeList, function (d) { return d.id == $scope.personnelOfficial.PersonnelStatus })[0];
                    $scope.acMode = branch.label;


                    var branch = $filter('filter')($scope.companyList, function (d) { return d.CompanyID == $scope.personnelOfficial.CompanyID })[0];
                    $scope.Company = branch.Name;
                    $scope.companyWiseBranch();

                    var branch = $filter('filter')($scope.departmentList, function (d) { return d.DepartmentID == $scope.personnelOfficial.DepartmentID })[0];
                    $scope.deptHead = branch.DepartmentHead;
                    $scope.departmentWiseDesignation(personnelOfficial.DepartmentID);

                    var branch = $filter('filter')($scope.branchList, function (d) { return d.BranchID == $scope.personnelOfficial.BranchID })[0];
                    $scope.Branch = branch.Name;
                    $scope.branchWiseDepartment();

                    var branch = $filter('filter')($scope.sectionList, function (d) { return d.SectionID == $scope.personnelOfficial.SectionID })[0];
                    $scope.Section = branch.SectionHead;



                }
            })

        } else {
            toastr.warning("Personnel ID Not Null !!!");
        }

    }
    $scope.getEditApprovalProcess = function () {
        //$scope.approvalProcessData = {};
        if ($scope.personnelInfo.PersonnelId != null) {
            hrmRepository.getEditApprovalProcess($scope.personnelInfo.PersonnelId).then(function (response) {
                if (response.data) {
                    debugger
                    if (response.data.EffectiveDate) {
                        var dobDate = response.data.EffectiveDate.replace('/Date(', '').replace(')/', '');
                        response.data.EffectiveDate = $filter('date')(dobDate, "dd-MMM-yyyy");
                    }

                    $scope.approval = response.data;
                    $scope.approval.EffectiveDate2 = $scope.approval.EffectiveDate;
                    $scope.approval.PersonnelName = $scope.personnelInfo.PersonnelName;
                    $scope.approval.PersonnelId = $scope.personnelInfo.PersonnelId;
                    $scope.hasApproval.FirstAppDepartmentID = $scope.approval.FirstAppDepartmentID;
                    $scope.hasApproval.FirstAppPersonnelId = $scope.approval.FirstAppPersonnelId;
                    $scope.hasApproval.ThirdAppPersonnelId = $scope.approval.ThirdAppPersonnelId;

                    $scope.hasApproval.ThirdAppDesignationID = $scope.approval.ThirdAppDesignationID;
                    if ($scope.personnelInfo.PersonnelId) {
                        $scope.personnelWiseDepartmentID($scope.personnelInfo.PersonnelId);

                    }
                    if ($scope.approval.HasFirstApproval) {
                        $scope.hasFirstApprovalContent($scope.approval.HasFirstApproval);
                    }
                    if ($scope.approval.HasSecondApproval) {
                        $scope.hasSecondApprovalContent($scope.approval.HasSecondApproval);

                    }
                    if ($scope.approval.HasThirdApproval) {
                        $scope.has3rdApprovalContent($scope.approval.HasThirdApproval);

                    }
                    if ($scope.approval.FirstAppDepartmentID) {
                        $scope.getDepartmentWiseEmp();
                    }

                } else {
                    $scope.approval.PersonnelName = $scope.personnelInfo.PersonnelName;
                    $scope.approval.PersonnelId = $scope.personnelInfo.PersonnelId;
                }
            });

        } else {
            toastr.warning("Personnel ID Not Null !!!");
        }

    }
    $scope.getEditTraining = function () {
        $scope.selectedTrainingView = [];
        $scope.trainingDtls = [];
        if ($scope.personnelInfo.PersonnelId != null) {
            hrmRepository.getEditTraining($scope.personnelInfo.PersonnelId).then(function (response) {
                if (response.data) {
                    for (i = 0; i < response.data.length; i++) {
                        if (response.data[i].StartDate) {
                            var dobDate = response.data[i].StartDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].StartDate = $filter('date')(dobDate, "dd-MMM-yyyy");
                        }
                        if (response.data[i].EndDate) {
                            var dobDate = response.data[i].EndDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].EndDate = $filter('date')(dobDate, "dd-MMM-yyyy");
                        }
                        if (response.data[i].CreatedDate) {
                            var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                        }
                    }
                    $scope.selectedTrainingView = response.data;
                    for (i = 0; i < $scope.selectedTrainingView.length; i++) {
                        $scope.trainingDtls.push({
                            TrainingID: $scope.selectedTrainingView[i].TrainingID,
                            PersonnelId: $scope.selectedTrainingView[i].PersonnelId,
                            TrainingHead: $scope.selectedTrainingView[i].TrainingHead,
                            Organizer: $scope.selectedTrainingView[i].Organizer,
                            StartDate: $scope.selectedTrainingView[i].StartDate,
                            EndDate: $scope.selectedTrainingView[i].EndDate,
                            Duration: $scope.selectedTrainingView[i].Duration,
                            CreatedBy: $scope.selectedTrainingView[i].CreatedBy,
                            CreatedDate: $scope.selectedTrainingView[i].CreatedDate
                        });
                    }

                }
            })
        } else {
            toastr.warning("Personnel ID Not Null !!!");
        }
    }

    $scope.getEditEducation = function () {
        $scope.selectedEducationView = [];
        $scope.educationDtls = [];
        if ($scope.personnelInfo.PersonnelId != null) {
            hrmRepository.getEditEducation($scope.personnelInfo.PersonnelId).then(function (response) {
                if (response.data) {
                    for (i = 0; i < response.data.length; i++) {
                        if (response.data[i].CreatedDate) {
                            var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                        }
                    }
                    $scope.selectedEducationView = response.data;
                    for (i = 0; i < $scope.selectedEducationView.length; i++) {
                        $scope.educationDtls.push({
                            PersonnelEducationID: $scope.selectedEducationView[i].PersonnelEducationID,
                            PersonnelID: $scope.selectedEducationView[i].PersonnelID,
                            ExamID: $scope.selectedEducationView[i].ExamID,
                            Institute: $scope.selectedEducationView[i].Institute,
                            BoardUniversity: $scope.selectedEducationView[i].BoardUniversity,
                            PassingYear: $scope.selectedEducationView[i].PassingYear,
                            DivisionGrade: $scope.selectedEducationView[i].DivisionGrade,
                            SubjectName: $scope.selectedEducationView[i].SubjectName,
                            GroupName: $scope.selectedEducationView[i].GroupName,
                            CreatedBy: $scope.selectedEducationView[i].CreatedBy,
                            CreatedDate: $scope.selectedEducationView[i].CreatedDate
                        });
                    }

                }
                debugger
                $scope.getYears = commonRepository.getYears();
            })
        } else {
            toastr.warning("Personnel ID Not Null !!!");
        }
    }

    $scope.getEditExperience = function () {
        $scope.selectedExperienceView = [];
        $scope.experienceDtls = [];
        if ($scope.personnelInfo.PersonnelId != null) {
            hrmRepository.getEditExperience($scope.personnelInfo.PersonnelId).then(function (response) {
                if (response.data) {
                    for (i = 0; i < response.data.length; i++) {
                        if (response.data[i].FromDate) {
                            var dobDate = response.data[i].FromDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].FromDate = $filter('date')(dobDate, "dd-MMM-yyyy");
                        }
                        if (response.data[i].ToDate) {
                            var dobDate = response.data[i].ToDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].ToDate = $filter('date')(dobDate, "dd-MMM-yyyy");
                        }
                        if (response.data[i].CreatedDate) {
                            var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                        }
                    }
                    $scope.selectedExperienceView = response.data;
                    for (i = 0; i < $scope.selectedExperienceView.length; i++) {
                        $scope.experienceDtls.push({
                            PersonnelExperienceID: $scope.selectedExperienceView[i].PersonnelExperienceID,
                            PersonnelID: $scope.selectedExperienceView[i].PersonnelID,
                            Organization: $scope.selectedExperienceView[i].Organization,
                            Position: $scope.selectedExperienceView[i].Position,
                            FromDate: $scope.selectedExperienceView[i].FromDate,
                            ToDate: $scope.selectedExperienceView[i].ToDate,
                            TotalMonth: $scope.selectedExperienceView[i].TotalMonth,
                            JobResponsibility: $scope.selectedExperienceView[i].JobResponsibility,
                            CreatedBy: $scope.selectedExperienceView[i].CreatedBy,
                            CreatedDate: $scope.selectedExperienceView[i].CreatedDate
                        });
                    }

                }
            })
        } else {
            toastr.warning("Personnel ID Not Null !!!");
        }
    }

    $scope.getEditBank = function () {

        $scope.selectedBankView = [];
        $scope.bankDtls = [];
        if ($scope.personnelInfo.PersonnelId != null) {
            hrmRepository.getEditBank($scope.personnelInfo.PersonnelId).then(function (response) {
                if (response.data) {
                    for (i = 0; i < response.data.length; i++) {
                        if (response.data[i].CreatedDate) {
                            var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                            response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                        }
                    }
                    debugger
                    $scope.selectedBankView = response.data;
                    for (i = 0; i < $scope.selectedBankView.length; i++) {
                        $scope.personnelBank.BankID = $scope.selectedBankView[i].BankID;
                        $scope.personnelBank.BankBranchID = $scope.selectedBankView[i].BankBranchID;
                        $scope.personnelBank.AccountTypeID = $scope.selectedBankView[i].AccountTypeID;
                        $scope.personnelBank.AccountModeID = $scope.selectedBankView[i].AccountModeID;
                        $scope.personnelBank.AccountNO = $scope.selectedBankView[i].AccountNO;


                    }
                    $scope.loadBankBranch($scope.personnelBank.BankID);
                }
            });
        } else {
            toastr.warning("Personnel ID Not Null !!!");
        }
    }

    $scope.calculateDate = function () {
        var fromeDate = new Date($scope.experience.FromDate);
        var toDate = new Date($scope.experience.ToDate);
        if (fromeDate != null && toDate != null) {
            var diff = (toDate.getTime() - fromeDate.getTime());
            // get months
            //var months = (diff / 1000 / 60 / 60 / 24);
            var months = (diff / 1000 / 60 / 60 / 24 / 30);
            //var months = (diff / (1000 * 60 * 60 * 24*30));
            //var days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));
            var years = (diff / 1000 / 60 / 60 / 24 / 30 / 12);
            $scope.experience.TotalMonth = months.toFixed(2);
            $scope.totalYears = years.toFixed(2);
        }

    }


    $scope.clearPersonnelData = function () {
        $scope.getNewPersonnelCode();
        $scope.personnelInfo = {};
        angular.element("input[type='file']").val(null);
        $cookieStore.put('editDataPersonnel', $scope.personnelInfo);

    }
    $scope.clearPersonnelBankData = function () {
        $scope.personnelBank = {};
        $scope.bankDtls = [];
        $scope.selectedBankView = [];
    }
    $scope.clearApprovalData = function () {
        $scope.approval = {};
        $scope.approval.FirstAppDepartmentID = null;
        $scope.firstEmployeeList = [];
    }
    $scope.clearPersonnelOfficialData = function () {
        $scope.personnelOfficial = {};
    }
    $scope.clearTrainingData = function () {
        angular.element("input[type='file']").val(null);
        $scope.training = {};
        $scope.trainingDtls = [];
        $scope.selectedTrainingView = [];
    }
    $scope.clearEducationData = function () {
        angular.element("input[type='file']").val(null);
        $scope.education = {};
        $scope.educationDtls = [];
        $scope.selectedEducationView = [];

    }
    $scope.clearExperienceData = function () {
        angular.element("input[type='file']").val(null);
        $scope.experience = {};
        $scope.experienceDtls = [];
        $scope.selectedExperienceView = [];
    }
    // Personnel Info
    $scope.loadReligion = function () {
        $scope.religionList = commonRepository.getReligion();
    }
    $scope.loadMStatus = function () {
        $scope.mStatusList = commonRepository.getMStatus();
    }
    $scope.loadBGroup = function () {
        $scope.bGroupList = commonRepository.getBGroup();
    }
    $scope.loadGender = function () {
        $scope.genderList = commonRepository.getGender();
    }
    $scope.getSpouse = function () {
        if ($scope.personnelInfo.MaritalStatus == 1) {
            $scope.spouseShow = true;
        } else {
            $scope.spouseShow = false;
        }
    }
    $scope.getLoadExam = function () {
        hrmRepository.loadExam().then(function (response) {
            if (response.data) {
                $scope.examList = response.data;
            }
        });
    }

    $scope.getExamName = function () {
        var title = $filter('filter')($scope.examList, function (d) { return d.ExamID == $scope.education.ExamID })[0];
        $scope.ExamName = title.ExamName;
    }
    // Personnel Bank
    $scope.loadAcType = function () {
        $scope.acTypeList = commonRepository.getAType();
    }
    $scope.loadAcMode = function () {
        $scope.acModeList = commonRepository.getAStatus();
        $scope.personnelOfficial.PersonnelStatus = $scope.acModeList[0].id;
    }
    //Personnel Official
    $scope.loadAppDepartment = function () {
        hrmRepository.getLoadDepartment().then(function (response) {
            if (response.data) {
                $scope.appDeptList = response.data;
            }
        });
    }
    $scope.loadSection = function () {
        hrmRepository.getLoadSection().then(function (response) {
            if (response.data) {
                $scope.sectionList = response.data;
            }
        });

    }
    $scope.loadEmpType = function () {
        hrmRepository.getLoadEmpType().then(function (response) {
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].Status) {
                        response.data[i].VStatus = commonRepository.convertAStatusText(response.data[i].Status);
                    }
                }
                $scope.empTypeList = response.data
            }
        })
    }
    $scope.loadGrade = function () {
        hrmRepository.getLoadGrade().then(function (response) {
            if (response.data) {
                $scope.gradeList = response.data;
            }
        });
    }
    // Approval Process
    $scope.getDepartmentWiseEmp = function () {
        hrmRepository.getDepartmentWiseEmp($scope.approval.FirstAppDepartmentID).then(function (response) {
            if (response.data) {
                $scope.firstEmployeeList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.loadDesignation = function () {
        hrmRepository.getLoadPosition().then(function (response) {
            if (response.data) {
                $scope.designationList = response.data;
            }
        });
    }
    $scope.personnelWiseDepartmentID = function (personnelID) {
        hrmRepository.personnelWiseDepartmentID(personnelID).then(function (response) {
            if (response.data) {
                $scope.personnelDepartment = response.data;
                $scope.approval.SecondAppDepartmentID = $scope.personnelDepartment.DepartmentID;
                if ($scope.approval.SecondAppDepartmentID) {
                    $scope.deptWiseHeadOfDepartmentData($scope.approval.SecondAppDepartmentID);
                }

            }
        });
    }
    $scope.deptWiseHeadOfDepartmentData = function (departmentID) {
        hrmRepository.deptWiseHeadOfDepartmentData(departmentID).then(function (response) {
            if (response.data) {
                debugger
                $scope.headOfDepartment = response.data;
                if ($scope.headOfDepartment != null) {
                    $scope.approval.SecondAppDesignationID = $scope.headOfDepartment.DesignationID;
                    $scope.approval.SecondAppPersonnelId = $scope.headOfDepartment.PersonnelID;
                }
            }
        });
    }
    //$scope.designationWiseHeadOfEmployeeData = function (designationID) {
    //    hrmRepository.designationWiseHeadOfEmployeeData(designationID).then(function (response) {
    //        if (response.data) {
    //            debugger
    //            $scope.headOfEmp = response.data;
    //            if ($scope.headOfEmp != null) {
    //                $scope.approval.SecondAppPersonnelId = $scope.headOfEmp.PersonnelID;
    //            }
    //        }
    //    });
    //}
    $scope.has3rdApprovalContent = function (sec) {
        debugger
        if (sec == "1") {
            $scope.hide3rdApproval = true;
            // $scope.personnelWiseDepartmentID($scope.personnelInfo.PersonnelId);
            $scope.approval.ThirdAppDesignationID = $scope.hasApproval.ThirdAppDesignationID;
            $scope.approval.ThirdAppPersonnelId = $scope.hasApproval.ThirdAppPersonnelId;
            $scope.approval.EffectiveDate = $scope.approval.EffectiveDate2;


        } else {
            $scope.hide3rdApproval = false;
            // $scope.approval.SecondAppDepartmentID = "";
            $scope.approval.ThirdAppDesignationID = "";
            $scope.approval.ThirdAppPersonnelId = "";
        }
    }
    $scope.hasSecondApprovalContent = function (sec) {
        debugger
        if (sec == "1") {
            $scope.hide2ndApproval = true;
            $scope.personnelWiseDepartmentID($scope.personnelInfo.PersonnelId);
        } else {
            $scope.hide2ndApproval = false;
            $scope.approval.SecondAppDepartmentID = "";
            $scope.approval.SecondAppDesignationID = "";
            $scope.approval.SecondAppPersonnelId = "";
        }
    }
    $scope.hasFirstApprovalContent = function (first) {
        debugger
        if (first == "1") {
            $scope.hide1stApproval = true;
            $scope.approval.FirstAppDepartmentID = $scope.hasApproval.FirstAppDepartmentID;
            $scope.approval.FirstAppPersonnelId = $scope.hasApproval.FirstAppPersonnelId;
        } else {
            $scope.hide1stApproval = false;
            $scope.loadAppDepartment();
            $scope.approval.FirstAppDepartmentID = "";
            $scope.approval.FirstAppPersonnelId = "";

        }
    }
    $scope.saveApprovalProcess = function () {
        debugger


        hrmRepository.saveApprovalProcess($scope.approval).then(function (response) {
            if (response.data.isSucess) {
                debugger
                toastr.success(response.data.message);
               // $scope.clearApprovalData();
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('#!/Login')
                }
                toastr.error(response.data.message);
            }
        });
    }
    //Personnel Bank
    $scope.loadBank = function () {
        hrmRepository.getLoadBank().then(function (response) {
            if (response.data) {
                $scope.bankList = response.data;
            }
        })
    }
    $scope.loadBankBranch = function () {
        hrmRepository.getLoadBankBranch($scope.personnelBank.BankID).then(function (response) {
            if (response.data) {
                $scope.bankBranchList = response.data;
            }
           
        });
    }

    // Attachment 
    $scope.attach = {};
    $scope.fileList = [];
    $scope.loadAttachmentTitle = function () {
        hrmRepository.loadAttachment().then(function (response) {
            if (response.data) {
                $scope.attachList = response.data
            }
        });
    }
    $scope.getTitle = function () {
        var title = $filter('filter')($scope.attachList, function (d) { return d.AttachmentTitleID == $scope.attach.AttachmentTitleID })[0];
        $scope.attach.attachID = title.AttachmentTitleID;
        $scope.attach.Title = title.Title;
    }
    $scope.saveAttachFile = function () {
        if ($scope.personnelInfo.PersonnelId == null) {
            toastr.error("Personnel ID Not Null !!!");
            return;
        }
        file = [];
        if ($scope.checkValidAttach() == true) {
            $scope.attach.PersonnelCode = $scope.personnelInfo.PersonnelCode;
            $scope.attach.PersonnelID = $scope.personnelInfo.PersonnelId;
            hrmRepository.saveAttachFile($scope.selectFileUpload, $scope.attach).then(function (response) {
                if (response.data.isSucess) {
                    
                    toastr.success(response.data.message);
                    $scope.clearAttachData();
                    $scope.getEditAttachFile();
                    $location.path('/PersonnelInfoCreate');
                } else {
                    if (response.data.message == "LogOut") {
                        $location.path('/Login');
                    }
                    toastr.error(response.data.message);
                }
            })
        } else {
            toastr.error("Please fill-up all required field !!!");
        }
    }
    $scope.getEditAttachFile = function () {
        
        $scope.attachIDList = [];
        hrmRepository.getFile($scope.personnelInfo.PersonnelId).then(function (response) {
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    response.data[i].FullName = $scope.personnelInfo.PersonnelId;
                }
                $scope.fileList = response.data;
            }

        });
        hrmRepository.getEmpPhoto($scope.personnelInfo.PersonnelId).then(function (response) {
            
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    response.data[i].FullName = $scope.personnelInfo.PersonnelId;
                }
                debugger
                $scope.photoList = response.data;

            }

        });

    }
    $scope.clearAttachData = function () {
        $scope.attach = {};
        angular.element("input[type='file']").val(null);

    }
    $scope.checkValidAttach = function () {
        debugger
        $scope.isValid = true;
        if (!$scope.attach.AttachmentTitleID != "") {
            $scope.isValid = false;
            return $scope.isValid;
        }
        else if (!$scope.selectFileUpload != "") {
            $scope.isValid = false;
            return $scope.isValid;
        } else {
            return $scope.isValid;
        }
    }

    // Single File Select event
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
    $scope.savePersonnelInfo = function () {
        debugger
        if ($scope.personnelForm.$valid) {
            hrmRepository.savePersonnelInfo($scope.personnelInfo).then(function (response) {
                if (response.data.isSucess) {
                    debugger
                    toastr.success(response.data.message);
                    //$scope.clearPersonnelData();
                    $scope.personnelInfo.PersonnelId = response.data.lastInsertedID;
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
    // Save Personnel Official
    $scope.savePersonalOfficial = function () {
        debugger
        if ($scope.personnelInfo.PersonnelId == null) {
            toastr.error("Personnel ID Not Null !!!");
            return;
        }
        if ($scope.pOfficialForm.$valid) {
            if ($scope.personnelOfficial.PersonnelOfficeID == null) {
                $scope.personnelOfficial.PersonnelId = $scope.personnelInfo.PersonnelId;
            }
            if ($scope.personnelOfficial.PersonnelOfficeID > 0) {
                $scope.increment.IncrementID = $scope.personnelOfficial.LastIncrementID;
            }
            $scope.personnelOfficial.GradeID = 5;
            $scope.increment.IncrementDate = $scope.personnelOfficial.DateOfJoin;
            $scope.increment.EffectiveDate = $scope.personnelOfficial.DateOfJoin;
            $scope.increment.GradeID = $scope.personnelOfficial.GradeID;
            $scope.increment.UpdatedSalary = $scope.personnelOfficial.GrossSalary;
            $scope.increment.PreviousSalary = $scope.personnelOfficial.GrossSalary;
       
            hrmRepository.savePersonalOfficial($scope.personnelOfficial, $scope.increment).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    //$scope.clearPersonnelOfficialData();
             
                    //$scope.personnelInfo.PersonnelId = '';

                } else {
                    if (response.data.message == "LogOut") {
                        $location.path('/Login');
                    }
                    toastr.error(response.data.message);
                }
            })
        } else {
            toastr.error("Please fill-up all required field !!!");
        }
    }

    //official department branch company
    $scope.loadCompany = function () {
        $scope.companyList = [];
        settingsRepository.loadCompany().then(function (response) {
            if (response.data) {
                $scope.companyList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }
    $scope.companyWiseBranch = function () {
        var branchesList = hrmRepository.searchcompanyWiseBranch($scope.personnelOfficial.CompanyID);
        branchesList.then(function (response) {

            $scope.branchList = response.data;
            if ($scope.personnelOfficial.BranchID) {
                var branch = $filter('filter')($scope.branchList, function (d) { return d.BranchID == $scope.personnelOfficial.BranchID })[0];
                $scope.Branch = branch.Name;
            }
           

        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }

    $scope.branchWiseShift = function () {
        debugger
        var shiftsList = hrmRepository.searchBranchWiseShift($scope.personnelOfficial.BranchID);
        shiftsList.then(function (response) {

            $scope.shiftList = response.data;

            //if ($scope.personnelOfficial.BranchShiftID) {
            //    var shift = $filter('filter')($scope.shiftList, function (d) { return d.BranchShiftID == $scope.personnelOfficial.BranchShiftID })[0];
            //    $scope.shiftName = shift.Name;
            //}

        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }

    $scope.branchWiseDepartment = function () {
        var departmentsList = hrmRepository.searchbranchWiseDepartment($scope.personnelOfficial.BranchID);
        departmentsList.then(function (response) {

            $scope.departmentList = response.data;

            if ($scope.personnelOfficial.DepartmentID) {
                var branch = $filter('filter')($scope.departmentList, function (d) { return d.DepartmentID == $scope.personnelOfficial.DepartmentID })[0];
                $scope.deptHead = branch.DepartmentHead;
            }

        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }

    $scope.departmentWiseSection = function () {
        var sectionsList = hrmRepository.searchDepartmentWiseSection($scope.personnelOfficial.DepartmentID);
        sectionsList.then(function (response) {
            debugger
            $scope.sectionList = response.data;

            //if ($scope.personnelOfficial.DepartmentID) {
            //    var sec = $filter('filter')($scope.departmentList, function (d) { return d.DepartmentID == $scope.personnelOfficial.DepartmentID })[0];
            //    $scope.secHead = sec.SectionHead;
            //}

        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }

    $scope.departmentWiseDesignation = function (departmentID) {
        debugger
        var departmentsList = hrmRepository.searchDepartmentWiseDesignation(departmentID);
        departmentsList.then(function (response) {
            debugger
            $scope.positionList = response.data;
        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });


    }
    //training date calculate
    $scope.trainingCalculateDate = function () {
        debugger
        var fromeDate = new Date($scope.training.StartDate);
        var toDate = new Date($scope.training.EndDate);
        if (fromeDate != null && toDate != null) {
            var diff = (toDate.getTime() - fromeDate.getTime());
            // get months
            //var months = (diff / 1000 / 60 / 60 / 24);
            var months = (diff / 1000 / 60 / 60 / 24 / 30);
            //var months = (diff / (1000 * 60 * 60 * 24*30));
            //var days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));
            var years = (diff / 1000 / 60 / 60 / 24 / 30 / 12);
            $scope.training.Duration = months.toFixed(2);
            //$scope.training.Duration = years.toFixed(2);
        }

    }
    // Personnel Training Add Data 
    $scope.savePersonnelTraining = function () {
        debugger
        if ($scope.personnelInfo.PersonnelId == null) {
            toastr.error("Personnel ID Not Null !!!");
            return;
        }
       
            hrmRepository.savePersonnelTraining($scope.trainingDtls, $scope.deleteTrainingRowDtlsID).then(function (response) {
                if (response.data.isSucess) {
                    toastr.success(response.data.message);
                    //$scope.clearTrainingData();
                } else {
                    if (response.data.message == "LogOut") {
                        $location.path('/Login');
                    }
                    toastr.error(response.data.message);
                }
            });
       
    }

    $scope.addTraining = function () {
        if ($scope.trainingForm.$valid) {
            debugger
            if (parseInt($scope.trainingDtls.length) > 0) {
                for (i = 0; i < $scope.trainingDtls.length; i++) {
                    if ($scope.trainingDtls[i].TrainingHead == $scope.training.TrainingHead) {
                        toastr.error('Sorry! This Training Name Already Exist!!!');
                        $scope.clearTraining();
                        return;
                    }
                }
            }

            $scope.selectedTrainingView.push({
                TrainingHead: $scope.training.TrainingHead,
                Organizer: $scope.training.Organizer,
                StartDate: $scope.training.StartDate,
                EndDate: $scope.training.EndDate,
                Duration: $scope.training.Duration
            })
            $scope.trainingDtls.push({
                TrainingID: 0,
                PersonnelId: $scope.personnelInfo.PersonnelId,
                TrainingHead: $scope.training.TrainingHead,
                Organizer: $scope.training.Organizer,
                StartDate: $scope.training.StartDate,
                EndDate: $scope.training.EndDate,
                Duration: $scope.training.Duration
            });
            $scope.clearTraining();
        } else {
            toastr.error("Please fill-up all required field !!!");
        }
        
    }
    ////////////////////Training Update & Add button///////////////////////////
    $scope.updateShow = true;
    $scope.addHide = false;

    $scope.editTrainingRow = function (index, row) {
        debugger
        //$scope.personnelInfo.PersonnelId = row.PersonnelId;
        $scope.training.TrainingHead = row.TrainingHead;
        $scope.training.Organizer = row.Organizer;
        $scope.training.StartDate = row.StartDate;
        $scope.training.EndDate = row.EndDate;
        $scope.training.Duration = row.Duration;
        $scope.training.TrainingID = row.TrainingID;

        $scope.addHide = true;
        $scope.updateShow = false;
    }

    $scope.updateTraining = function (row) {
        debugger
        for (var i = 0; i < $scope.selectedTrainingView.length; i++) {
            if (row.TrainingID == $scope.selectedTrainingView[i].TrainingID) {
                $scope.selectedTrainingView[i].TrainingHead = row.TrainingHead;
                $scope.selectedTrainingView[i].Organizer = row.Organizer;
                $scope.selectedTrainingView[i].StartDate = row.StartDate;
                $scope.selectedTrainingView[i].EndDate = row.EndDate;
                $scope.selectedTrainingView[i].Duration = row.Duration;
            }
            if (row.TrainingID == $scope.trainingDtls[i].TrainingID) {
                TrainingID: 0,
                //$scope.personnelInfo.PersonnelId = row.PersonnelId;
                $scope.trainingDtls[i].TrainingHead = row.TrainingHead;
                $scope.trainingDtls[i].Organizer = row.Organizer;
                $scope.trainingDtls[i].EndDate = row.EndDate;
                $scope.trainingDtls[i].StartDate = row.StartDate;
                $scope.trainingDtls[i].Duration = row.Duration;
            }
        }
        $scope.clearTraining();
        $scope.updateShow = true;
        $scope.addHide = false;
    }
    ///////////////////////////////////////////////////////////////////

    $scope.deleteTrainingRow = function (index, row) {
        $scope.selectedTrainingView.splice(index, 1);
        $scope.trainingDtls.splice(index, 1);
        $scope.deleteTrainingRowDtlsID.push(row.TrainingID);
    }

    // Start :: Education Part
    $scope.savePersonalEducation = function () {
        debugger
        if ($scope.personnelInfo.PersonnelId == null) {
            toastr.error("Personnel ID Not Null !!!");
            return;
        }
        hrmRepository.savePersonalEducation($scope.educationDtls).then(function (response) {
            if (response.data.isSucess) {
                debugger
                toastr.success(response.data.message);
                //$scope.clearEducationData();
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('/Login');
                }
                toastr.error(response.data.message);
            }
        });


    }
    $scope.addEducation = function () {
        debugger

        if ($scope.educationForm.$valid) {
            if (parseInt($scope.educationDtls.length) > 0) {
                for (i = 0; i < $scope.educationDtls.length; i++) {
                    if ($scope.educationDtls[i].ExamID == $scope.education.ExamID) {
                        toastr.error('Sorry! This Exam Name Already Exist!!!');
                        $scope.clearEducation();
                        return;
                    }
                }
            }

            $scope.selectedEducationView.push({
                ExamID: $scope.education.ExamID,
                ExamName: $scope.ExamName,
                Institute: $scope.education.Institute,
                BoardUniversity: $scope.education.BoardUniversity,
                PassingYear: $scope.education.PassingYear,
                DivisionGrade: $scope.education.DivisionGrade,
                SubjectName: $scope.education.SubjectName,
                GroupName: $scope.education.GroupName
            })
            $scope.educationDtls.push({
                PersonnelEducationID: 0,
                PersonnelID: $scope.personnelInfo.PersonnelId,
                ExamID: $scope.education.ExamID,
                Institute: $scope.education.Institute,
                BoardUniversity: $scope.education.BoardUniversity,
                PassingYear: $scope.education.PassingYear,
                DivisionGrade: $scope.education.DivisionGrade,
                SubjectName: $scope.education.SubjectName,
                GroupName: $scope.education.GroupName
            });
            $scope.clearEducation();
        }
        else {
            toastr.error("Please fill-up all required field !!!");
        }
    }

    ////////////////////Education Update & Add button///////////////////////////
    $scope.updateShow = true;
    $scope.addHide = false;

    $scope.editEducationRow = function (index, row) {
        debugger
        $scope.education.ExamID = row.ExamID;
        $scope.education.PersonnelEducationID = row.PersonnelEducationID;
        $scope.education.Institute = row.Institute;
        $scope.education.BoardUniversity = row.BoardUniversity;
        $scope.education.PassingYear = row.PassingYear;
        $scope.education.DivisionGrade = row.DivisionGrade;
        $scope.education.SubjectName = row.SubjectName;
        $scope.education.GroupName = row.GroupName;

        $scope.addHide = true;
        $scope.updateShow = false;
    }

    $scope.updateEducation = function (row) {
        debugger
        for (var i = 0; i < $scope.selectedEducationView.length; i++) {
            if (row.PersonnelEducationID == $scope.selectedEducationView[i].PersonnelEducationID) {
                $scope.selectedEducationView[i].ExamID = row.ExamID;
                $scope.selectedEducationView[i].ExamName = $scope.ExamName;
                $scope.selectedEducationView[i].Institute = row.Institute;
                $scope.selectedEducationView[i].BoardUniversity = row.BoardUniversity;
                $scope.selectedEducationView[i].PassingYear = row.PassingYear;
                $scope.selectedEducationView[i].DivisionGrade = row.DivisionGrade;
                $scope.selectedEducationView[i].SubjectName = row.SubjectName;
                $scope.selectedEducationView[i].GroupName = row.GroupName;
            }
            if (row.PersonnelEducationID == $scope.educationDtls[i].PersonnelEducationID) {
                debugger
                $scope.education.PersonnelId = row.PersonnelId;
                $scope.educationDtls[i].ExamID = row.ExamID;
                $scope.educationDtls[i].Institute = row.Institute;
                $scope.educationDtls[i].BoardUniversity = row.BoardUniversity;
                $scope.educationDtls[i].PassingYear = row.PassingYear;
                $scope.educationDtls[i].DivisionGrade = row.DivisionGrade;
                $scope.educationDtls[i].SubjectName = row.SubjectName;
                $scope.educationDtls[i].GroupName = row.GroupName;
            }
        }
        $scope.clearEducation();
        $scope.updateShow = true;
        $scope.addHide = false;
    }

    $scope.deleteEducationRow = function (index, row) {
        $scope.selectedEducationView.splice(index, 1);
        $scope.educationDtls.splice(index, 1);
        $scope.deleteEducationRowDtlsID.push(row.PersonnelEducationID);
    }
    // End :: Education Part

    // Start :: Experience Part
    $scope.savePersonalExperience = function () {
        if ($scope.personnelInfo.PersonnelId == null) {
            toastr.error("Personnel ID Not Null !!!");
            return;
        }
        debugger

        hrmRepository.savePersonalExperience($scope.experienceDtls, $scope.deleteExperienceRowDtlsID).then(function (response) {
            if (response.data.isSucess) {
                toastr.success(response.data.message);
               // $scope.clearExperienceData();
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('/Login');
                }
                toastr.error(response.data.message);
            }
        });


    }
    $scope.addExperience = function () {

        if ($scope.expForm.$valid) {
            if (parseInt($scope.experienceDtls.length) > 0) {
                for (i = 0; i < $scope.experienceDtls.length; i++) {
                    if ($scope.experienceDtls[i].PersonnelExperienceID == $scope.experience.PersonnelExperienceID) {
                        toastr.error('Sorry! This Organization Already Exist!!!');
                        $scope.clearExperience();
                        return;
                    }
                }
            }

            $scope.selectedExperienceView.push({
                Organization: $scope.experience.Organization,
                Position: $scope.experience.Position,
                FromDate: $scope.experience.FromDate,
                ToDate: $scope.experience.ToDate,
                TotalMonth: $scope.experience.TotalMonth,
                OrgAddress: $scope.experience.OrgAddress,
                JobResponsibility: $scope.experience.JobResponsibility
            })
            $scope.experienceDtls.push({
                PersonnelExperienceID: 0,
                PersonnelID: $scope.personnelInfo.PersonnelId,
                Organization: $scope.experience.Organization,
                Position: $scope.experience.Position,
                FromDate: $scope.experience.FromDate,
                ToDate: $scope.experience.ToDate,
                TotalMonth: $scope.experience.TotalMonth,
                OrgAddress: $scope.experience.OrgAddress,
                JobResponsibility: $scope.experience.JobResponsibility
            });
            debugger
            $scope.clearExperience();
        }
        else {
            toastr.error("Please fill-up all required field !!!");
        }
    }
    ////////////////////Experience Update & Add button///////////////////////////
    $scope.updateShow = true;
    $scope.addHide = false;

    $scope.editExperienceRow = function (index, row) {
        debugger
        $scope.experience.Organization = row.Organization;
        $scope.experience.Position = row.Position;
        $scope.experience.FromDate = row.FromDate;
        $scope.experience.ToDate = row.ToDate;
        $scope.experience.TotalMonth = row.TotalMonth;
        $scope.experience.JobResponsibility = row.JobResponsibility;
        $scope.experience.OrgAddress = row.OrgAddress;
        $scope.experience.PersonnelExperienceID = row.PersonnelExperienceID;

        $scope.addHide = true;
        $scope.updateShow = false;
    }
    $scope.updateExperience = function (row) {
        debugger
        for (var i = 0; i < $scope.selectedExperienceView.length; i++) {
            if (row.PersonnelExperienceID == $scope.selectedExperienceView[i].PersonnelExperienceID) {
                $scope.selectedExperienceView[i].Organization = row.Organization;
                $scope.selectedExperienceView[i].Position = row.Position;
                $scope.selectedExperienceView[i].FromDate = row.FromDate;
                $scope.selectedExperienceView[i].ToDate = row.ToDate;
                $scope.selectedExperienceView[i].TotalMonth = row.TotalMonth;
                $scope.selectedExperienceView[i].OrgAddress = row.OrgAddress;
                $scope.selectedExperienceView[i].JobResponsibility = row.JobResponsibility;
            }
            if (row.PersonnelExperienceID == $scope.experienceDtls[i].PersonnelExperienceID) {
                debugger
                PersonnelExperienceID: 0,
                $scope.experienceDtls[i].Organization = row.Organization;
                $scope.experienceDtls[i].Position = row.Position;
                $scope.experienceDtls[i].FromDate = row.FromDate;
                $scope.experienceDtls[i].ToDate = row.ToDate;
                $scope.experienceDtls[i].TotalMonth = row.TotalMonth;//row.TotalMonth
                $scope.experienceDtls[i].OrgAddress = row.OrgAddress;
                $scope.experienceDtls[i].JobResponsibility = row.JobResponsibility;
            }
        }
        $scope.clearExperience();
        $scope.updateShow = true;
        $scope.addHide = false;
    }
    $scope.deleteExperienceRow = function (index, row) {
        $scope.selectedExperienceView.splice(index, 1);
        $scope.experienceDtls.splice(index, 1);
        $scope.deleteExperienceRowDtlsID.push(row.PersonnelExperienceID);
    }
    // End :: Experience Part

    // Start :: Bank Part
    $scope.getBankName = function () {
        debugger
        var bank = $filter('filter')($scope.bankList, function (d) { return d.BankId == $scope.personnelBank.BankID })[0];
        $scope.BankName = bank.BankName;
    }
    $scope.getBranchName = function () {
        debugger
        var branch = $filter('filter')($scope.bankBranchList, function (d) { return d.BankBranchID == $scope.personnelBank.BankBranchID })[0];
        $scope.BranchName = branch.BranchName;
    }
    $scope.savePersonnelBank = function (personnelBank) {
        debugger
        if ($scope.personnelInfo.PersonnelId == null) {
            toastr.error("Personnel ID Not Null !!!");
            return;
        }
        hrmRepository.savePersonnelBank(personnelBank).then(function (response) {
            if (response.data.isSucess) {
                debugger
                toastr.success(response.data.message);
                //$scope.clearPersonnelBankData();
                $scope.addHide = false;
                $scope.updateShow = true;
            } else {
                if (response.data.message == "LogOut") {
                    $location.path('/Login');
                }
                toastr.error(response.data.message);
            }
        })
    }

    $scope.addBank = function () {
        debugger
        $scope.personnelBank.PersonnelID = $scope.personnelInfo.PersonnelId;
        $scope.savePersonnelBank($scope.personnelBank);
    }


    //$scope.editBankRow = function (index, row) {
    //    debugger        
    //    $scope.personnelBank.BankID = row.BankID;
    //    $scope.personnelBank.BankBranchID = row.BankBranchID;
    //    $scope.personnelBank.AccountNO = row.AccountNO;
    //    $scope.personnelBank.AccountTypeID = row.AccountTypeID;
    //    $scope.personnelBank.AccountModeID = row.AccountModeID;
    //    $scope.personnelBank.CreatedDate = row.CreatedDate;
    //    $scope.personnelBank.PersonnelBankID = row.PersonnelBankID;
    //    $scope.personnelBank.PersonnelID = row.PersonnelID;


    //    //$scope.getBranchName();
    //    $scope.addHide = true;
    //    $scope.updateShow = false;
    //}


    $scope.deleteBankRow = function (index, row) {
        debugger
        $scope.selectedBankView.splice(index, 1);
        $scope.bankDtls.splice(index, 1);
        $scope.personnelBank = row;
        $scope.deleteBankRowDtlsID = row.PersonnelBankID
        $scope.savePersonnelBank($scope.personnelBank, $scope.deleteBankRowDtlsID);
    }
    // End :: Bank Part
    // convert the array to a base64-encoded string by Priti
    //$scope.arrayBufferToBase64 = function (buffer) {
    //    var binary = '';
    //    var bytes = new Uint8Array(buffer);
    //    var len = bytes.byteLength;
    //    for (var i = 0; i < len; i++) {
    //        binary += String.fromCharCode(bytes[i]);
    //    }
    //    return window.btoa(binary);
    //}

    // Personnel Info Loading Master Data
    $scope.loadpersonnelInfo = function () {
        hrmRepository.loadPersonnelInfo().then(function (response) {
            if (response.data) {
                for (i = 0; i < response.data.length; i++) {
                    if (response.data[i].DOB) {
                        var dobDate = response.data[i].DOB.replace('/Date(', '').replace(')/', '');
                        response.data[i].DOB = $filter('date')(dobDate, "dd-MMM-yyyy");
                    }
                    if (response.data[i].CreatedDate) {
                        var createdDate = response.data[i].CreatedDate.replace('/Date(', '').replace(')/', '');
                        response.data[i].CreatedDate = $filter('date')(createdDate, "dd-MMM-yyyy");
                    }
                    //if (response.data[i].MaritalStatus) {
                    //    response.data[i].MaritalStatus = commonRepository.convertMaritalStatusText(response.data[i].MaritalStatus);
                    //}
                }
                debugger
                $scope.personnelInfoList = response.data;
            }
        })
    }

    $scope.getEmpDataProfile = function () {

        debugger
        var branch = $filter('filter')($scope.genderList, function (d) { return d.id == $scope.personnelInfo.Gender })[0];
        $scope.gender = branch.label;

        var branch = $filter('filter')($scope.mStatusList, function (d) { return d.id == $scope.personnelInfo.MaritalStatus })[0];
        $scope.MaritalStatus = branch.label;

        var branch = $filter('filter')($scope.bGroupList, function (d) { return d.id == $scope.personnelInfo.BloodGroup })[0];
        $scope.BloodGroup = branch.label;


        var branch = $filter('filter')($scope.religionList, function (d) { return d.id == $scope.personnelInfo.Religion })[0];
        $scope.Religion = branch.label;


        var branch = $filter('filter')($scope.sectionList, function (d) { return d.SectionID == $scope.personnelInfo.SectionID })[0];
        $scope.Section = branch.SectionHead;

        var branch = $filter('filter')($scope.empTypeList, function (d) { return d.EmpTypeID == $scope.personnelOfficial.EmpTypeID })[0];
        $scope.EmpType = branch.Name;


        var branch = $filter('filter')($scope.empLevelList, function (d) { return d.EmpLevelID == $scope.personnelOfficial.EmpLevelID })[0];
        $scope.EmpLevel = branch.label;


        var branch = $filter('filter')($scope.acModeList, function (d) { return d.id == $scope.personnelOfficial.PersonnelStatus })[0];
        $scope.acMode = branch.label;


        var branch = $filter('filter')($scope.companyList, function (d) { return d.CompanyID == $scope.personnelOfficial.CompanyID })[0];
        $scope.Company = branch.Name;
        $scope.companyWiseBranch();

        var branch = $filter('filter')($scope.branchList, function (d) { return d.BranchID == $scope.personnelOfficial.BranchID })[0];
        $scope.Branch = branch.Name;
        $scope.branchWiseDepartment();


        var branch = $filter('filter')($scope.departmentList, function (d) { return d.DepartmentID == $scope.personnelOfficial.DepartmentID })[0];
        $scope.deptHead = branch.DepartmentHead;
        $scope.departmentWiseDesignation(personnelOfficial.DepartmentID);
    }

    $scope.editRow = function (row) {
        debugger
        $location.path("/PersonnelInfoCreate");
        $scope.personnelInfo = row;
        $scope.empInfo.PersonnelCode = row.PersonnelCode;
        $scope.empInfo.PersonnelName = row.PersonnelName;
        $scope.empInfo.DesignationName = row.DesignationName;
        $scope.empInfo.GrossSalary = row.GrossSalary;
        $cookieStore.put('editDataPersonnel', $scope.personnelInfo);
    }
    $scope.clearTraining = function () {
        $scope.training.TrainingHead = '';
        $scope.training.Organizer = '';
        $scope.training.TrainingDate = '';
        $scope.training.Duration = '';
    }
    $scope.clearEducation = function () {
        $scope.education.ExamName = '';
        $scope.education.Institute = '';
        $scope.education.BoardUniversity = '';
        $scope.education.PassingYear = '';
        $scope.education.DivisionGrade = '';
        $scope.education.SubjectName = '';
        $scope.education.GroupName = '';
    }
    $scope.clearExperience = function () {
        $scope.experience.Organization = '';
        $scope.experience.Position = '';
        $scope.experience.FromDate = '';
        $scope.experience.ToDate = '';
        $scope.experience.TotalMonth = '';
        $scope.experience.totalYears = '';
        $scope.experience.OrgAddress = '';
        $scope.experience.JobResponsibility = '';
    }
    $scope.clearBank = function () {
        $scope.personnelBank.BankID = null;
        $scope.personnelBank.BankName = null;
        $scope.personnelBank.BankBranchID = null;
        $scope.personnelBank.BranchName = null;
        $scope.personnelBank.AccountNO = '';
        $scope.personnelBank.AccountTypeID = null;
        $scope.personnelBank.AccountModeID = null;
    }


    $scope.checkValidTrainingDtls = function () {
        $scope.isValid = false;
        $scope.message = '';
        if ($scope.training.TrainingHead != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message = 'Please Insert Your Name !';
        }

        if ($scope.training.Organizer != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your Organizer !';
        }

        if ($scope.training.StartDate != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Select Your Date !';
        }
        if ($scope.training.Duration != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your Date !';
        }
        if ($scope.message != '') {
            toastr.error($scope.message);
        }
        return $scope.isValid;
    }
    $scope.checkValidEducationDtls = function () {
        $scope.isValid = false;
        $scope.message = '';
        if ($scope.education.ExamID > 0) {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message = 'Please Select Your Exam !!';
        }

        if ($scope.education.Institute != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your Institute !';
        }

        if ($scope.education.BoardUniversity != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your BoardUniversity !';
        }
        if ($scope.education.PassingYear != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your PassingYear !';
        }
        if ($scope.message != '') {
            toastr.error($scope.message);
        }
        return $scope.isValid;
    }
    $scope.checkValidExperienceDtls = function () {
        $scope.isValid = false;
        $scope.message = '';
        if ($scope.experience.Organization != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message = 'Please Insert Your Organization !';
        }

        if ($scope.experience.Position != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Select Your Position !';
        }

        if ($scope.experience.FromDate != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your FromDate !';
        }
        if ($scope.experience.ToDate != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your ToDate !';
        }
        if ($scope.message != '') {
            toastr.error($scope.message);
        }
        return $scope.isValid;
    }
    $scope.checkValidbankDtls = function () {
        $scope.isValid = false;
        $scope.message = '';
        if ($scope.personnelBank.BankID > 0) {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message = 'Please Select Your Bank !';
        }

        if ($scope.personnelBank.BankBranchID > 0) {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Select Your Branch !';
        }

        if ($scope.personnelBank.AccountNO != '') {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Select Your Account Number !';
        }
        if ($scope.personnelBank.AccountTypeID > 0) {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your Account Type !';
        }
        if ($scope.personnelBank.AccountModeID >= 0) {
            $scope.isValid = true;
        } else {
            $scope.isValid = false;
            $scope.message += '\n Please Insert Your Account Mode !';
        }
        if ($scope.message != '') {
            toastr.error($scope.message);
        }
        return $scope.isValid;
    }

    $scope.checkValidInfo = function () {
        debugger
        $scope.isValid = true;
        if (!$scope.personnelInfo.PersonnelCode != "") {
            $scope.isValid = false;
            return $scope.isValid;
        }
        if (!$scope.personnelInfo.PersonnelName != "") {
            $scope.isValid = false;
            return $scope.isValid;
        } else {
            return $scope.isValid;
        }
    }
    $scope.checkValidOfficial = function () {
        debugger
        $scope.isValid = true;
        if (!$scope.personnelOfficial.GrossSalary != "") {
            $scope.isValid = false;
            return $scope.isValid;
        } else {
            return $scope.isValid;
        }
    }
    $scope.viewDailyAttendanceReport = function (row) {
        debugger
        var reportUrl = '/Reports/Employee/ViewReport';
        var parameters = { PersonnelId: row.PersonnelId };
        printBaseOnMultiParameter(reportUrl, parameters, 'Do you want to view this Report ?', '')

    }
    // $scope.isEmployeeDtlsModal = false;
    $scope.modelClose = function () {
        //$scope.isEmployeeDtlsModal = !$scope.isEmployeeDtlsModal;
        $scope.photoList = [];
      

    }
    $scope.viewEmployeeProfileDtlsRow = function (row) {
        debugger
        $scope.personnelInfo = row;
        if ($scope.personnelInfo.MaritalStatus == 2) {
            $scope.personnelInfo.SpousName = "Not Applicable";
        }
        $scope.isReadOnly = true;

    }
})