angular.module('app').factory('commonRepository', function ($http) {
    return {

        loadBanks: function () {
            return $http.post('/Bank/LoadBanks');
        },
        loadCountry: function () {
            return $http.post('/Common/LoadCountry');
        },       

        generateCode: function(tableName, fieldName, prefix){
            return $http.post('/Common/GenerateCode', { "tableName": tableName, "fieldName": fieldName, "prefix": prefix });
        },
        viewMultiParameterDocument: function (parameters, confirmationMsg, url) {
            debugger
            var reportUrl = url;
            var parameters = parameters;
            printBaseOnMultiParameter(reportUrl, parameters, confirmationMsg);
        },
        getPageValue: function () {
            var page = [
                {
                    id: 10,
                    value: '10'
                }, {
                    id: 20,
                    value: '20'
                }, {
                    id: 50,
                    value: '50'
                }, {
                    id: 80,
                    value: '80'
                }, {
                    id: 100,
                    value: '100'
                }
            ];
            return page;
        },
        getProductType: function () {
            debugger
            var type = [{
                ProductType: 1,
                label: 'Sound'
            }, {
                ProductType: 2,
                label: 'Damage'
            }];
            return type;
        },   
        getRowFinishedType: function () {
            debugger
            var type = [{
                ProductTypeID: 1,
                label: 'Raw Materials'
            }, {
                ProductTypeID: 2,
                label: 'Finished Products'
            }
            , {
                ProductTypeID: 3,
                label: 'Spare Parts'
            }];
            return type;
        },
        getSubMenu: function () {
            debugger
            var type = [{
                ID: 1,
                Name: 'PACKAGE'
            }, {
                ID: 2,
                Name: 'CLOTHINGTYPE'
            },
            {
                ID: 3,
                Name: 'FABRICS'
            },
            {
                ID: 4,
                 Name: 'DESIGN'
            },
            {
                ID: 5,
                Name: 'POCKET'
            },
            {
                ID: 6,
                Name: 'LINING'
            },
            {
                ID: 7,
                Name: 'BUTTON'
            },
            {
                ID: 8,
                 Name: 'LAPEL'
            },
            {
                ID: 9,
                Name: 'BACK'
            },
            {
                ID: 10,
                 Name: 'SLEEVE'
            },
           {
               ID: 11,
               Name: 'MEASUREMENT'
           }
            ];
            return type;
        },




        getImportType: function () {
            debugger
            var type = [{
                ImportTypeID: 1,
                Name: 'Local'
            }, {
                ImportTypeID: 2,
                Name: 'Import'
            }];
            return type;
        },

        getStatus: function () {
            var status = [{
                StatusID: '1',
                label: ''
            }, {
                StatusID: '2',
                label: 'Approve'
            }, {
                StatusID: '3',
                label: 'Pending'
            }, {
                StatusID: '4',
                label: 'Cancel'
            }, {
                StatusID: '5',
                label: ''
            }];
            return status;
        },
        getGrade: function () {
            var status = [{
                GradeID: 1,
                label: 'G-1'
            }, {
                GradeID: 2,
                label: 'G-2'
            }, {
                GradeID: 3,
                label: 'G-3'
            }, {
                GradeID:4,
                label: 'G-4'
            }, {
                GradeID: 5,
                label: 'G-5'
            }];
            return status;
        },
       
        getAfterLeave: function () {
            var leaveMonths = [{
                FirstLeaveAfterJoining: 1,
                month: '1'
            }, {
                FirstLeaveAfterJoining: 2,
                month: '2'
            }, {
                FirstLeaveAfterJoining: 3,
                month: '3'
            }, {
                FirstLeaveAfterJoining: 4,
                month: '4'
            }, {
                FirstLeaveAfterJoining: 5,
                month: '5'
            },
            {
                FirstLeaveAfterJoining: 6,
                month: '6'
            },
            {
                FirstLeaveAfterJoining: 7,
                month: '7'
            },
            {
                FirstLeaveAfterJoining: 8,
                month: '8'
            },
            {
                FirstLeaveAfterJoining: 9,
                month: '9'
            }, {
                FirstLeaveAfterJoining: 10,
                month: '10'
            },
            {
                FirstLeaveAfterJoining: 11,
                month: '11'
            },
            {
                FirstLeaveAfterJoining: 12,
                month: '12'
            },

            ];
            return leaveMonths;
        },
        getPageValue: function () {
            var page = [
                {
                    id: 10,
                    value: '10'
                }, {
                    id: 20,
                    value: '20'
                }, {
                    id: 50,
                    value: '50'
                },{
                    id: 80,
                    value: '80'
                }, {
                    id: 100,
                    value: '100'
                }
            ];
            return page;
        },
        convertTwelveHours: function (rowTime) {
            var convertedTime = '';
            var hours = rowTime.Hours;
            var minits = rowTime.Minutes;
            var amPm = ' AM';
            if (hours > 12) {
                amPm = ' PM';
                hours = hours - 12;
            }
            if (hours < 10) {
                hours = '0' + hours;
            }

            if (minits < 10) {
                minits = '0' + minits;
            }

            if (hours == 12) {
                amPm = ' PM';
            }
            convertedTime = hours + ":" + minits + amPm;
            return convertedTime;
        },
        convertWeekDay: function (day) {
            var status = '';
            switch (day) {
                case 1:
                    status = "Saturday";
                    break
                case 2:
                    status = "Sunday";
                    break
                case 3:
                    status = "Monday";
                    break
                case 4:
                    status = "Tuesday";
                    break
                case 5:
                    status = "Wednesday";
                    break
                case 6:
                    status = "Thursday";
                    break
                case 7:
                    status = "Friday";
                    break
            }
            return status
        },
        convertSecondStatusText: function (statusText) {
            var status = '';
            switch (statusText) {
                case 'P':
                    status = "Pending";
                    break
                case 'A':
                    status = "Pending";
                    break
                case '2A':
                    status = "Approved";
                    break
                case '3A':
                    status = "Approved";
                    break
                case 'IF':
                    status = "Approved";
                    break
                case 'IP':
                    status = "Approved";
                    break
            }
            return status
        },
        convertStatusText: function (statusID) {
            var status = '';
            switch (statusID) {
                case '1':
                    status = 'Create';
                    break
                case '2':
                    status = 'Approved';
                    break
                case '3':
                    status = 'Pending';
                    break
                case '4':
                    status = 'canceled';
                    break
                case '5':
                    status = 'Updated';
                    break
            }
            return status
        },
        convertInstallmentStatus: function (statusID) {
            var status = '';
            switch (statusID) {
                case 'F':
                    status = 'Full Paid';
                    break
                case 'P':
                    status = 'Partial Paid';
                    break
                case 'D':
                    status = 'Due';
                    break
            }
            return status
        },
        convertPaymentMode: function (paymentID) {
            var status = '';
            switch (paymentID) {
                case "1":
                    status = 'Cash';
                    break;
                case "2":
                    status = 'Bank';
                    break;
            }
            return status
        },

        getDebitCredit: function () {
            var type = [{
                BalanceType: '1',
                label: 'Debit'
            }, {
                BalanceType: '2',
                label: 'Credit'
            }];
            return type;
        },

        viewDocument: function (id, alertMessage) {
            var confirmatonMsg = alertMessage;
            var reportUrl = '/Reports/BoqReports/ViewBoqDtlsReport/';
            var transactionID = id;
            printDocument(reportUrl, transactionID, confirmatonMsg); //function location asset/custom.js       
        },
        getPayType: function () {
            var payType = [{
                id: 1,
                label: "Regular"
            },
            {
                id: 2,
                label: "Consolidate"
            }];
            return payType;
        },
        convertPayTypeText: function (payTypeID) {
            var payType = '';
            switch (payTypeID) {
                case 1:
                    payType = 'Regular'
                    break;
                case 2:
                    payType = 'Consolidate';
                    break;

            }
            return payType;
        },

        getDay: function () {
            var WeekDays = [{
                Day: 1,
                label: "Saturday",
                StartTime: '',
                EndTime: '',
                ShiftWeeklyDaysID:0
            },
            {
                Day: 2,
                label: "Sunday",
                Start: '',
                EndTime: '',
                ShiftWeeklyDaysID: 0
            },
            {
                Day: 3,
                label: "Monday",
                StartTime: '',
                EndTime: '',
                ShiftWeeklyDaysID: 0
            },
            {
                Day: 4,
                label: "Tuesday",
                StartTime: '',
                EndTime: '',
                ShiftWeeklyDaysID: 0
            },
            {
                Day: 5,
                label: "Wednesday",
                StartTime: '',
                EndTime: '',
                ShiftWeeklyDaysID: 0
            },
            {
                Day: 6,
                label: "Thursday",
                StartTime: '',
                EndTime: '',
                ShiftWeeklyDaysID: 0
            },
            {
                Day: 7,
                label: "Friday",
                StartTime: '',
                EndTime: '',
                ShiftWeeklyDaysID: 0
            },

            ];
            return WeekDays;
        },


        getEmployeeLevel: function () {
            var empLevel = [{
              
                EmpLevelID: 1,
                label: "Top Management"

            },
            {
                
                EmpLevelID: 2,
                label: "Middle Management"
            },
             {
                
                 EmpLevelID: 3,
                 label: "Lower Management"
             },
             {
                 
                 EmpLevelID: 4,
                 label: "Worker"
             }
            ];
            return empLevel;
        },

        getAStatus: function () {
            var AStatus = [{
                id: 1,
                label: "Active"
            },
            {
                id: 0,
                label: "Inactive"
            }];
            return AStatus;
        },
        convertAStatusText: function (aStatusID) {
            var AStatus = '';
            switch (aStatusID) {
                case 1:
                    AStatus = 'Active';
                    break;
                case 0:
                    AStatus = 'Inactive';
                    break;

            }
            return AStatus;
        },
        getReligion: function () {
            var religion = [{
                id: 1,
                label: "Islam"
            }, {
                id: 2,
                label: "Hindus"
            },
            {
                id: 3,
                label: "Christians"
            },
            {
                id: 4,
                label: "Irreligious and atheist"
            },
            {
                id: 5,
                label: "Buddhists"
            },
            {
                id: 6,
                label: "Sikhism"
            },
            {
                id: 7,
                label: "Spiritism"
            }];
            return religion;
        },
        convertReligionText: function (religionID) {
            var religion = '';
            switch (religionID) {
                case 1:
                    religion = 'Islam';
                    break;
                case 2:
                    religion = 'Hindus';
                    break;
                case 3:
                    religion = 'Christians';
                    break;
                case 4:
                    religion = 'Irreligious and atheist';
                    break;
                case 5:
                    religion = 'Buddhists';
                    break;
                case 6:
                    religion = 'Sikhism';
                    break;
                case 7:
                    religion = 'Spiritism';
                    break;

            }
            return religion
        },
        getMonth: function () {
            var months = [];
            for (var i = 1; i <= 31; i++) {
                var month = {
                    id: i,
                    label: i
                };
                months.push(month);
            }
            return months;
        },
        getGender: function () {
            var gender = [{
                id: 1,
                label: "Male"
            }, {
                id: 2,
                label: "Female"
            },
            {
                id: 3,
                label: "Others"
            }];
            return gender;
        },
        getCountry: function () {
            var country = [{
                id: 1,
                label: "Bangladesh"
            }, {
                id: 2,
                label: "Pakistan"
            },
            {
                id: 3,
                label: "India"
            },
            {
                id: 4,
                label: "United Kingdom (UK)"
            },
            {
                id: 5,
                label: "Canada"
            }
            ];
            return country;
        },
        convertGenderText: function (genderID) {
            var gender = '';
            switch (genderID) {
                case 1:
                    gender = 'Male';
                    break;
                case 2:
                    gender = 'Female';
                    break;
                case 3:
                    gender = 'Others';
                    break;

            }
            return gender;
        },
        getMaritalStatus: function () {
            var maritalStatus = [{
                id: 1,
                label: "Married"
            }, {
                id: 2,
                label: "Un_married"
            },
            {
                id: 3,
                label: "Others"
            }];
            return maritalStatus;
        },
        convertMaritalStatusText: function (maritalStatusID) {
            var maritalStatus = '';
            switch (maritalStatusID) {
                case 1:
                    maritalStatus = 'Married';
                    break;
                case 2:
                    maritalStatus = 'Single';
                    break;
                case 3:
                    maritalStatus = 'Others';
                    break;

            }
            return maritalStatus;
        },
        getMStatus: function () {
            var maritalStatus = [{
                id: 1,
                label: "Married"
            }, {
                id: 2,
                label: "Single"
            },
            {
                id: 3,
                label: "Others"
            }];
            return maritalStatus;
        },
        convertMStatusText: function (mStatusID) {
            var MStatus = '';
            switch (mStatusID) {
                case 1:
                    MStatus = 'Married';
                    break;
                case 2:
                    MStatus = 'Un_married';
                    break;
                case 3:
                    MStatus = 'Others';
                    break;

            }
            return MStatus;
        },
        getBGroup: function () {
            var BGroup = [{
                id: 1,
                label: "A+"
            }, {
                id: 2,
                label: "A-"
            },
            {
                id: 3,
                label: "B+"
            },
            {
                id: 4,
                label: "B-"
            },
            {
                id: 5,
                label: "AB+"
            },
            {
                id: 6,
                label: "AB-"
            },
            {
                id: 7,
                label: "O+"
            },
            {
                id: 8,
                label: "O-"
            }];
            return BGroup;
        },
        convertBGroupText: function (bGroupID) {
            var BGroup = '';
            switch (bGroupID) {
                case 1:
                    BGroup = 'A+';
                    break;
                case 2:
                    BGroup = 'A-';
                    break;
                case 3:
                    BGroup = 'B+';
                    break;
                case 4:
                    BGroup = 'B-';
                    break;
                case 5:
                    BGroup = 'AB+';
                    break;
                case 6:
                    BGroup = 'AB-';
                    break;
                case 7:
                    BGroup = 'O+';
                    break;
                case 8:
                    BGroup = 'O-';
                    break;

            }
            return BGroup;
        },
        getAType: function () {
            var AType = [{
                id: 1,
                label: "Saving Account"
            },
            {
                id: 2,
                label: "Current Account"
            }];
            return AType;
        },
        convertATypeText: function (aTypeID) {
            var AType = '';
            switch (aTypeID) {
                case 1:
                    AType = 'Saving Account';
                    break;
                case 2:
                    AType = 'Current Account';
                    break;

            }
            return AType;
        },
        getLeaveType: function () {
            var LType = [{
                    LeaveTypeID: "LWP",
                    LeaveType: "LWP"
                },
                {
                    LeaveTypeID: "Policy",
                    LeaveType: "Policy"
                }];
            return LType;
        },

        getYear: function (startYear) {
            debugger
            var currentYear = new Date().getFullYear(), years = [];
            startYear = startYear || 1970;  
            while ( startYear <= currentYear ) {
                years.push({
                    startYears: startYear++
                });
            }   
            return years;
        },
        // Attendance Section
        getLateStatus: function () {
            var status = [{
                StatusID: 'A',
                label: 'All'
            }, {
                StatusID: 'L',
                label: 'Late'
            }, {
                StatusID: 'R',
                label: 'Regular'
            }];
            return status;
        },

        getPurpose: function () {
            var Purpose = [{
                PurposeID: 'O',
                label: 'Official'
            }, {
                PurposeID: 'P',
                label: 'Personal'
            }];
            return Purpose;
        },

        getMonthWithDays: function () {
            var month = [{
                monthID: '1',
                label: 'January'
            }, {
                monthID: '2',
                label: 'February'
            }, {
                monthID: '3',
                label: 'March'
            }, {
                monthID: '4',
                label: 'April'
            }, {
                monthID: '5',
                label: 'May'
            }, {
                monthID: '6',
                label: 'Jun'
            }, {
                monthID: '7',
                label: 'July'
            }, {
                monthID: '8',
                label: 'August'
            }, {
                monthID: '9',
                label: 'September'
            }, {
                monthID: '10',
                label: 'October'
            }, {
                monthID: '11',
                label: 'November'
            }, {
                monthID: '12',
                label: 'December'
            }];
            return month;
        },
        getDays: function () {
            var days = [{
                id: 1,
                label: 'Current Day'
            }, {
                id: 5,
                label: 'Last 5 Days'
            }, {
                id: 10,
                label: 'Last 10 Days'
            }, {
                id: 20,
                label: 'Last 20 Days'
            }, {
                id: 30,
                label: 'Last 30 Days'
            }];
            return days;
        },

        convertLeaveStatus: function (statusID) {
            var status = '';
            switch (statusID) {
            case 'A':
                status = 'Approved';
                break
            case 'C':
                status = 'Cancel';
                break
            case 'N':
                status = 'Not Approval';
                break
            }
            return status;
        },
        getAfterMonth: function (monthid) {
            var months = [];
            for (var i = 1; i <= monthid; i++) {
                if (i > 1) {
                    var month = {
                        id: i,
                        label: i + " " + "Months"
                    };
                } else {
                    var month = {
                        id: i,
                        label: i + " " +  "Month"
                    };
                }
                
                months.push(month);
            }
            return months;
        },

       
    }
})