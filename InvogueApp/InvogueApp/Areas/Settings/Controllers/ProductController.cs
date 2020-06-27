using BLL.Common;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;
using BLL.Models;
using DAL.db;
using InvogueApp.Areas.Settings.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Settings.Controllers
{
    public class ProductController : Controller
    {
        // GET: Settings/Product
        private IProductFactory productFactory;
        private Result result;
        public ActionResult ProductList()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Product";
            ViewBag.CallingViewPage = "#";
            return View();
        }
        public ActionResult ProductCreate()
        {
            DefaultLoad();
            return View();
        }
        public void DefaultLoad()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Product";
            ViewBag.CallingForm2 = "Add New";
            ViewBag.CallingViewPage = "#!/ProductList";
        }

        [HttpPost]
        public JsonResult SaveProduct()
        {
            result = new Result();
            productFactory = new ProductFactorys();
            var product = Request.Form["product"];
            var productFile = JsonConvert.DeserializeObject<VM_ProductModel>(product);

            result = productFactory.SaveProduct(productFile.ProductDept, productFile.Product, productFile.editProductdeptID);
            string path = Server.MapPath("~/Content/product_Img/product/Image/" + productFile.Product.ProductID + "/");
            try
            {
               
                if (result.isSucess)
                {
                    if (Request.Files.Count > 0)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var uniqueName = productFile.Product.ProductID.ToString();
                        foreach (string key in Request.Files)
                        {
                            HttpPostedFileBase file = Request.Files[key];
                            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName) ?? "";
                            var fileName = Path.GetFileName(file.FileName) ?? "";
                            var newFileName = fileName.Replace(fileNameWithoutExt, uniqueName);
                            var fileNameWithoutExt2 = Path.GetFileNameWithoutExtension(newFileName);
                            string fullPath = path + newFileName;

                            DirectoryInfo directory2 = new DirectoryInfo(path);
                            if (directory2.Exists)
                            {
                                var fileGetName2 = directory2.EnumerateFiles().Where(x => x.Name.ToLower() == (fileNameWithoutExt2.ToLower() + ".png") || x.Name.ToLower() == (fileNameWithoutExt2.ToLower() + ".jpg") || x.Name.ToLower() == (fileNameWithoutExt2.ToLower() + ".jpeg")).Select(x => x.Name).FirstOrDefault();
                                if (fileGetName2 != null)
                                {
                                    var delPath = path + fileGetName2;
                                    System.IO.File.Delete(delPath);
                                }
                                else
                                {
                                    if (System.IO.File.Exists(fullPath))
                                    {
                                        System.IO.File.Delete(fullPath);
                                    }
                                }
                            }
                            file.SaveAs(fullPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.isSucess = false;
                result.message = ex.InnerException.Message;
            }

            return Json(result);
        }
           [HttpPost]
        public JsonResult LoadProduct(int? productID)
        {
            productFactory = new ProductFactorys();
            List<INV_Product> list = productFactory.SearchProduct(productID);
            var productList = list.Select(x => new
            {
                x.Name,
                x.ProductID,
                x.UnitID,
                x.TypeID,
                x.BranchID,
                x.MachineID,
                x.ModelID,
                BranchName = x.SET_CompanyBranch.Name,
                //x.SerialNO,
                x.CategoryID,
                CataGory = x.INV_Category.Name,
                unitName = x.INV_Unit == null ? "" : x.INV_Unit.Name,
                typeName = x.INV_Type.Name,
                x.Code,
                x.ImportTypeID,
                ImportType = x.ImportTypeID == 1 ? "Local" : "Import",
                x.SubCategoryID,
                x.Status
            });


            //var jsonResult = Json(productList, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadProductWithPagenation(string Namevalue, int Pageindex, int Pagesize)
        {
            List<VM_SearchwithPageNationProduct> list = new List<VM_SearchwithPageNationProduct>();
            var departmnetName = "";
            var departmentWithoutComa = "";
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", Namevalue);
            SqlParameter p2 = new SqlParameter("@P2", Pageindex);
            SqlParameter p3 = new SqlParameter("@P3", Pagesize);
            var productList = db.Database.SqlQuery<VM_SearchwithPageNationProduct>("SP_ProductSearchPagination @p1,@P2,@P3", p1, p2, p3).ToList();
            int productCount = Convert.ToInt32(productList.Select(x=>x.TotalProduct).FirstOrDefault());
            foreach (var details in productList)
            {
                var productWiseDepartment = (from p in db.INV_Product
                    join pd in db.INV_ProductDepartment on p.ProductID equals pd.ProductID
                    join d in db.INV_Department on pd.DepartmentID equals d.DepartmentID
                    where pd.ProductID == details.ProductID
                    select new
                    {
                        d.Name
                    }).ToList();
                foreach (var department in productWiseDepartment)
                {
                    departmnetName += department.Name + "," + " ";
                }

                if (departmnetName != "")
                {
                    departmentWithoutComa = departmnetName.Substring(0, departmnetName.Length - 2);
                }
                
                VM_SearchwithPageNationProduct product = new VM_SearchwithPageNationProduct();
                product.Name = details.Name;
                product.ProductID = details.ProductID;
                product.UnitID = details.UnitID;
                product.unitName = details.unitName;
                product.typeName = details.typeName;
                product.TypeID = details.TypeID;
                product.BranchID = details.BranchID;
                product.BranchName = details.BranchName;
                product.MachineID = details.MachineID;
                product.CataGory = details.CataGory;
                product.CategoryID = details.CategoryID;
                product.SubCategoryID = details.SubCategoryID;
                product.Status = details.Status;
                product.ImportTypeID = details.ImportTypeID;
                product.Code = details.Code;
                product.CountryID = details.CountryID;
                product.PartNumber = details.PartNumber;
                product.SerialNO = details.SerialNO;
                product.DepartmentName = departmentWithoutComa;
                product.ImageURL = details.ImageURL;
                list.Add(product);

                departmnetName = "";
                departmentWithoutComa = "";
            }

            var datalist = new
            {
                productLists = list.OrderByDescending(x => x.ProductID),
                productCounts = productCount
            };
            return Json(datalist, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult LoadProductForDropdown(string name){
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", name);
            var trialList = db.Database.SqlQuery<VM_SearchProduct>("sp_ProductSearch @p1", p1).ToList();
            return Json(trialList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadCategoryWiseProduct(int? categoryID,string name)
        {
           
            DevourInvEntities db = new DevourInvEntities();
            SqlParameter p1 = new SqlParameter("@P1", name);
            SqlParameter p2 = new SqlParameter("@P2", categoryID);
            var trialList = db.Database.SqlQuery<VM_SearchProduct>("sp_ProductSearch @p1,@p2", p1,p2).ToList();
            return Json(trialList, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public async Task<ActionResult> LoadCategoryWiseProduct(int? categoryID)
        //{

        //    DevourInvEntities db = new DevourInvEntities();
        //    var list = await db.INV_Product.FindAsync();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult LoadFinishGoods(int? productID)
        {
            productFactory = new ProductFactorys();
            List<INV_Product> list = productFactory.SearchProduct(productID);
            var productList = list.Select(x => new
            {
                x.Name,
                x.ProductID,
                x.UnitID,
                x.TypeID,
                x.SerialNO,
                x.CategoryID,
                CataGory = x.INV_Category.Name,
                unitName = x.INV_Unit == null ? "" : x.INV_Unit.Name,
                typeName = x.INV_Type.Name,
                x.Code,
                x.SubCategoryID,
                x.Status,
                x.ImageURL

            }).Where(x=>x.TypeID == 2);

            foreach (var id in list)
            {

                var Eimg = GetEmpPhoto(id.ProductID.ToString());
                // id.ImgFullPathName = Eimg.ImgFullPathName;

                id.ImageURL = Eimg.ImageURL;
            }

            return Json(productList.OrderByDescending(x => x.ProductID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadRawMetarials(int? packageID)
        {
            int? productID = null;
            productFactory = new ProductFactorys();
            List<INV_Product> list = productFactory.SearchProduct(productID);
            var productList = list.Select(x => new
            {
                x.Name,
                x.ProductID,
                x.UnitID,
                x.TypeID,
                x.SerialNO,
                x.CategoryID,
                CataGory = x.INV_Category.Name,
                unitName = x.INV_Unit == null ? "" : x.INV_Unit.Name,
                typeName = x.INV_Type.Name,
                x.Code,
                x.ImportTypeID,
                x.SubCategoryID,
                x.Status,
                x.ImageURL

            }).Where(x => x.TypeID == 1);

            foreach (var id in list)
            {

                var Eimg = GetEmpPhoto(id.ProductID.ToString());
                // id.ImgFullPathName = Eimg.ImgFullPathName;

                id.ImageURL = Eimg.ImageURL;
            }

            return Json(productList.OrderByDescending(x => x.ProductID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public VM_ForProductImg GetEmpPhoto(string code)
        {
            string path = Server.MapPath("~/Content/product_Img/product/Image/" + code + "/");
            DirectoryInfo directory = new DirectoryInfo(path);
            VM_ForProductImg fileData = new VM_ForProductImg();
            if (directory.Exists)
            {
                var file = directory.EnumerateFiles().FirstOrDefault();   //.Where(x => x.Name.Contains("Photo")).Select(x => new { x.Name, x.FullName })
                if (file == null)
                {
                    string paths = Server.MapPath("~/Content/product_Img/product/Image/");
                    DirectoryInfo directorys = new DirectoryInfo(paths);

                    file = directorys.EnumerateFiles().FirstOrDefault();   //.Where(x => x.Name.Contains("empPhoto")).Select(x => new { x.Name, x.FullName })
                }

              //  fileData.ImgPathName = file.Name;
                fileData.ImageURL = file.Name;
                return fileData;

            }
            else
            {
                string paths = Server.MapPath("~/Content/user_define/EmpDemoImg/empPhoto.png");
                DirectoryInfo directorys = new DirectoryInfo(paths);

                var file = directorys.EnumerateFiles().FirstOrDefault(); 
                // .Where(x => x.Name.Contains("empPhoto")).Select(x => new { x.Name, x.FullName })
                if (file != null)
                {
                    // fileData.ImgPathName = file.Name;
                    fileData.ImageURL = file.Name;
                }
              
                return fileData;
            }

        }

        [HttpPost]
        public JsonResult LoadProductDept(int? productDeptID)
        {
            productFactory = new ProductFactorys();
            List<INV_ProductDepartment> list = productFactory.SearchProductDepartment(productDeptID);
            var pdeptList = list.Select(x => new
            {
               x.ProductDeptID,
               x.ProductID,
               x.DepartmentID
            });
            return Json(pdeptList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetProductImgFile(string productID)
        {
            string path = Server.MapPath("~/Content/product_Img/product/Image/" + productID + "/");
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                var fileData = directory.EnumerateFiles().Where(x => x.Name == productID + ".jpg" || x.Name == productID + ".png" || x.Name == productID + ".pdf" || x.Name == productID + ".jpeg").Select(x => new { x.Name, x.FullName });
                return Json(fileData);
            }
            return Json("");
        }

        public ActionResult ProductUpload()
        {
            ViewBag.CallingForm = "Settings";
            ViewBag.CallingForm1 = "Upload Product";
            ViewBag.CallingViewPage = "#";
            return View();
        }

        [HttpPost]
        public JsonResult UploadProduct()
        {
            DataTable dt = new DataTable();
            result = new Result();
            productFactory = new ProductFactorys();
            List<VM_ProductUpload> uploadedProductList = new List<VM_ProductUpload>();
            //bool isSucess = false;
            if (Request.Files.Count > 0)
            {
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[key];
                    string fileName = Path.GetFileName(file.FileName);
                    string csvPath = Server.MapPath("~/Content/user_files/product_csv/") + fileName;
                    if (System.IO.File.Exists(csvPath))
                    {
                        System.IO.File.Delete(csvPath);
                    }
                    file.SaveAs(csvPath);
                    dt = ReadToEnd(csvPath);
                }

                string category = "";
                List<VM_ProductUpload> products = new List<VM_ProductUpload>();
                foreach (DataRow row in dt.Rows)
                {
                    
                    VM_ProductUpload item = GetItem<VM_ProductUpload>(row);
                    if (item.Category != string.Empty)
                    {
                        category = item.Category;
                    }
                    else
                    {
                        item.Category = category;
                    }
                  
                    if (item.Product_Code == null && item.Product_Name == null && item.Unit == null)
                    {
                        continue;
                    }
                    products.Add(item);
                }


                result = productFactory.SaveCSVProduct(products);
                if (result.isSucess)
                {
                    LoadProduct(0);
                }
            }

            return Json(result);
        }

        private DataTable ReadToEnd(string filePath)
        {
            DataTable dtDataSource = new DataTable();
            string[] fileContent = System.IO.File.ReadAllLines(filePath);
            if (fileContent.Count() > 0)
            {
                string[] columns = fileContent[0].Split(',');
                for (int i = 0; i < columns.Count(); i++)
                {
                    dtDataSource.Columns.Add(columns[i]);
                }
                for (int i = 1; i < fileContent.Count(); i++)
                {
                    string[] rowData = fileContent[i].Split(',');
                    dtDataSource.Rows.Add(rowData);
                }
            }
            return dtDataSource;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        } 
    }
    //=====================================Product Search Model ============================================
    public class VM_SearchProduct
    {

        public string Name { get; set; }
        public int ProductID { get; set; }
        public int? UnitID { get; set; }
        public int TypeID { get; set; }
        public int CategoryID { get; set; }
        public int Status { get; set; }
    }
    public class VM_SearchwithPageNationProduct
    {

        public string Name { get; set; }
        public int ProductID { get; set; }
        public int? UnitID { get; set; }
        public int TypeID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int Status { get; set; }
        public int ImportTypeID { get; set; }
        public int CountryID { get; set; }

        public int BranchID { get; set; }
        public int MachineID { get; set; }
        public int ModelID { get; set; }
        public string BranchName { get; set; }

        public int TotalProduct { get; set; }

        public string Code { get; set; }
        public string CataGory { get; set; }
        public string unitName { get; set; }
        public string typeName { get; set; }
        public string PartNumber { get; set; }
        public string SerialNO { get; set; }
        public string ImageURL { get; set; }
        public string DepartmentName { get; set; }
        
    }
}