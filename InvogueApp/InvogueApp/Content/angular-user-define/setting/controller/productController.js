angular.module('app').controller("productController", function ($scope, $location, $route, $filter, $cookieStore, $templateCache, settingRepository, commonRepository) {
    $scope.product = {};
    $scope.statusList = [];
    $scope.productRowFinishList = [];

    $scope.executiveElitPackage = false;
    $scope.luxPackage = false;
    $scope.setField = function (packageID) {
        if (packageID == 1 || packageID == 2) {  //executive, elite package
            $scope.executiveElitPackage = true;
            $scope.luxPackage = false;
        } else {
            $scope.luxPackage = true;
            $scope.executiveElitPackage = false;
        }
    }
    $scope.maxSize = 5;
    $scope.totalItemCount = 0;
    $scope.Pageindex = 1;
    $scope.Pagesize = 10;
    $scope.Namevalue = "";
    $scope.workPagination = function () {
        $scope.pageValueList = commonRepository.getPageValue();
        settingRepository.loadProductWithPagenation($scope.Namevalue, $scope.Pageindex, $scope.Pagesize).then(function (response) {
            if (response.data) {
                debugger
                $scope.productList = response.data.productLists;
                $scope.totalCount = response.data.productCounts;
            }
        })

    }

    $scope.pageChanged = function () {
        $scope.workPagination();
    }
    $scope.changePageSize = function () {
        $scope.pageIndex = 1;
        $scope.workPagination();
    }
    $scope.loadDropdowns = function () {
        $scope.statusList = commonRepository.getAStatus();
        $scope.importList = commonRepository.getImportType();
        $scope.product.ImportTypeID = $scope.importList[0].ImportTypeID;

        $scope.loadUnit();
        $scope.loadType();
        $scope.loadCategory();
        $scope.loadBranch();        
        $scope.loadMachine();
        $scope.loadCountry();
        $scope.loadDepartment();
        $scope.productRowFinishList = commonRepository.getRowFinishedType();

        debugger
        $scope.product = $cookieStore.get('editproduct');        
        
        if ($scope.product.ProductID > 0) {
            $scope.loadSubCategory();
            $scope.loadProductDepartment($scope.product.ProductID);
            $scope.getMachineWiseModel($scope.product.MachineID);
            $scope.createImage = false;
            $scope.dbImage = true;
            $scope.getProductImgFile($scope.product.ProductID);
            
        }
        
        $scope.product.MachineID = 1;
        $scope.product.ModelID = 1;
        $scope.product.TypeID = 1;
        $scope.product.ImportTypeID = 1;
    }
    $scope.Reload = function () {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
    $scope.removeCookies = function () {
        $scope.product = {};
        $cookieStore.put('editproduct', $scope.product);
    }
    $scope.clearData = function () {
        $scope.product = {};
        $cookieStore.put('editproduct', $scope.product);
        $scope.Reload();
    }
    $scope.deptLists = [];
    $scope.editDeptID = [];
    $scope.addDept = function (dept) {
        debugger
        if (dept.selected == true) {
            $scope.deptLists.push({
                ProductDeptID: 0,
                ProductID: $scope.product.ProductID,
                DepartmentID: dept.DepartmentID
            });
        } else {
            for (var i = 0; i < $scope.deptLists.length; i++) {
                if ($scope.deptLists[i].DepartmentID == dept.DepartmentID) {
                    $scope.deptLists.splice(i, 1);
                    $scope.editDeptID.push(dept.DepartmentID);
                }

            }

        }
    }
    //for edit 
    $scope.changeDept = function (dept, productDeptID) {
        for (var i = 0; i < $scope.deptLists.length; i++) {
            if ($scope.deptLists[i] != null && $scope.deptLists[i].DepartmentID == dept) {
                $scope.deptLists[i].DepartmentID = dept;
                $scope.deptLists[i].ProductDeptID = productDeptID;
                $scope.deptLists[i].ProductID = $scope.product.ProductID;
                return;
            }
        }
        $scope.deptLists.push({
            ProductDeptID: productDeptID,
            ProductID: $scope.product.ProductID,
            DepartmentID: dept.DepartmentID
        });
    }
    // ========================= Save Product ===========================
    $scope.allData = {};
    $scope.saveProduct = function () {
        debugger
        $scope.allData = {
            ProductDept: $scope.deptLists,
            Product: $scope.product,
            editProductdeptID: $scope.editDeptID
        };
        if ($scope.isValidData()) {
            var saveType = settingRepository.saveProduct($scope.selectFileUpload, $scope.allData).then(function (response) {
                if (response.data.isSucess) {
                    
                    toastr.success(response.data.message);
                    $scope.clearData();
                    $scope.Reload();
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

    $scope.isValidData = function () {
        debugger
        $scope.isValid = true;
        if (!$scope.product.BranchID) {
            $scope.isValid = false;
        }

        if (!$scope.product.CategoryID) {
            $scope.isValid = false;
        }
        if (!$scope.product.Name) {
            $scope.isValid = false;
        }
        if (!$scope.product.MachineID) {
            $scope.isValid = false;
        }
        if (!$scope.product.ModelID) {
            $scope.isValid = false;
        }
        if (!$scope.product.TypeID) {
            $scope.isValid = false;
        }
        if (!$scope.product.UnitID) {
            $scope.isValid = false;
        }
        if (!$scope.product.Status) {
            $scope.isValid = false;
        }
        return $scope.isValid;
    }


    $scope.loadProduct = function () {
        
        $scope.productList = [];
        settingRepository.loadProduct().then(function (response) {
            if (response.data) {
                debugger
                $scope.productList = response.data;
            }
        })
    }


    $scope.loadBranch = function () {
        
        $scope.branchList = [];
        settingRepository.loadBranch().then(function (response) {
            if (response.data) {                
                $scope.branchList = response.data;
                $scope.product.BranchID = response.data[0].BranchID;
            }
        })
    }

    $scope.loadMachine = function () {
        debugger
        $scope.machineList = [];
        settingRepository.loadMachine().then(function (response) {
            if (response.data) {
                debugger
                $scope.machineList = response.data;                
            }
        })
    }

    $scope.loadUnit = function () {
        
        $scope.unitList = [];
        settingRepository.loadUnit().then(function (response) {
            if (response.data) {
                $scope.unitList = response.data;
            }
        })
    }
    $scope.loadProductDepartment = function (productID) {
        $scope.productDepartmentList = [];
        settingRepository.loadProductDept(productID).then(function (response) {
            if (response.data) {
                $scope.productDepartmentList = response.data;
            }
            if ($scope.deptList.length > 0) {
                for (var i = 0; i < $scope.deptList.length; i++) {
                    var dept = $filter('filter')($scope.productDepartmentList, function (d) { return d.DepartmentID === $scope.deptList[i].DepartmentID; })[0];
                    if (dept != null && dept.DepartmentID > 0) {
                        $scope.deptList[i].selected = true;
                        $scope.changeDept(dept, dept.ProductDeptID);
                    }
                }
            }
            
        })
    }
    $scope.loadType = function () {
        
        $scope.typeList = [];
        settingRepository.loadType().then(function (response) {
            if (response.data) {
                $scope.typeList = response.data;
            }
        })
    }
    $scope.loadDepartment = function () {
        
        $scope.deptList = [];
        settingRepository.loadDepartment().then(function (response) {
            if (response.data) {
                $scope.deptList = response.data;
            }
            if ($scope.productDepartmentList.length > 0) {
                for (var i = 0; i < $scope.deptList.length; i++) {
                    var dept = $filter('filter')($scope.productDepartmentList, function (d) { return d.DepartmentID === $scope.deptList[i].DepartmentID; })[0];
                    if (dept != null && dept.DepartmentID > 0) {
                        $scope.deptList[i].selected = true;
                        $scope.changeDept(dept, dept.ProductDeptID);
                    }
                }
            }
            


        })
    }
    $scope.loadCategory = function () {
        
        $scope.categoryList = [];
        settingRepository.loadCategory().then(function (response) {
            if (response.data) {
                $scope.categoryList = response.data;
            }
        })
    }
    $scope.loadSubCategory = function () {
        $scope.subCategoryList = [];
        settingRepository.loadIdwiseSubCategory($scope.product.CategoryID).then(function (response) {
            if (response.data) {
                $scope.subCategoryList = response.data;
            }
        })
    }
    $scope.loadMachine = function () {
        $scope.machineList = [];
        settingRepository.loadMachine().then(function (response) {
            if (response.data) {
                $scope.machineList = response.data;
            }
        })
    }
    $scope.getMachineWiseModel = function (id) {
        $scope.modelList = [];
        settingRepository.getMachineWiseModel(id).then(function (response) {
            if (response.data) {
                $scope.modelList = response.data;               
            }
        })
    }

    $scope.loadCountry = function () {        
        $scope.countryList = [];
        commonRepository.loadCountry().then(function (response) {
            if (response.data) {
                $scope.countryList = response.data;
            }
        })
    }

    $scope.editRow = function (row) {
        debugger
        $scope.product = row;
        $scope.loadProductDepartment(row.ProductID);
        $cookieStore.put('editproduct', $scope.product);

        $location.path("/ProductCreate");

    }

    //save Img
    // Single File Select event

    $scope.selectFileUpload = "";
    $scope.stepsModel = "";
    $scope.singleImageUpload = function (event) {
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
    
    $scope.getProductImgFile = function (productID) {
        $scope.productImg = [];
        settingRepository.getProductImgFile(productID).then(function (response) {
            if (response.data) {
                for (var i = 0; i < response.data.length; i++) {
                    response.data[i].FullName = $scope.product.ProductID;
                }
                $scope.productImg = response.data;
            }
        }).catch(function (response) {
            toastr.warning('No Data Found!!');
        });
    }

    $scope.selectApplicationFile = function (file) {
        var filename = file[0].name;
        var index = filename.lastIndexOf(".");
        var ext = filename.substring(index, filename.length);
        if (ext == '.csv') {
            $scope.selectedApplicationFile = file[0];
            var reader = new FileReader();
            reader.onload = function (event) {
                $scope.image_source = event.target.result;
                $scope.$apply();
            }
            reader.readAsDataURL(file[0]);
        }
        else {
            toastr.error("Allowed file formats are: csv");
            angular.element("input[type='file']").val(null);
        }
    }

    $scope.uploadProduct = function () {
        if ($scope.selectedApplicationFile != null) {
            settingRepository.uploadProduct($scope.selectedApplicationFile).then(function (response) {
                if (!response.data && response.data.isSucess == false) {
                    toastr.success(response.data.message);
                } else {
                    angular.element("input[type='file']").val(null);
                    $scope.Reload();
                    $scope.loadEmpMealList = response.data;
                }
            }).catch(function (response) {
                toastr.error($scope.name += response.data + '!!');
            });

        } else {
            toastr.error("All input field are not valid");
        }
    }

})