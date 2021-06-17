using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace OfficeSpace.Models
{
    public class EmailModel
    {

        [Required]
        public string EmailID { get; set; }

        private string DecodePass(string password)
        {
            string msg = "";
            byte[] Decode = new byte[password.Length];
            Decode = Convert.FromBase64String(password);
            msg = Encoding.UTF8.GetString(Decode);
            return msg;
        }       

        public string[] GetPasswordString()
        {
            string[] details = new string[2];            
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format("SELECT Username,Password FROM UserDetails WHERE CompanyEmail = '{0}' and IsActive = 1", EmailID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        details[0] = reader[0].ToString();
                        details[1] = DecodePass(reader[1].ToString());

                    }
                }
                reader.Close();


            }
            return details;
        }

    }
    
}