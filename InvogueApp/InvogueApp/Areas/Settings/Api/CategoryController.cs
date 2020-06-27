using Application.Common;
using BLL.Factory.Settings;
using BLL.Interfaces.Settings;
using DAL.db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace InvogueApp.Areas.Settings.Api
{
    public class CategoryController : ApiController
    {
        static readonly ICategoryFactory categoryFactory = new CategoryFactorys();
        [HttpPost]
        public List<Category> GetCategory(int? categoryID, string token)
        {
            var categoryModal = new List<Category>();
            if (Constants.Token == token)
            {
               var category = categoryFactory.SearchCategory(categoryID);
                foreach (var cat in category)
                {
                    var cModal = new Category();
                    cModal.CategoryID = cat.CategoryID;
                    cModal.Code = cat.Code;
                    cModal.Name = cat.Name;
                    cModal.Status = cat.Status;
                    categoryModal.Add(cModal);
                }
                return categoryModal;
            }
            return categoryModal;
        }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
    }
}
