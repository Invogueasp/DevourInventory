angular.module('app').controller('userController', function ($scope, $filter, $location, $route, $templateCache, $cookieStore, securityRepository, settingRepository, settingsRepository, hrmRepository) {

        //start :: ng-idel => when user idel specific time then it will be auto logout
        $scope.$on('IdleStart', function () {
            toastr.warning('your session will be end soon!!');
        });

        $scope.$on('IdleTimeout', function () {
            window.location = "/Security/LogOff#!/";
        });
        //End :: ng-idel

        $scope.SecurityQuestions = [];
        $scope.SecurityQuestions = ["Which one is your favorite sport Team", "Where is your birth Place", "What is the name of your first School", "Who is your favorite Athlete"];


        $scope.Reload = function () {
            var currentPageTemplate = $route.current.templateUrl;
            $templateCache.remove(currentPageTemplate);
            $route.reload();
        }

        $scope.rowCollection = [];
        $scope.user = {};

        $scope.loadDropdowns = function () {
            $scope.companyWiseBranch();
            //$scope.loadEmployee();
            $scope.loadDepartment();
            $scope.loadCompany();
            var loadDropdownUserGroup = securityRepository.getDropdownUserGroupData();
            loadDropdownUserGroup.then(function (response) {
                $scope.userGroupList = response.data.data;
            });
            
        }


        $scope.loadDepartment = function () {
            debugger
            $scope.deptList = [];
            settingRepository.loadDepartment().then(function (response) {
                if (response.data) {
                    $scope.deptList = response.data;
                }
            })
        }





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
            debugger
            $scope.branchList = [];
            var branchesList = settingsRepository.loadBranch();
            //var branchesList = settingRepository.loadBranch();
            branchesList.then(function (response) {
                debugger
                $scope.branchList = response.data;
            });
        }
        //$scope.companyWiseBranch = function (companyID) {
        //    debugger 
        //    var branchesList = hrmRepository.searchcompanyWiseBranch(companyID);
        //    branchesList.then(function (response) {
        //        $scope.branchList = response.data;
        //    }).catch(function (response) {
        //        toastr.warning('No Data Found!!');
        //    });
        //}
        $scope.loadEmployee = function () {
            var getUser = securityRepository.loadEmployee();
            getUser.then(function (response) {
                if (response.data) {
                    $scope.employeeList = response.data;
                }
            });
            //.catch(function (response) {
            //    toastr.error("Data Not Found!!");
            //});
        }
        $scope.getEmployeeName = function () {
            debugger 
            if ($scope.editUser.ID > 0) {
                var emp = $filter('filter')($scope.employeeList, function (d) { return d.PersonnelId === $scope.editUser.PersonnelId; })[0];
                var personnelName = emp.PersonnelName;
                hyphen = personnelName.split(" ");
                hyphen = hyphen[0] + " " + hyphen[1] + "";
                $scope.editUser.UserFullName = hyphen;
            } else {
                var emp = $filter('filter')($scope.employeeList, function (d) { return d.PersonnelId === $scope.user.PersonnelId; })[0];
                var personnelName = emp.PersonnelName;
                hyphen = personnelName.split(" ");
                hyphen = hyphen[0] + " " + hyphen[1] + "";
                $scope.user.UserFullName = hyphen;
            }
            
        }
        $scope.getUser = function () {
            var getUser = securityRepository.getAllUserData();
            getUser.then(function (response) {
                if (response.data) {
                    $scope.rowCollection = response.data;
                }
            }).catch(function (response) {
                $scope.name += response.data.toUpperCase() + '!!';
            });
            $scope.itemsByPage = 10;
        }

        $scope.deleteRow = function (id) {
            var deleteUser = securityRepository.deleteUser(id);
            deleteUser.then(function (response) {
                if (response.data.success === true) {
                    $scope.Reload();
                    toastr.success(response.data.message);
                } else {
                    toastr.error(response.data.message);
                }
            });
        }

        $scope.activeOrInActive = function (id, status) {
            var userStatus = securityRepository.activeDeActiveUser(id, status);
            userStatus.then(function (response) {
                if (response.data.success === true) {
                    $scope.Reload();
                    toastr.success(response.data.message);
                } else {
                    toastr.error(response.data.message);
                }
            });
        }

        $scope.filterValueForNumberOnly = function (event) {
            var key = window.event ? event.keyCode : event.which;
            if ((key >= 48 && key <= 57)|| key === 8 || key === 46) {
                return true;
            }
            else {
                return;
            }
        }
        

        //Create User
        $scope.saveUser = function () {
            debugger
            var newPassword = $scope.user.Password;
            var confirmPassword = $scope.user.ConfirmPassword;
            if (newPassword.length < 6 || confirmPassword < 6) {
                toastr.error("Your password will be minimum 6 character");
                return;
            }
            if ($scope.user.Password !== $scope.user.ConfirmPassword) {
                toastr.error("New and Confirm Password are not matched");
                return;
            }
            if ($scope.CreateUserForm.$valid) {
                var createUser = securityRepository.saveUserData($scope.user);
                createUser.then(function (response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clierForm();
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
            $cookieStore.put('user', row);
            $location.path('/EditUser');
        }
        $scope.editUser = {};
        $scope.editUserByID = function () {
            $scope.loadDropdowns();
            $scope.editUser = $cookieStore.get('user');
            debugger 
            if ($scope.editUser.CompanyID) {
                $scope.companyWiseBranch($scope.editUser.CompanyID);
            }
        }
        $scope.clierForm = function () {
            $scope.user = {};
            $scope.editUser = {};
            $cookieStore.put('user', $scope.editUser);
        }


        $scope.updateUser = function () {
            debugger
            if ($scope.UpdateUserForm.$valid) {
                $scope.data = {
                    ID: $scope.editUser.ID,
            
                    SecurityQuestion: $scope.editUser.SecurityQuestion,
                    SecutiryAnswer: $scope.editUser.SecutiryAnswer
                };
                var editUser = securityRepository.updateUserData($scope.editUser);
                editUser.then(function(response) {
                    if (response.data.isSucess) {
                        toastr.success(response.data.message);
                        $scope.clierForm();
                    } else {
                        if (response.data.message === "LogOut") {
                            $location.path('#!/LogIn');
                        }
                        toastr.error(response.data.message);
                    }
                });
            } else {
                toastr.error("All input field are not valid");
            }
        }


    });
