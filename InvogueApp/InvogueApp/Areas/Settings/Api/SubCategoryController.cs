using Application.Common;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InvogueApp.Areas.Settings.Api
{
    public class SubCategoryController : ApiController
    {
        static readonly ISubCategoryFactory subCategoryFactory = new SubCategoryFactorys();
        [HttpPost]
        public List<SubCategory> GetSubCategory(int? subCategoryID, string token)
        {
            var subCategoryModal = new List<SubCategory>();
            if (Constants.Token == token)
            {
                var subCategorys = subCategoryFactory.SearchSubCategory(subCategoryID);
                foreach (var subCategory in subCategorys)
                {
                    var scModal = new SubCategory();
                    scModal.SubCategoryID = subCategory.SubCategoryID;
                    scModal.CategoryID = subCategory.CategoryID;
                    scModal.Name = subCategory.Name;
                    scModal.Code = subCategory.Code;
                    scModal.Status = subCategory.Status;
                    subCategoryModal.Add(scModal);
                }
                return subCategoryModal;
            }
            return subCategoryModal;
        }
    }

    public class SubCategory
    {
        public int SubCategoryID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
    }
}
