using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace OfficeSpace
{
    public class BAL
    {
        public List<Company> GetCompanyList(string roleName, string userName=null)
        {
            List<Company> result = new List<Company>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (roleName == "User")
                {
                    
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "'  and  RoleName = 'User') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                else
                {
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Company { CompanyId = reader["CompanyName"].ToString(), CompayName = reader["CompanyName"].ToString() });
                        }
                    }
                }

            }
            return result;
        }

        public string UpdateFurnitureDetails(DataTable dt)
        {
            string result = "Failed";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_UpdateFurnishedRentalOfficesDetails";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@UpdateData";
                    param.TypeName = "dbo.FurnitureUpdateType";
                    param.Value = dt;
                    command.Parameters.Add(param);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        result = "Success";

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public Dictionary<string, bool> FetchFieldEditConfiguration(string tableName)

        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "UDP_FetchFieldEditConfiguration";
                command.Parameters.Add(new SqlParameter() { ParameterName = "@TableName", Value = tableName, DbType = System.Data.DbType.String });
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(Convert.ToString(reader["FieldName"].ToString()), Convert.ToBoolean(reader["Editable"].ToString()));
                    }
                }
            }
            return result;
        }

    }
}
    