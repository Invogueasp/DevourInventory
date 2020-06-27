using BLL.Common;
using BLL.Models;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Settings
{
    public interface ICompanyFactory
    {
        Result SaveCompany(SET_Company company);
        List<SET_Company> SearchCompany(int? companyID);
    }
    public interface ICategoryFactory
    {
        Result SaveCategory(INV_Category category);
        List<INV_Category> SearchCategory(int? categoryID);
    }

    public interface ISubCategoryFactory
    {
        Result SaveSubCategory(INV_SubCategory subcategory);
        List<INV_SubCategory> SearchSubCategory(int? subCategoryID);
        List<INV_SubCategory> IdWiseSubCategory(int? categoryID);
    }
    public interface IMachineFactory
    {
        Result SaveMachine(INV_Machine machine);
        List<INV_Machine> SearchMachine(int? machineID);
    }
    public interface IModelFactory
    {
        Result SaveModel(INV_Model model);
        List<INV_Model> SearchModel(int? id);
        List<INV_Model> SearchMachineWiseModel(int id);
    }
    public interface IBrandFactory
    {
        Result SaveBrand(INV_Brand brand);
        List<INV_Brand> SearchBrand(int? brand);

        List<SET_Country> SearchCountry(int? id);
    }
    public interface IDepartmentFactory
    {
        Result SaveDepartment(INV_Department dept);
        List<INV_Department> SearchDepartment(int? deptID);
    }
    public interface IUnitFactory
    {
        Result SaveUnit(INV_Unit unit);
        List<INV_Unit> SearchUnit(int? unitID);
    }

    public interface ITypeFactory
    {
        Result SaveType(INV_Type type);
        List<INV_Type> SearchType(int? typeID);
    }
    public interface IProductFactory
    {
        Result SaveProduct(List<INV_ProductDepartment> productDepartment, INV_Product product, List<int> editProductdeptID);
        Result SaveCSVProduct(List<VM_ProductUpload> products);
        List<INV_Product> SearchProduct(int? productID);
        List<INV_Product> SearchCategoryWiseProduct(int? categoryID);
        List<INV_Product> SearchProduct(int? productID, int? productType);
        INV_Product GetProduct(int productID);

        List<INV_ProductDepartment> SearchProductDepartment(int? productID);
    }
    public interface IStoreRackFactory
    {
        Result SaveStoreRack(INV_StoreRack storeRack);
        List<INV_StoreRack> SearchStoreRack(int? storeRackID);
    }
    public interface IStoreBinFactory
    {
        Result SaveStoreBin(INV_StoreBin storeBin);
        List<INV_StoreBin> SearchStoreBin(int? storeBinID);
    }

    public interface ISupplierFactory
    {
        Result SaveSupplier(INV_Supplier supplier, List<INV_SupplierProduct> supplierProduct, List<int> deleteSupplierProductRowID);
        List<INV_Supplier> SearchSupplier(int? supplierID);
        List<INV_SupplierProduct> SearchSupplierProduct(int? supplierID);
        List<SET_Currency> SearchCurrency(int? CurrencyID);
    }

   
    public interface ICompanyBranchFactory 
    {
        //Result SaveCompanyBranch(SET_CompanyBranch companyBranch, List<SET_BranchDepartment> branchDeptList, List<int> editDeleteBranchDeptRowID, List<int> editDeleteBranchShiftRowID);
        List<SET_CompanyBranch> SearchCompanyBranch(int? id);
        //List<SET_BranchDepartment> SearchBranchDepartment(int branchID);

    }
}
