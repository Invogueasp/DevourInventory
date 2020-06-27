using DAL.Common;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace BLL.Master
{
    public class DBOrganization
    {
        DevourInvEntities db = new DevourInvEntities();

        public int Submit(OrganizationCore organizationcore)
        {
            int i = 1; 

            try
            {

                SqlCommand iSqlCommand = new SqlCommand("sp_Organization_Insert", DataManager.cnConnection);
                iSqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter prmErr = new SqlParameter("@rErr", SqlDbType.Int);
                prmErr.Direction = ParameterDirection.Output;

                iSqlCommand.Parameters.AddWithValue("@iOrganizationName", organizationcore.OrganizationName);
                iSqlCommand.Parameters.AddWithValue("@iRegisteredPerson", organizationcore.RegisteredPerson ?? SqlString.Null); //string.IsNullOrEmpty(organizationcore.DeliveryDate.ToString()) == true ? dbNull.ToString() : organizationcore.DeliveryDate.ToString());
                iSqlCommand.Parameters.AddWithValue("@iOrganizationMode", organizationcore.OrganizationMode);
                iSqlCommand.Parameters.AddWithValue("@iOrganizationAddress", organizationcore.OrganizationAddress ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iPosTCode", organizationcore.PosTCode ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iCity", organizationcore.City);
                iSqlCommand.Parameters.AddWithValue("@iCountry", organizationcore.Country);
                iSqlCommand.Parameters.AddWithValue("@iPhone", organizationcore.Phone ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iFax", organizationcore.Fax ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iEmail", organizationcore.Email ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iOrganizationLogo", organizationcore.OrganizationLogo);
                iSqlCommand.Parameters.AddWithValue("@iWeb", organizationcore.Web ?? SqlString.Null);

                iSqlCommand.Parameters.Add(prmErr);
                DataManager.cnConnection.Open();

                i = iSqlCommand.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                i = -1;
            }
            finally
            {
                DataManager.cnConnection.Close();
            }

            return i;
        }
        public int Save(OrganizationCore organizationcore)
        {
            int i = 1;

            try
            {
                SqlCommand iSqlCommand = new SqlCommand("sp_Organization_Update", DataManager.cnConnection);
                iSqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter prmErr = new SqlParameter("@rErr", SqlDbType.Int);
                prmErr.Direction = ParameterDirection.Output;

                iSqlCommand.Parameters.AddWithValue("@iOrganizationId", organizationcore.OrganizationId);
                iSqlCommand.Parameters.AddWithValue("@iOrganizationName", organizationcore.OrganizationName);
                iSqlCommand.Parameters.AddWithValue("@iRegisteredPerson", organizationcore.RegisteredPerson ?? SqlString.Null); //string.IsNullOrEmpty(organizationcore.DeliveryDate.ToString()) == true ? dbNull.ToString() : organizationcore.DeliveryDate.ToString());
                iSqlCommand.Parameters.AddWithValue("@iOrganizationMode", organizationcore.OrganizationMode);
                iSqlCommand.Parameters.AddWithValue("@iOrganizationAddress", organizationcore.OrganizationAddress ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iPosTCode", organizationcore.PosTCode ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iCity", organizationcore.City);
                iSqlCommand.Parameters.AddWithValue("@iCountry", organizationcore.Country);
                iSqlCommand.Parameters.AddWithValue("@iPhone", organizationcore.Phone ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iFax", organizationcore.Fax ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iEmail", organizationcore.Email ?? SqlString.Null);
                iSqlCommand.Parameters.AddWithValue("@iOrganizationLogo", organizationcore.OrganizationLogo);
                iSqlCommand.Parameters.AddWithValue("@iWeb", organizationcore.Web ?? SqlString.Null);

                iSqlCommand.Parameters.Add(prmErr);
                DataManager.cnConnection.Open();

                i = iSqlCommand.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                i = -1;
            }
            finally
            {
                DataManager.cnConnection.Close();
            }

            return i;
        }
        public int Insert(OrganizationCore organizationcore)
        {
            int i = -1;
            db.OrganizationCores.Add(organizationcore);
            i = db.SaveChanges();
            return i;
        }

        public int Update(OrganizationCore organizationcore)
        {
            int i = -1;
            db.Entry(organizationcore).State = EntityState.Modified;
            i = db.SaveChanges();
            return i;
        }
    }
}