using BLL.Common;
using BLL.Factory;
using DAL.db;
using DAL.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvogueApp.Areas.Inventory.Controllers
{
    public class OpeningStockController : Controller
    {
        // GET: Inventory/OpeningStock
        private SQLFactory sqlFactory;
        private Function function;
        Result _result = new Result();
        public ActionResult OpeningStock()
        {
            ViewBag.CallingForm = "OpeningStock";
            ViewBag.CallingForm1 = "Opening Stock";
            ViewBag.CallingForm2 = "Add New";

            return View();
        }

        [HttpPost]
        public JsonResult SaveStockIn(List<INV_OpeningStockDtls> oStock, INV_OpeningStock stockO)
        {
           _result = new Result();
            sqlFactory = new SQLFactory();
            function = new Function();
            string tableName = "Opening Stock";
            try
            {
                Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
                int userid = Convert.ToInt32(dictionary[3].Id == "" ? 0 : Convert.ToInt32(dictionary[3].Id));
                stockO.CreatedBy=userid;
                SqlCommand cmd = new SqlCommand("sp_SaveOpeningStock");
                SqlParameter prmErr = new SqlParameter("@rError", SqlDbType.VarChar, 200);
                prmErr.Direction = ParameterDirection.Output;
                DataTable details = function.ToDataTable(oStock);
                cmd.Parameters.Add(prmErr);
                cmd.Parameters.AddWithValue("@iOpeningDetails", details);
                //cmd.Parameters.AddWithValue("@iOpeningStockID", oStock.OpeningStockID);

                //cmd.Parameters.AddWithValue("@iProductID", oStock.ProductID);
                //cmd.Parameters.AddWithValue("@iCategoryID", oStock.CategoryID);
                //cmd.Parameters.AddWithValue("@iUnitID", oStock.UnitID);
                //cmd.Parameters.AddWithValue("@iQuantity", oStock.Quantity);
                cmd.Parameters.AddWithValue("@iBranchID", stockO.BranchID);
                cmd.Parameters.AddWithValue("@iCreatedBy", stockO.CreatedBy);
                cmd.Parameters.AddWithValue("@iCreatedDate", stockO.CreatedDate);
                
                var isSave = sqlFactory.ExecuteSP(cmd);

                var error = cmd.Parameters["@rError"].Value.ToString();
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
            return Json(_result);
           
        }
    }
    public partial class INV_OpeningStockDtls
    {
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public decimal Quantity { get; set; }
   
    }
}