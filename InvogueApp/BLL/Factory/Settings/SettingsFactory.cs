using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Settings;
using BLL.Models;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory.Settings
{

    //public class ShiftFactory : GenericFactory<DevourInvEntities, SET_Shift>
    //{

    //}

    public class SettingsFactory : GenericFactory<DevourInvEntities, SET_Company>
    {

    }
    
    public class SettingsFactorys : ICompanyFactory
    {
        private IGenericFactory<SET_Company> _companyFactory;
        Result _result = new Result();
        string tableName = "Company";

        public Result SaveCompany(SET_Company company)
        {
            _companyFactory = new SettingsFactory();
            try
            {
                if (company.CompanyID> 0)
                {
                    _companyFactory.Edit(company);
                    _result = _companyFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int CompanyID = 1;
                    var prvCompanyID = _companyFactory.GetLastRecord().OrderByDescending(x => x.CompanyID).FirstOrDefault();

                    if (prvCompanyID != null)
                    {
                        CompanyID = prvCompanyID.CompanyID + 1;
                    }
                    company.CompanyID = CompanyID;
                    _companyFactory.Add(company);
                    _result = _companyFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<SET_Company> SearchCompany(int? companyID)
        {
            _companyFactory = new SettingsFactory();
            try
            {
                var list = new List<SET_Company>();
                if (companyID > 0)
                {
                    list = _companyFactory.FindBy(x => x.CompanyID == companyID).ToList();
                }
                else
                {
                    list = _companyFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
   public class CategoryFactory : GenericFactory<DevourInvEntities, INV_Category>
    {

    }
   public class SubCategoryFactory : GenericFactory<DevourInvEntities, INV_SubCategory>
   {

   }

   public class BrandFactory : GenericFactory<DevourInvEntities, INV_Brand>
   {

   }
   public class DepartmentFactory : GenericFactory<DevourInvEntities, INV_Department>
   {

   }
   public class UnitFactory : GenericFactory<DevourInvEntities, INV_Unit>
   {

   }
   public class CountryFactory : GenericFactory<DevourInvEntities, SET_Country>
   {

   }
   public class TypeFactory : GenericFactory<DevourInvEntities, INV_Type>
   {

   }

   public class StoreRackFactory : GenericFactory<DevourInvEntities, INV_StoreRack>
   {

   }
   public class StoreBinFactory : GenericFactory<DevourInvEntities, INV_StoreBin>
   {

   }

   public class ProductFactory : GenericFactory<DevourInvEntities, INV_Product>
   {

   }

   public class ProductDepartmentFactory : GenericFactory<DevourInvEntities, INV_ProductDepartment>
   {

   }
   public class SupplierFactory : GenericFactory<DevourInvEntities, INV_Supplier>
   {

   }
   public class SupplierDtlsFactory : GenericFactory<DevourInvEntities, INV_SupplierProduct>
   {

   }

   public class CurrencyFactory : GenericFactory<DevourInvEntities, SET_Currency>
   {

   }
   public class MachineFactory : GenericFactory<DevourInvEntities, INV_Machine>
   {

   }
   public class ModelFactory : GenericFactory<DevourInvEntities, INV_Model>
   {

   }
   public class SubCategoryFactorys : ISubCategoryFactory
   {
       private IGenericFactory<INV_SubCategory> _subCategoryFactory;
       Result _result = new Result();
       string tableName = "Sub Category";

       public Result SaveSubCategory(INV_SubCategory subCategory)
       {
           _subCategoryFactory = new SubCategoryFactory();
           try
           {
               if (subCategory.SubCategoryID > 0)
               {
                   _subCategoryFactory.Edit(subCategory);
                   _result = _subCategoryFactory.Save();
                   if (_result.isSucess)
                   {
                       _result.message = _result.UpdateSuccessfull(tableName);
                   }
               }
               else
               {
                   int subcategoryID = 1;
                   var prvsubcategoryID = _subCategoryFactory.GetLastRecord().OrderByDescending(x => x.SubCategoryID).FirstOrDefault();

                   if (prvsubcategoryID != null)
                   {
                       subcategoryID = prvsubcategoryID.SubCategoryID + 1;
                   }
                   subCategory.SubCategoryID = subcategoryID;
                   _subCategoryFactory.Add(subCategory);
                   _result = _subCategoryFactory.Save();
                   if (_result.isSucess)
                   {
                       _result.message = _result.SaveSuccessfull(tableName);
                   }
               }

           }
           catch (Exception ex)
           {
               _result.isSucess = false;
               _result.message = ex.Message;
           }
           return _result;
       }
       public List<INV_SubCategory> SearchSubCategory(int? subCategoryID)
       {
           _subCategoryFactory = new SubCategoryFactory();
           try
           {
               var list = new List<INV_SubCategory>();
               if (subCategoryID > 0)
               {
                   list = _subCategoryFactory.FindBy(x => x.SubCategoryID == subCategoryID).ToList();
               }
               else
               {
                   list = _subCategoryFactory.GetAll().ToList();
               }
               return list;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public List<INV_SubCategory> IdWiseSubCategory(int? categoryID)
       {
           _subCategoryFactory = new SubCategoryFactory();
           try
           {
               var list = new List<INV_SubCategory>();
               if (categoryID > 0)
               {
                   list = _subCategoryFactory.FindBy(x => x.CategoryID == categoryID).ToList();
               }
               else
               {
                   list = _subCategoryFactory.GetAll().ToList();
               }
               return list;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


   }
    public class CategoryFactorys : ICategoryFactory
    {
        private IGenericFactory<INV_Category> _categoryFactory;
        Result _result = new Result();
        string tableName = "Category";

        public Result SaveCategory(INV_Category category)
        {
            _categoryFactory = new CategoryFactory();
            try
            {
                if (category.CategoryID > 0)
                {
                    _categoryFactory.Edit(category);
                    _result = _categoryFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int categoryID = 1;
                    var prvCategoryID = _categoryFactory.GetLastRecord().OrderByDescending(x => x.CategoryID).FirstOrDefault();

                    if (prvCategoryID != null)
                    {
                        categoryID = prvCategoryID.CategoryID + 1;
                    }
                    category.CategoryID = categoryID;
                    _categoryFactory.Add(category);
                    _result = _categoryFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Category> SearchCategory(int? categoryID)
        {
            _categoryFactory = new CategoryFactory();
            try
            {
                var list = new List<INV_Category>();
                if (categoryID > 0)
                {
                    list = _categoryFactory.FindBy(x => x.CategoryID == categoryID).ToList();
                }
                else
                {
                    list = _categoryFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    public class MachineFactorys : IMachineFactory
    {
        private IGenericFactory<INV_Machine> _machineFactory;
        Result _result = new Result();
        string tableName = "Machine";

        public Result SaveMachine(INV_Machine machine)
        {
            _machineFactory = new MachineFactory();
            try
            {
                if (machine.MachineID > 0)
                {
                    _machineFactory.Edit(machine);
                    _result = _machineFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int machineID = 1;
                    var prvMachineID = _machineFactory.GetLastRecord().OrderByDescending(x => x.MachineID).FirstOrDefault();

                    if (prvMachineID != null)
                    {
                        machineID = prvMachineID.MachineID + 1;
                    }
                    machine.MachineID = machineID;
                    _machineFactory.Add(machine);
                    _result = _machineFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Machine> SearchMachine(int? machineID)
        {
            _machineFactory = new MachineFactory();
            try
            {
                var list = new List<INV_Machine>();
                if (machineID > 0)
                {
                    list = _machineFactory.FindBy(x => x.MachineID == machineID).ToList();
                }
                else
                {
                    list = _machineFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
    public class ModelFactorys : IModelFactory
    {
        private IGenericFactory<INV_Model> _modelFactory;
        Result _result = new Result();
        string tableName = "Model";

        public Result SaveModel(INV_Model model)
        {
            _modelFactory = new ModelFactory();
            try
            {
                if (model.ModelID > 0)
                {
                    _modelFactory.Edit(model);
                    _result = _modelFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int machineID = 1;
                    var prvID = _modelFactory.GetLastRecord().OrderByDescending(x => x.ModelID).FirstOrDefault();

                    if (prvID != null)
                    {
                        machineID = prvID.ModelID + 1;
                    }
                    model.ModelID = machineID;
                    _modelFactory.Add(model);
                    _result = _modelFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Model> SearchModel(int? id)
        {
            _modelFactory = new ModelFactory();
            try
            {
                var list = new List<INV_Model>();
                if (id > 0)
                {
                    list = _modelFactory.FindBy(x => x.ModelID == id).ToList();
                }
                else
                {
                    list = _modelFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<INV_Model> SearchMachineWiseModel(int id)
        {
            _modelFactory = new ModelFactory();
            try
            {
                var list = new List<INV_Model>();
                if (id > 0)
                {
                    list = _modelFactory.FindBy(x => x.MachineID == id).ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class BrandFactorys : IBrandFactory
    {
        private IGenericFactory<INV_Brand> _brandFactory;
        private IGenericFactory<SET_Country> _countryFactory;
        Result _result = new Result();
        string tableName = "Brand";

        public Result SaveBrand(INV_Brand brand)
        {
            _brandFactory = new BrandFactory();
            try
            {
                if (brand.BrandID > 0)
                {
                    _brandFactory.Edit(brand);
                    _result = _brandFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int brandID = 1;
                    var prvBrandID = _brandFactory.GetLastRecord().OrderByDescending(x => x.BrandID).FirstOrDefault();

                    if (prvBrandID != null)
                    {
                        brandID = prvBrandID.BrandID + 1;
                    }
                    brand.BrandID = brandID;
                    _brandFactory.Add(brand);
                    _result = _brandFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Brand> SearchBrand(int? brandID)
        {
            _brandFactory = new BrandFactory();
            try
            {
                var list = new List<INV_Brand>();
                if (brandID > 0)
                {
                    list = _brandFactory.FindBy(x => x.BrandID == brandID).ToList();
                }
                else
                {
                    list = _brandFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SET_Country> SearchCountry(int? id)
        {
            _countryFactory = new CountryFactory();
            try
            {
                var list = new List<SET_Country>();
                if (id > 0)
                {
                    list = _countryFactory.FindBy(x => x.CountryID == id).ToList();
                }
                else
                {
                    list = _countryFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }



    public class DepartmentFactorys : IDepartmentFactory
    {
        private IGenericFactory<INV_Department> _departmentFactory;
        Result _result = new Result();
        string tableName = "Department";

        public Result SaveDepartment(INV_Department dept)
        {
            _departmentFactory = new DepartmentFactory();
            try
            {
                if (dept.DepartmentID > 0)
                {
                    _departmentFactory.Edit(dept);
                    _result = _departmentFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int deptID = 1;
                    var prvdeptID = _departmentFactory.GetLastRecord().OrderByDescending(x => x.DepartmentID).FirstOrDefault();

                    if (prvdeptID != null)
                    {
                        deptID = prvdeptID.DepartmentID + 1;
                    }
                    dept.DepartmentID = deptID;
                    _departmentFactory.Add(dept);
                    _result = _departmentFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Department> SearchDepartment(int? deptID)
        {
            _departmentFactory = new DepartmentFactory();
            try
            {
                var list = new List<INV_Department>();
                if (deptID > 0)
                {
                    list = _departmentFactory.FindBy(x => x.DepartmentID == deptID).ToList();
                }
                else
                {
                    list = _departmentFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }



    public class UnitFactorys : IUnitFactory
    {
        private IGenericFactory<INV_Unit> _unitFactory;
        Result _result = new Result();
        string tableName = "Unit";

        public Result SaveUnit(INV_Unit unit)
        {
            _unitFactory = new UnitFactory();
            try
            {
                if (unit.UnitID > 0)
                {
                    _unitFactory.Edit(unit);
                    _result = _unitFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int unitID = 1;
                    var prvUnitID = _unitFactory.GetLastRecord().OrderByDescending(x => x.UnitID).FirstOrDefault();

                    if (prvUnitID != null)
                    {
                        unitID = prvUnitID.UnitID + 1;
                    }
                    unit.UnitID = unitID;
                    _unitFactory.Add(unit);
                    _result = _unitFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Unit> SearchUnit(int? unitID)
        {
            _unitFactory = new UnitFactory();
            try
            {
                var list = new List<INV_Unit>();
                if (unitID > 0)
                {
                    list = _unitFactory.FindBy(x => x.UnitID == unitID).ToList();
                }
                else
                {
                    list = _unitFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


    public class TypeFactorys : ITypeFactory
    {
        private IGenericFactory<INV_Type> _typeFactory;
        Result _result = new Result();
        string tableName = "Type";

        public Result SaveType(INV_Type type)
        {
            _typeFactory = new TypeFactory();
            try
            {
                if (type.TypeID > 0)
                {
                    _typeFactory.Edit(type);
                    _result = _typeFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int typeID = 1;
                    var prvtypeID = _typeFactory.GetLastRecord().OrderByDescending(x => x.TypeID).FirstOrDefault();

                    if (prvtypeID != null)
                    {
                        typeID = prvtypeID.TypeID + 1;
                    }
                    type.TypeID = typeID;
                    _typeFactory.Add(type);
                    _result = _typeFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Type> SearchType(int? typeID)
        {
            _typeFactory = new TypeFactory();
            try
            {
                var list = new List<INV_Type>();
                if (typeID > 0)
                {
                    list = _typeFactory.FindBy(x => x.TypeID == typeID).ToList();
                }
                else
                {
                    list = _typeFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class StoreRackFactorys : IStoreRackFactory
    {
        private IGenericFactory<INV_StoreRack> _storeRackFactory;
        Result _result = new Result();
        string tableName = "StoreRack";

        public Result SaveStoreRack(INV_StoreRack storeRack)
        {
            _storeRackFactory = new StoreRackFactory();
            try
            {
                if (storeRack.StoreRackID > 0)
                {
                    _storeRackFactory.Edit(storeRack);
                    _result = _storeRackFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int storeRackID = 1;
                    var prvstoreRackID = _storeRackFactory.GetLastRecord().OrderByDescending(x => x.StoreRackID).FirstOrDefault();

                    if (prvstoreRackID != null)
                    {
                        storeRackID = prvstoreRackID.StoreRackID + 1;
                    }
                    storeRack.StoreRackID = storeRackID;
                    _storeRackFactory.Add(storeRack);
                    _result = _storeRackFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_StoreRack> SearchStoreRack(int? storeRackID)
        {
            _storeRackFactory = new StoreRackFactory();
            try
            {
                var list = new List<INV_StoreRack>();
                if (storeRackID > 0)
                {
                    list = _storeRackFactory.FindBy(x => x.StoreRackID == storeRackID).ToList();
                }
                else
                {
                    list = _storeRackFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    public class StoreBinFactorys : IStoreBinFactory
    {
        private IGenericFactory<INV_StoreBin> _storeBinFactory;
        Result _result = new Result();
        string tableName = "StoreBin";

        public Result SaveStoreBin(INV_StoreBin storeBin)
        {
            _storeBinFactory = new StoreBinFactory();
            try
            {
                if (storeBin.StoreBinID > 0)
                {
                    _storeBinFactory.Edit(storeBin);
                    _result = _storeBinFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.UpdateSuccessfull(tableName);
                    }
                }
                else
                {
                    int storeBinID = 1;
                    var prvstoreBinID = _storeBinFactory.GetLastRecord().OrderByDescending(x => x.StoreBinID).FirstOrDefault();

                    if (prvstoreBinID != null)
                    {
                        storeBinID = prvstoreBinID.StoreBinID + 1;
                    }
                    storeBin.StoreBinID = storeBinID;
                    _storeBinFactory.Add(storeBin);
                    _result = _storeBinFactory.Save();
                    if (_result.isSucess)
                    {
                        _result.message = _result.SaveSuccessfull(tableName);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_StoreBin> SearchStoreBin(int? storeBinID)
        {
            _storeBinFactory = new StoreBinFactory();
            try
            {
                var list = new List<INV_StoreBin>();
                if (storeBinID > 0)
                {
                    list = _storeBinFactory.FindBy(x => x.StoreBinID == storeBinID).ToList();
                }
                else
                {
                    list = _storeBinFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


    public class ProductFactorys : IProductFactory
    {
        private IGenericFactory<INV_Product> _productFactory;
        private IGenericFactory<INV_ProductDepartment> _productDeptFactory;
        private SQLFactory sqlFactory;
        private Function function;

        Result _result = new Result();
        string tableName = "Product";

        public Result SaveProduct(List<INV_ProductDepartment> productDept, INV_Product product, List<int> editProductdeptID)
        {
            _productFactory = new ProductFactory();
            _productDeptFactory = new ProductDepartmentFactory();
            try
            {
                if (product.ProductID > 0)
                {
                    _productFactory.Edit(product);
                    _result = _productFactory.Save();
                  
                    if (_result.isSucess)
                    {// delete rows during edit
                        if (editProductdeptID != null)
                        {
                            foreach (var detailsID in editProductdeptID)
                            {
                                _productDeptFactory.Delete(x => x.DepartmentID == detailsID);
                                _result = _productDeptFactory.Save();
                            }
                        }

                        if (productDept != null)
                        {
                            foreach (var dept in productDept)
                            {
                                if (dept.ProductDeptID < 1)
                                {

                                    int productDeptID = 1;
                                    var prvproductDeptID = _productDeptFactory.GetLastRecord().OrderByDescending(x => x.ProductDeptID).FirstOrDefault();

                                    if (prvproductDeptID != null)
                                    {
                                        productDeptID = prvproductDeptID.ProductDeptID + 1;
                                    }

                                    dept.ProductDeptID = productDeptID;
                                    dept.ProductID = product.ProductID;
                                    _productDeptFactory.Add(dept);
                                    _result = _productDeptFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }

                                }
                                else
                                {

                                    _productDeptFactory.Edit(dept);
                                    _result = _productDeptFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }
                                }
                            }
                           
                        }
                    
                    }

                }
                else
                {
                    int productID = 1;
                    var prvproductID = _productFactory.GetLastRecord().OrderByDescending(x => x.ProductID).FirstOrDefault();

                    if (prvproductID != null)
                    {
                        productID = prvproductID.ProductID + 1;
                    }

                    product.ProductID = productID;
                    _productFactory.Add(product);
                    _result = _productFactory.Save();
                  
                    if (_result.isSucess)
                    {
                        foreach (var dept in productDept)
                        {
                            if (dept.ProductDeptID < 1)
                            {

                                int productDeptID = 1;
                                var prvproductDeptID = _productDeptFactory.GetLastRecord().OrderByDescending(x => x.ProductDeptID).FirstOrDefault();

                                if (prvproductDeptID != null)
                                {
                                    productDeptID = prvproductDeptID.ProductDeptID + 1;
                                }

                                dept.ProductDeptID = productDeptID;
                                dept.ProductID = product.ProductID;
                                _productDeptFactory.Add(dept);
                                _result = _productDeptFactory.Save();

                                if (_result.isSucess)
                                {
                                    _result.message = _result.UpdateSuccessfull(tableName);
                                }

                            }
                            if (_result.isSucess)
                            {
                                _result.message = _result.SaveSuccessfull(tableName);
                            }
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public Result SaveCSVProduct(List<VM_ProductUpload> products)
        {
            _productFactory = new ProductFactory();
            sqlFactory = new SQLFactory();
            function = new Function();
            try
            {
                DataTable details = function.ToDataTable(products);
                SqlCommand cmd = new SqlCommand("sp_SaveCsvProduct");
                SqlParameter prmErr = new SqlParameter("@rError", SqlDbType.VarChar, 100);
                prmErr.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prmErr);

                cmd.Parameters.AddWithValue("@icsvProducts", details);
                var isSave = sqlFactory.ExecuteSP(cmd);
                if (isSave == "1")
                {
                    _result.isSucess = true;
                    _result.message = _result.SaveSuccessfull(tableName);
                }
                else
                {
                    _result.message = isSave;
                }
            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }
        public List<INV_Product> SearchProduct(int? productID)
        {
            _productFactory = new ProductFactory();
            try
            {
                var list = new List<INV_Product>();
                if (productID > 0)
                {
                    list = _productFactory.FindBy(x => x.ProductID == productID).ToList();
                }
                else
                {
                    list = _productFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_Product> SearchCategoryWiseProduct(int? categoryID)
        {
            _productFactory = new ProductFactory();
            try
            {
                var list = new List<INV_Product>();
                if (categoryID > 0)
                {
                    list = _productFactory.FindBy(x => x.CategoryID == categoryID).ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_Product> SearchProduct(int? productID, int? productType)
        {
            _productFactory = new ProductFactory();
            try
            {
                var list = new List<INV_Product>();
                if (productID > 0)
                {
                    list = _productFactory.FindBy(x => x.ProductID == productID).ToList();
                }
                else
                {
                    list = _productFactory.GetAll().ToList();
                }
                if (productType > 0)
                {
                    list = list.Where(x => x.TypeID == (int)productType).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public INV_Product GetProduct(int productID)
        {
            _productFactory = new ProductFactory();
            try
            {
                var list = new INV_Product();
                if (productID > 0)
                {
                    list = _productFactory.FindBy(x => x.ProductID == productID).FirstOrDefault();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<INV_ProductDepartment> SearchProductDepartment(int? productID)
        {
            _productDeptFactory = new ProductDepartmentFactory();
            try
            {
                var list = new List<INV_ProductDepartment>();
                if (productID > 0)
                {
                    list = _productDeptFactory.FindBy(x => x.ProductID == productID).ToList();
                }
                else
                {
                    list = _productDeptFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


    public class SupplierFactorys : ISupplierFactory
    {
        private IGenericFactory<INV_Supplier> _supplierFactory;
        private IGenericFactory<INV_SupplierProduct> _supplierProductFactory;
        private IGenericFactory<SET_Currency> _currencyFactory;

        Result _result = new Result();
        string tableName = "Supplier";

        public Result SaveSupplier(INV_Supplier supplier, List<INV_SupplierProduct> supplierProduct, List<int> deleteSupplierProductRowID)
        {
            _supplierFactory = new SupplierFactory();
            _supplierProductFactory = new SupplierDtlsFactory();
            try
            {
                if (supplier.SupplierID > 0)
                {
                    _supplierFactory.Edit(supplier);
                    _result = _supplierFactory.Save();

                    if (_result.isSucess)
                    {// delete rows during edit
                        if (deleteSupplierProductRowID != null)
                        {
                            foreach (var detailsID in deleteSupplierProductRowID)
                            {
                                _supplierProductFactory.Delete(x => x.SupplierProductID == detailsID);
                                _result = _supplierProductFactory.Save();
                            }
                        }

                        if (supplierProduct != null)
                        {
                            foreach (var supply in supplierProduct)
                            {
                                if (supply.SupplierProductID < 1)
                                {

                                    int productSupplierID = 1;
                                    var prvproductSupplierID = _supplierProductFactory.GetLastRecord().OrderByDescending(x => x.SupplierProductID).FirstOrDefault();

                                    if (prvproductSupplierID != null)
                                    {
                                        productSupplierID = prvproductSupplierID.SupplierProductID + 1;
                                    }

                                    supply.SupplierProductID = productSupplierID;
                                    supply.SupplierID = supplier.SupplierID;
                                    _supplierProductFactory.Add(supply);
                                    _result = _supplierProductFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }

                                }
                                else
                                {

                                    _supplierProductFactory.Edit(supply);
                                    _result = _supplierProductFactory.Save();

                                    if (_result.isSucess)
                                    {
                                        _result.message = _result.UpdateSuccessfull(tableName);
                                    }
                                }
                            }

                        }

                    }

                }
                else
                {
                    int supplierID = 1;
                    var prvsupplierID = _supplierFactory.GetLastRecord().OrderByDescending(x => x.SupplierID).FirstOrDefault();

                    if (prvsupplierID != null)
                    {
                        supplierID = prvsupplierID.SupplierID + 1;
                    }

                    supplier.SupplierID = supplierID;
                    _supplierFactory.Add(supplier);
                    _result = _supplierFactory.Save();

                    if (_result.isSucess)
                    {
                        foreach (var supply in supplierProduct)
                        {
                            if (supply.SupplierProductID < 1)
                            {

                                int supplierProductID = 1;
                                var prvsupplierProductID = _supplierProductFactory.GetLastRecord().OrderByDescending(x => x.SupplierProductID).FirstOrDefault();

                                if (prvsupplierProductID != null)
                                {
                                    supplierProductID = prvsupplierProductID.SupplierProductID + 1;
                                }

                                supply.SupplierProductID = supplierProductID;
                                supply.SupplierID = supplier.SupplierID;
                                _supplierProductFactory.Add(supply);
                                _result = _supplierProductFactory.Save();

                            }
                            if (_result.isSucess)
                            {
                                _result.message = _result.SaveSuccessfull(tableName);
                            }
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                _result.isSucess = false;
                _result.message = ex.Message;
            }
            return _result;
        }


        public List<INV_Supplier> SearchSupplier(int? supplierID)
        {
            _supplierFactory = new SupplierFactory();
            try
            {
                var list = new List<INV_Supplier>();
                if (supplierID > 0)
                {
                    list = _supplierFactory.FindBy(x => x.SupplierID == supplierID).ToList();
                }
                else
                {
                    list = _supplierFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<INV_SupplierProduct> SearchSupplierProduct(int? supplierID)
        {
            _supplierProductFactory = new SupplierDtlsFactory();
            try
            {
                var list = new List<INV_SupplierProduct>();
                if (supplierID > 0)
                {
                    list = _supplierProductFactory.FindBy(x => x.SupplierID == supplierID).ToList();
                }
                else
                {
                    list = _supplierProductFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SET_Currency> SearchCurrency(int? currencyID)
        {
            _currencyFactory = new CurrencyFactory();
            try
            {
                var list = new List<SET_Currency>();
                if (currencyID > 0)
                {
                    list = _currencyFactory.FindBy(x => x.CurrencyID == currencyID).ToList();
                }
                else
                {
                    list = _currencyFactory.GetAll().ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}
