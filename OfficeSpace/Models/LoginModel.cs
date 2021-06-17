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
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        //[Required]
        //public string CurrentPassword { get; set; }

     
        public string Validate()
        {
            string result = string.Empty;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format("SELECT RoleName FROM UserDetails WHERE Username = '{0}' and Password = '{1}' and IsActive = 1",Username,EncryptPass(Password));
                var obj = command.ExecuteScalar();
                result = obj == null ? string.Empty : obj.ToString();
            }

            return result;
        }

      
        private string EncryptPass(string Newpassword)
        {
            string msg = "";
            byte[] encode = new byte[Newpassword.Length];
            encode = Encoding.UTF8.GetBytes(Newpassword);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

      
    }

    
}