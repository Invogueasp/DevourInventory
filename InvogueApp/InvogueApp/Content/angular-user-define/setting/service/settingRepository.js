angular.module('app').factory('settingRepository', function ($http, $location) {
    return {
    
        // ============== Category ===========================================
        loadCategory: function (id) {
            return $http.post('/Category/LoadCategory', { "categoryID": id });
        },
        saveCategory: function (company) {
            debugger
            return $http.post('/Category/SaveCategory', company);
        },
        // ============== Sub Category ===========================================
        loadSubCategory: function (id) {
            return $http.post('/SubCategory/LoadSubCategory', { "subCategoryID": id });
        },
        loadIdwiseSubCategory: function (id) {
            debugger
            return $http.post('/SubCategory/IdWiseSubCategory', { "categoryID": id });
        },
        saveSubCategory: function (subCategory) {
            debugger
            return $http.post('/SubCategory/SaveSubCategory', subCategory);
        },

        loadMachine: function(id){
            return $http.post('/Machine/LoadMachine', { "id": id });
        },
        loadModel: function(id){
            return $http.post('/Model/LoadModel', { "id": id });
        },
        loadCountry: function (id) {
            return $http.post('/Supplier/SeaarchCountry', { "id": id });
        },
        saveBrand: function (brand) {
        debugger
        return $http.post('/Brand/SaveBrand', brand);
        },
        // ============== Dept =============================================
        loadDepartment: function (id) {
            return $http.post('/Department/LoadDepartment', { "sizeID": id });
        },
        saveDept: function (size) {
            debugger
            return $http.post('/Department/SaveDepartment', size);
        },
        // ============== Unit =============================================
        loadUnit: function (id) {
            return $http.post('/Unit/LoadUnit', { "unitID": id });
        },
        saveUnit: function (unit) {
            debugger
            return $http.post('/Unit/SaveUnit', unit);
        },
        // ============== Type =============================================
        loadType: function (id) {
            return $http.post('/Type/LoadType', { "unitID": id });
        },
        saveType: function (unit) {
            debugger
            return $http.post('/Type/SaveType', unit);
        },
        // ============== Warehouse =============================================
        loadWarehouse: function (id) {
            //return $http.post('/Warehouse/LoadWarehouse', { "warehouseID": id });
            return $http.post('/Branch/LoadBranch', { "branchID": id, "branchCode": 'STORE' });
        },
        saveWarehouse: function (warehouse) {
            debugger
            return $http.post('/Warehouse/SaveWarehouse', warehouse);
        },
        // ============== Store =============================================
        //loadStore: function (id) {
        //    return $http.post('/Store/LoadStore', { "storeID": id });
        //},

        loadStore: function (id) {
            return $http.post('/Branch/LoadBranch', { "branchID": id, "branchCode": 'STORE' });
        },
         
        saveStore: function (store) {
            debugger
            return $http.post('/Store/SaveStore', store);
        },
        // ============== Store Rack =============================================
        loadStoreRack: function (id) {
            return $http.post('/StoreRack/LoadStoreRack', { "warehouseID": id });
        },
        saveStoreRack: function (store) {
            debugger
            return $http.post('/StoreRack/SaveStoreRack', store);
        },
        // ============== Store Bin =============================================
        loadStoreBin: function (id) {
            return $http.post('/StoreBin/LoadStoreBin', { "warehouseID": id });
        },
        saveStoreBin: function (storeBin) {
            debugger
            return $http.post('/StoreBin/SaveStoreBin', storeBin);
        },

        // ============== Product =============================================

        saveProduct: function (uploadImage, product) {
            var formData = new FormData();
            formData.append("file", uploadImage);
            formData.append("product", angular.toJson(product));
            var config = {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }
            return $http.post('/Product/SaveProduct', formData, config);

        },
        loadBranch: function (id, code) {
            return $http.post('/Branch/LoadBranch', { "branchID": id, "branchCode": code });
        },
        loadProduct: function (id) {
            return $http.post('/Product/LoadProduct', { "productID": id });
        },
        getMachineWiseModel: function (id) {
            return $http.post('/Model/GetMachineWiseModel', { "id": id });
        },
        loadProductWithPagenation: function (Namevalue, Pageindex, Pagesize) {
            return $http.post('/Product/LoadProductWithPagenation', { "Namevalue": Namevalue, "Pageindex": Pageindex, "Pagesize": Pagesize });
        },
        loadCategoryWiseProduct: function (categoryID,name) {
            return $http.post('/Product/LoadCategoryWiseProduct', { "categoryID": categoryID,"name":name });
        },
        getProductImgFile: function (productID) {
            return $http.post('/Product/GetProductImgFile', { "productID": productID });
        },
        loadProductDept: function (id) {
            return $http.post('/Product/LoadProductDept', { "productDeptID": id });
        },
        // ============== supplier =============================================
        loadSupplier: function (id) {
            return $http.post('/Supplier/LoadSupplier', { "supplierID": id });
        },
        loadSupplierProduct: function (id) {
            return $http.post('/Supplier/LoadSupplierProduct', { "supplierID": id });
        },
        LoadCurrency: function (id) {
            return $http.post('/Supplier/LoadCurrency', { "currencyID": id });
        },
        saveSupplier: function (supplier, supplierDtls, deleteSupplierProductRowID) {
            debugger
            return $http.post('/Supplier/SaveSupplier', { "supplier": supplier, "supplierProduct": supplierDtls, "deleteSupplierProductRowID": deleteSupplierProductRowID });
        },
        // ============== Machine =============================================
        loadMachine: function (id) {
            return $http.post('/Machine/LoadMachine', { "id": id });
        },
        
        saveMachine: function (machine) {
            return $http.post('/Machine/SaveMachine', { "machine": machine });
        },

        // ============== Model =============================================
        
        saveModel: function (model) {
            return $http.post('/Model/SaveModel', { "model": model });
        },
        // ============== Store Requisition =============================================

        saveStoreReq: function (storeReq, storeReqDtls, deleteStoreReqDtlsID) {
            debugger
            return $http.post('/StoreRequisition/SaveStoreReq', { "storeReq": storeReq, "storeReqDtls": storeReqDtls, "deleteStoreReqDtlsID": deleteStoreReqDtlsID });
        },
        loadStoreReq: function (id, status) {
            return $http.post('/StoreRequisition/LoadStoreReq', { "srID": id, "status": status });
        },
        load2ndAppStoreReq: function (id) {
            return $http.post('/StoreRequisition/LoadStore2ndAppReq', { "srID": id });
        },
        loadStoreReqDtls: function (id) {
            return $http.post('/StoreRequisition/LoadStoreDtls', { "sRID": id });
        },
        saveStoreReqApp: function (storeReq, storeReqDtls, deleteStoreReqDtlsID) {
            debugger
            return $http.post('/SRApproval/saveStoreReqApp', { "storeReq": storeReq, "storeReqDtls": storeReqDtls, "deleteStoreReqDtlsID": deleteStoreReqDtlsID });
        },
        uploadProduct: function (uploadFile) {
            var formData = new FormData();
            formData.append("file", uploadFile);
            var config = {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }
            return $http.post('/Product/UploadProduct', formData, config);
        },
    }
});