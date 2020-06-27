angular.module('app').controller("shiftController", function ($scope, $location, $cookieStore,settingsRepository, commonRepository) {
    $scope.shift = {};
    $scope.dayList = [];
    $scope.shiftList = [];
    $scope.loadDropdowns = function () {
        debugger
        $scope.dayList = commonRepository.getDay();
        $scope.shift = $cookieStore.get('shiftDtls');
        if ($scope.shift.ShiftID != null) {
            $scope.loadShiftDtls($scope.shift.ShiftID);
        }
    }

    $scope.removeCookies = function () {
        $scope.shift = {};
        $cookieStore.put('shiftDtls', $scope.shift);
    }
    $scope.selectCopyTime = false;
    $scope.moreShift = function (row) {
        debugger
        for (i = 0; i <= $scope.dayList.length; i++) {
            if ($scope.dayList[i].label == row.label) {
               
                $scope.dayList[i].StartTime = row.StartTime;
                $scope.dayList[i].EndTime = row.EndTime;
                if ($scope.dayList[i].StartTime != "" && $scope.dayList[i].EndTime != "") {

                    $scope.selectCopyTime = true;
                }

            }
        }
        
    }
    $scope.sameTimeSelect = function () {
        debugger
        if ($scope.select == true) {
            for (i = 0; i <= $scope.dayList.length; i++) {
                if ($scope.dayList[i].StartTime != "" && $scope.dayList[i].EndTime != "") {

                    var start = $scope.dayList[i].StartTime;
                    var end = $scope.dayList[i].EndTime;

                }
             
                $scope.dayList[i].StartTime = start;
                $scope.dayList[i].EndTime = end;

            }
        }
        else {
            for (i = 0; i <= $scope.dayList.length; i++) {

                $scope.dayList[i].StartTime = "";
                $scope.dayList[i].EndTime = "";

            }
        }
    }

    $scope.loadShift = function () {
        settingsRepository.loadShift().then(function (response) {
            if (response.data) {
                $scope.shiftList = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data found!!');
        });
    }
    $scope.selectCopyTime = false;
    $scope.loadShiftDtls = function (shiftID) {
        settingsRepository.loadShiftWithTime(shiftID).then(function (response) {
            if (response.data) {
                
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
                
                $scope.selectCopyTime = false;
                $scope.shiftDtlsList = response.data;
                for (i = 0; i <= $scope.shiftDtlsList.length; i++) {
                    if ($scope.dayList[i].label == $scope.shiftDtlsList[i].Day) {
                        $scope.dayList[i].StartTime = $scope.shiftDtlsList[i].StartTime;
                        $scope.dayList[i].EndTime = $scope.shiftDtlsList[i].EndTime;
                        $scope.dayList[i].ShiftWeeklyDaysID = $scope.shiftDtlsList[i].ShiftWeeklyDaysID;
                        if ($scope.dayList[i].StartTime != "" && $scope.dayList[i].EndTime != "") {
                            $scope.selectCopyTime = false;
                        }
                    }
                }
                
            }           


        });
    }

    $scope.timeFilter = function () {
        debugger
        for (i = 0; i <= $scope.dayList.length; i++) {

            if ($scope.dayList[i].StartTime) {
                var TimeWithoutAmPM = $scope.dayList[i].StartTime;

                time = TimeWithoutAmPM.split(" ");

                time = time[0] + " ";
                $scope.dayList[i].StartTime = time;
            }

            if ($scope.dayList[i].EndTime) {
                var TimeWithoutAmPM = $scope.dayList[i].EndTime;

                time = TimeWithoutAmPM.split(" ");

                time = time[0] + " ";
                $scope.dayList[i].EndTime = time;
            }
            $scope.saveShiftWithtime();
        }
       
       
     
    }


    $scope.saveShiftWithtime = function () {


        if ($scope.shiftForm.$valid) {
            var saveCustomer = settingsRepository.saveShift($scope.shift, $scope.dayList).then(function (response) {
                if (response.data.isSucess) {
                    debugger
                    toastr.success(response.data.message);
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

    $scope.editRow = function (row) {
        debugger        
        $cookieStore.put('shiftDtls', row);
        $location.path('/ShiftCreate');
    }

    $scope.clearData = function (row) {
        debugger
        $scope.select = false;
        $scope.shift = {};
        for (i = 0; i <= $scope.dayList.length; i++) {
            $scope.dayList[i].StartTime = "";
            $scope.dayList[i].EndTime = "";
            $scope.selectCopyTime = false;
            
        }
       
    }
});