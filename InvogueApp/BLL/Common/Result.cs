using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public class Result
    {
        public string message { get; set; }
        public bool isSucess { get; set; }
        public Int64 lastInsertedID { get; set; }
        public string lastInsertedCode { get; set; }

        public string SaveSuccessfull(string tableName)
        {
            return tableName + " Save Successful !!!";;
        }
        public string UpdateSuccessfull(string tableName)
        {
            return tableName + " Update Successful !!!";
        }
        public string DeleteSuccessfull(string tableName)
        {
            return tableName + " Delete Successful !!!";
        }
        public string CancelSuccessfull(string tableName)
        {
            return tableName + " Cancel Successful !!!";
        }
        public string Exists(string tableName)
        {
            return tableName + "This " + tableName+ " Already Exists! Try Another One !!!";
        }
    }

    public enum Status
    {
        Created = 1,
        Approve = 2,
        Pending = 3,
        Cancel = 4,
        Updated = 5,
        PartialComplete = 6,
        Complete = 7
    }
}
