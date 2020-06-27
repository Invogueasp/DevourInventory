using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class Common
    {
        public static List<T> ConvertDataTableToObject<T>(DataTable dt)
        {
            // Get all public fields
            var fields = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];

                            // Set the value into the object
                            if (value.ToString() != string.Empty)
                            {
                                fieldInfo.SetValue(ob, value);
                            }                            
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }
            return lst;
        }
    }
}