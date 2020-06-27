using Application.Common;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace InvogueApp.Areas.Settings.Api
{
    public class ProductController : ApiController
    {
        static readonly IProductFactory productFactory = new ProductFactorys();

        [HttpPost]
        public List<ProductModal> GetAllProducts(int? productID, int? typeID, string token)
        {
            var products = new List<ProductModal>();
            if (Constants.Token == token)
            {
                var productList = productFactory.SearchProduct(productID, typeID);
                foreach (var product in productList)
                {
                    var pModal = new ProductModal();
                    pModal.ProductID = product.ProductID;
                    pModal.CategoryID = product.CategoryID;
                    pModal.SubCategoryID = product.SubCategoryID;
                    pModal.Name = product.Name;
                    pModal.Code = product.Code;
                    pModal.TypeID = product.TypeID;
                    pModal.UnitID = product.UnitID;
                    pModal.SerialNO = product.SerialNO;
                    pModal.ImageURL = product.ImageURL;
                    pModal.Status = product.Status;
                    var sizeList = new List<ProductSizeModal>();
                    foreach (var size in product.INV_ProductSize)
                    {
                        var sz = new ProductSizeModal();
                        sz.ProductSizeID = size.ProductSizeID;
                        sz.ProductID = size.ProductID;
                        sz.SizeID = size.SizeID;
                        sizeList.Add(sz);
                    }
                    pModal.ProductSizeModal = sizeList;
                    products.Add(pModal);
                }
                return products;
            }
            return products;            
        }
    }
    public class ProductModal
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int? SubCategoryID { get; set; }
        public int PackageID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string GlobalCode { get; set; }
        public int? BrandID { get; set; }
        public int? ColorID { get; set; }
        public int? TypeID { get; set; }
        public int? UnitID { get; set; }
        public string SerialNO { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Gender { get; set; }
        public string ImageURL { get; set; }
        public int Status { get; set; }
        public virtual List<ProductSizeModal> ProductSizeModal { get; set; }
    }
    public class ProductSizeModal
    {
        public int ProductSizeID { get; set; }
        public int ProductID { get; set; }
        public int SizeID { get; set; }

    }
}
